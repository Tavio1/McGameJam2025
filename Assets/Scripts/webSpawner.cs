using System;
using System.Collections;
using System.Collections.Generic;
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

        if (Physics.Raycast(origin, dir.normalized, out hitForward, maxWebLength, raycastMask)) {

            // If you hit a bug
            if (hitForward.collider.gameObject.layer == 11) {
                Debug.Log("Bug Hit!");
                hitForward.collider.gameObject.tag = "BugToCocoon";
                WebNode tempWeb = InstantiateWeb(origin, hitForward.point, null, null, true, false, true);
                return null;
            }

            WebNode mergedStartNode = null;
            if (attachedTo != null)
            {
                origin += dir.normalized * pullValue;
                mergedStartNode = new WebNode(origin);
                ConnectWebs(attachedTo, origin, mergedStartNode);
            }
            else if (Physics.Raycast(origin, -dir.normalized, out hitBackward, 1.0f, raycastMask))
            {
                origin = hitBackward.point;
            }
            else {
                origin -= new Vector3(0, 0.5f, 0);
            }

            endPoint = hitForward.point;
            
            //Debug.Log("hit point at " + hitForward.point);
            if (Vector3.Distance(origin, endPoint) < minWebLength)
            {
                return null;
            }
            WebInfo otherWeb = hitForward.transform.GetComponent<WebInfo>(); ;
            WebNode mergedNode = null;
            if (otherWeb != null)
            {
                endPoint -= dir.normalized * pullValue;
                mergedNode = new WebNode(endPoint);
                ConnectWebs(otherWeb, endPoint, mergedNode);
            }
            return InstantiateWeb(origin, endPoint, mergedStartNode, mergedNode);
        }
        else
        {
            return null;
        }
    }

    public void ConnectWebs(WebInfo other, Vector3 contactPoint, WebNode mergedNode)
    {
        WebNode oldStartNode = other.start;
        oldStartNode.removeAdjacent(other.end);
        oldStartNode.addAdjacent(mergedNode);
        WebNode oldEndNode = other.end;
        oldEndNode.removeAdjacent(other.start);
        oldEndNode.addAdjacent(mergedNode);
        mergedNode.addAdjacent(oldEndNode);
        mergedNode.addAdjacent(oldStartNode);
        InstantiateWeb(other.start.pos, contactPoint, oldStartNode, mergedNode, false, false);
        InstantiateWeb(contactPoint, other.end.pos, mergedNode, oldEndNode, false, false);
        Destroy(other.gameObject);
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
            if (startNode == null)
            {
                webScript.start = new WebNode(start);
            }
            else
            {
                webScript.start = startNode;
            }
            if (endNode == null)
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
}
