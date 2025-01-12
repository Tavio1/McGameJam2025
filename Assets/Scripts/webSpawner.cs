using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WebSpawner : MonoBehaviour
{
    public GameObject webPrefab;
    public LayerMask raycastMask;
    public float minWebLength;
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
        RaycastHit hit;

        WebNode mergedStartNode = null;
        if(attachedTo != null) {
            origin += dir.normalized * pullValue;
            mergedStartNode = new WebNode(origin);
            ConnectWebs(attachedTo, origin, mergedStartNode);
        }

        if (Physics.Raycast(origin, dir.normalized, out hit, dir.magnitude, raycastMask))
        {
            endPoint = hit.point;
            Debug.Log("hit point at " + hit.point);
            if (Vector3.Distance(origin, endPoint) < minWebLength)
            {
                return null;
            }
            WebInfo otherWeb = hit.transform.GetComponent<WebInfo>();;
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
            Debug.Log("No hit detected.");
            endPoint = worldMousePos;
            if (Vector3.Distance(origin, endPoint) < minWebLength)
            {
                return null;
            }
            return InstantiateWeb(origin, endPoint, mergedStartNode);
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

    WebNode InstantiateWeb(Vector3 start, Vector3 end, WebNode startNode = null, WebNode endNode = null, bool runAnimations = true, bool setAdjacencies = true)
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
            if(setAdjacencies) {
                webScript.start.addAdjacent(webScript.end);
                webScript.end.addAdjacent(webScript.start);
            }
        }
        else
        {
            Debug.Log("script not found");
        }
        StartCoroutine(animate(web, start, end));
        return webScript.start;
    }

    private IEnumerator animate(GameObject web, Vector3 start, Vector3 end)
    {
        float targetLength = Vector3.Distance(start, end) / 2;
        float currLength = 0;
        Vector3 initScale = web.transform.localScale;

        while(currLength < targetLength)
        {
            currLength += animationSpeed * Time.deltaTime;
            float actualLength = Mathf.Min(currLength, targetLength);
            web.transform.localScale = new Vector3(initScale.x, actualLength, initScale.z);

            Vector3 midpoint = start + (end - start).normalized * actualLength * 2 / 2;
            web.transform.position = midpoint; 
            yield return null;
        }

        web.transform.localScale = new Vector3(initScale.x, targetLength, initScale.z);
    }
}
