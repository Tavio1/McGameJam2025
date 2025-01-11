using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class web : MonoBehaviour
{
    public GameObject webPrefab;
    public GameObject player;

    private void Update()
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
        float distance = dir.magnitude;
        Debug.Log("distance is: " + distance);

        GameObject web = Instantiate(webPrefab);
        web.transform.position = pos + dir / 2;
        web.transform.LookAt(worldMousPos);
        web.transform.localScale = new Vector3(web.transform.localScale.x, web.transform.localScale.y, distance);
        Debug.Log("scale is is: " + web.transform.localScale.x + ", " + web.transform.localScale.y + ", " + distance);
    }
}
