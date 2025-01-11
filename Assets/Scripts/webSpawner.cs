using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class webSpawner : MonoBehaviour
{
    public GameObject webPrefab;
    public GameObject player;
    public LayerMask raycastMask;

    void FixedUpdate()
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

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.WorldToScreenPoint(pos).z;
        Vector3 worldMousPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 dir = worldMousPos - pos;
        RaycastHit hit;

        if (Physics.Raycast(pos, dir.normalized, out hit, dir.magnitude, raycastMask))
        {
            Debug.DrawRay(pos, dir.normalized * dir.magnitude, Color.green, 2f);

            worldMousPos = hit.point;
            Debug.Log("hit point at " + worldMousPos);
        }
        else
        {
            Debug.DrawRay(pos, dir.normalized * dir.magnitude, Color.red, 2f);
            Debug.Log("No hit detected.");
        }

        Debug.Log("raycast done");
        dir = worldMousPos - pos;
        float distance = dir.magnitude;
        Debug.Log("Distance is: " + distance);
        Debug.Log("start");

        GameObject web = Instantiate(webPrefab);
        web.transform.position = pos + dir / 2;
        web.transform.LookAt(worldMousPos);
        web.transform.Rotate(90, 0, 0);

        web.transform.localScale = new Vector3(web.transform.localScale.x, distance/2, web.transform.localScale.z);
        Debug.Log("Web scale is: " + web.transform.localScale.x + ", " + distance  + ", " + web.transform.localScale.z);

        web webScript = web.GetComponent<web>();
        if (webScript != null)
        {
            webScript.start = pos;
            webScript.end = worldMousPos;
        }
        else
        {
            Debug.Log("script not found");
        }
    }
}
