using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class WebSpawner : MonoBehaviour
{
    public GameObject webPrefab;
    public GameObject cocoonPrefab;
    public LayerMask raycastMask;
    public float minWebLength;
    public float maxWebLength;
    public float minNodeDistance;
    public float pullValue;
    private Vector3 endPoint;


    private Vector3 mousePos;
    private Vector3 worldMousePos;
    public float animationSpeed = 3f;

    public WebNode SpawnWeb(Vector3 origin, WebInfo attachedTo)
    {
        mousePos = Input.mousePosition;

        mousePos.z = Camera.main.WorldToScreenPoint(origin).z;
        worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        Vector3 dir = worldMousePos - origin;
        RaycastHit hitForward;
        RaycastHit hitBackward;

        if (Physics.Raycast(origin, dir.normalized, out hitForward, maxWebLength, raycastMask))
        {

            // If you hit a bug
            if (hitForward.collider.gameObject.layer == 11)
            {
                Debug.Log("Bug Hit!");
                hitForward.collider.gameObject.tag = "BugToCocoon";
                InstantiateWeb(origin, hitForward.point, null, null, true, false, true);
                return null;
            }

            #region  Start Node

            WebNode closestToOrigin = findClosestNodeTo(origin);
            bool originCloseToNode = closestToOrigin != null ? Vector3.Distance(closestToOrigin.pos, origin) < minNodeDistance : false;

            WebNode mergedStartNode = null;
            if (attachedTo != null && !originCloseToNode)
            {
                origin += dir.normalized * pullValue;
                mergedStartNode = SplitWeb(attachedTo, origin);
            }
            else if (originCloseToNode)
            {
                mergedStartNode = closestToOrigin;
            }
            else if (Physics.Raycast(origin, -dir.normalized, out hitBackward, 1.0f, raycastMask))
            {
                origin = hitBackward.point;
            }
            else
            {
                origin -= new Vector3(0, 0.5f, 0);
            }
            #endregion

            #region End Node
            endPoint = hitForward.point;

            WebNode closestToEnd = findClosestNodeTo(endPoint);
            bool endCloseToNode = closestToEnd != null ? Vector3.Distance(closestToEnd.pos, endPoint) < minNodeDistance : false;

            if (Vector3.Distance(origin, endPoint) < minWebLength)
            {
                return null;
            }
            WebInfo otherWeb = hitForward.transform.GetComponent<WebInfo>();
            WebNode mergedEndNode = null;
            if (otherWeb != null && !endCloseToNode)
            {
                endPoint -= dir.normalized * pullValue;
                mergedEndNode = SplitWeb(otherWeb, endPoint);
            }
            else if (endCloseToNode)
            {
                mergedEndNode = closestToEnd;
            }
            #endregion
            return InstantiateWeb(origin, endPoint, mergedStartNode, mergedEndNode);
        }
        else
        {
            return null;
        }
    }

    //returns new WebNode
    public WebNode SplitWeb(WebInfo other, Vector3 contactPoint)
    {
        WebNode mergedNode = new WebNode(contactPoint);

        //old start node
        WebNode oldStartNode = other.start;
        oldStartNode.removeAdjacent(other.end);
        oldStartNode.addAdjacent(mergedNode);

        //old end node
        WebNode oldEndNode = other.end;
        oldEndNode.removeAdjacent(other.start);
        oldEndNode.addAdjacent(mergedNode);

        //merged node
        mergedNode.addAdjacent(oldEndNode);
        mergedNode.addAdjacent(oldStartNode);

        InstantiateWeb(other.start.pos, contactPoint, oldStartNode, mergedNode, false, false);
        InstantiateWeb(contactPoint, other.end.pos, mergedNode, oldEndNode, false, false);
        Destroy(other.gameObject);

        return mergedNode;
    }

    WebNode InstantiateWeb(Vector3 start, Vector3 end, WebNode startNode = null, WebNode endNode = null, bool runAnimations = true, bool setAdjacencies = true, bool deleteAfter = false)
    {
        GameObject web = Instantiate(webPrefab);
        web.transform.position = start + ((end - start) / 2);
        web.transform.LookAt(end);
        web.transform.Rotate(90, 0, 0);

        WebInfo webScript = web.GetComponent<WebInfo>();
        if (webScript != null)
        {
            if (startNode == null && !deleteAfter)
            {
                webScript.start = new WebNode(start);
            }
            else
            {
                webScript.start = startNode;
            }

            if (endNode == null && !deleteAfter)
            {
                webScript.end = new WebNode(end);
            }
            else
            {
                webScript.end = endNode;
            }

            if (setAdjacencies)
            {
                webScript.start.addAdjacent(webScript.end);
                webScript.end.addAdjacent(webScript.start);
            }
        }
        else
        {
            Debug.Log("script not found");
        }
        if (runAnimations)
        {
            StartCoroutine(animate(web, start, end, deleteAfter));
        }
        else
        {
            web.transform.localScale = new Vector3(web.transform.localScale.x, Vector3.Distance(start, end) / 2, web.transform.localScale.z);
        }
        return webScript.start;
    }

    private IEnumerator animate(GameObject web, Vector3 start, Vector3 end, bool deleteAfter = false)
    {
        float targetLength = Vector3.Distance(start, end) / 2;
        float currLength = 0;
        Vector3 initScale = web.transform.localScale;

        while (currLength < targetLength)
        {
            currLength += animationSpeed * Time.deltaTime;
            float actualLength = Mathf.Min(currLength, targetLength);
            web.transform.localScale = new Vector3(initScale.x, actualLength, initScale.z);

            Vector3 midpoint = start + (end - start).normalized * actualLength * 2 / 2;
            web.transform.position = midpoint;
            yield return null;
        }

        if (AudioManager.INSTANCE != null)
            AudioManager.INSTANCE.playWebCollide();

        web.transform.localScale = new Vector3(initScale.x, targetLength, initScale.z);

        if (deleteAfter)
        {
            Debug.Log("Deleting!");
            Destroy(web);
            GameObject bug = GameObject.FindGameObjectWithTag("BugToCocoon");
            Transform cocoonPos = bug.transform;
            bool isFlyMore = bug.GetComponent<DieOnContact>().fly0Ant1 == 0;
            bool isGolden = bug.GetComponent<BugStats>().isGolden;
            Destroy(bug);

            GameObject cocoon = Instantiate(cocoonPrefab);
            cocoon.transform.position = cocoonPos.position;
            cocoon.transform.rotation = cocoonPos.rotation;
            cocoon.GetComponent<DieOnContact>().fly0Ant1 = isFlyMore ? 0 : 1;
            cocoon.GetComponent<BugStats>().isGolden = isGolden;
        }
    }

    public WebNode findClosestNodeTo(Vector3 pos)
    {
        if (WebManager.Instance.nodes.Count == 0)
        {
            return null;
        }
        WebNode closest = WebManager.Instance.nodes[0];
        foreach (WebNode other in WebManager.Instance.nodes)
        {
            if (Vector3.Distance(pos, other.pos) < Vector3.Distance(pos, closest.pos))
            {
                closest = other;
            }
        }
        return closest;
    }
}
