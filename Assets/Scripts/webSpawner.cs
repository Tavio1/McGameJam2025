using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class webSpawner : MonoBehaviour
{
    public GameObject webPrefab;
    public GameObject player;
    public LayerMask raycastMask;
    public float minWebLength;
    private Vector3 endPoint;

    public Vector3 mousePos;
    public Vector3 worldMousePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnWeb();
        }
    }
    // Update is called once per frame
    void SpawnWeb()
    {
        Vector3 pos = player.transform.position;

        mousePos = Input.mousePosition;

        mousePos.z = Camera.main.WorldToScreenPoint(pos).z;
        worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        Vector3 dir = worldMousePos - pos;
        RaycastHit hit;

        if (Physics.Raycast(pos, dir.normalized, out hit, dir.magnitude, raycastMask))
        {
            endPoint = hit.point;
            Debug.Log("hit point at " + hit.point);
            if (Vector3.Distance(pos, endPoint) < minWebLength)
            {
                return;
            }
            WebInfo otherWeb = null;
            if (hit.transform.gameObject.tag == "Web")
            {
                otherWeb = hit.transform.parent.GetComponent<WebInfo>();
            }
            WebNode mergedNode = null;
            if (otherWeb != null)
            {
                mergedNode = new WebNode(endPoint);
                ConnectWebs(otherWeb, endPoint, mergedNode);
            }
            InstantiateWeb(pos, endPoint, null, mergedNode);
        }
        else
        {
            Debug.Log("No hit detected.");
            endPoint = worldMousePos;
            if (Vector3.Distance(pos, endPoint) < minWebLength)
            {
                return;
            }
            InstantiateWeb(pos, endPoint);
        }
    }

    void ConnectWebs(WebInfo other, Vector3 contactPoint, WebNode mergedNode)
    {
        InstantiateWeb(other.start.pos, contactPoint, null, mergedNode);
        InstantiateWeb(contactPoint, other.end.pos, mergedNode);
        Destroy(other.gameObject);
    }

    WebInfo InstantiateWeb(Vector3 start, Vector3 end, WebNode startNode = null, WebNode endNode = null)
    {
        GameObject web = Instantiate(webPrefab);
        web.transform.position = start + ((end - start) / 2);
        web.transform.LookAt(end);
        web.transform.Rotate(90, 0, 0);
        web.transform.localScale = new Vector3(web.transform.localScale.x, (end - start).magnitude / 2, web.transform.localScale.z);
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
            webScript.start.addAdjacent(webScript.end);
            webScript.end.addAdjacent(webScript.start);
        }
        else
        {
            Debug.Log("script not found");
        }
        return web.GetComponent<WebInfo>();
    }
}
