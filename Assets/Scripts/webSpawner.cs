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
            WebInfo otherWeb = null;
            if(hit.transform.gameObject.tag == "Web") {
                otherWeb = hit.transform.parent.GetComponent<WebInfo>();
            }
            if (otherWeb != null)
            {
                GameObject thisWeb = InstantiateWeb(pos, endPoint);
                ConnectWebs(thisWeb.GetComponent<WebInfo>(), otherWeb, hit.point);
            } else {
                GameObject thisWeb = InstantiateWeb(pos, endPoint);
            }
        }
        else
        {
            Debug.Log("No hit detected.");
            endPoint = worldMousePos;
            InstantiateWeb(pos, endPoint);
        }

        // web.transform.position = pos + dir / 2;
        // web.transform.LookAt(endPoint);
        // web.transform.Rotate(90, 0, 0);

        // web.transform.localScale = new Vector3(web.transform.localScale.x, distance/2, web.transform.localScale.z);
        // Debug.Log("Web scale is: " + web.transform.localScale.x + ", " + distance  + ", " + web.transform.localScale.z);

        // WebInfo webScript = web.GetComponent<WebInfo>();
        // if (webScript != null)
        // {
        //     webScript.start = new WebNode(pos);
        //     webScript.end = new WebNode(endPoint);
        //     webScript.start.addAdjacent(webScript.end);
        //     webScript.end.addAdjacent(webScript.start);
        // }
        // else
        // {
        //     Debug.Log("script not found");
        // }
    }

    void ConnectWebs(WebInfo self, WebInfo other, Vector3 contactPoint)
    {
        WebNode mergedNode = new WebNode(contactPoint);
        GameObject fromStart = InstantiateWeb(other.start.pos, contactPoint);
        GameObject toEnd = InstantiateWeb(contactPoint, other.end.pos);
        self.end = mergedNode;
        fromStart.GetComponent<WebInfo>().end = mergedNode;
        toEnd.GetComponent<WebInfo>().end = mergedNode;
        Destroy(other.gameObject);
    }

    GameObject InstantiateWeb(Vector3 start, Vector3 end, WebNode startNode = null, WebNode endNode = null)
    {
        GameObject web = Instantiate(webPrefab);
        web.transform.position = start + ((end - start) / 2);
        web.transform.LookAt(end);
        web.transform.Rotate(90, 0, 0);
        web.transform.localScale = new Vector3(web.transform.localScale.x, (end - start).magnitude / 2, web.transform.localScale.z);
        WebInfo webScript = web.GetComponent<WebInfo>();
        if (webScript != null)
        {
            if(startNode == null) {
                webScript.start = new WebNode(start);
            } else {
                webScript.start = startNode;
            }
            if(endNode == null) {
                webScript.end = new WebNode(endPoint);
            } else {
                webScript.end = endNode;
            }
            webScript.start.addAdjacent(webScript.end);
            webScript.end.addAdjacent(webScript.start);
        }
        else
        {
            Debug.Log("script not found");
        }
        return web;
    }
}
