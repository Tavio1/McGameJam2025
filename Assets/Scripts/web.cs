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
/*        RaycastHit hit;

        if (Physics.Raycast(pos, dir.normalized, out hit, dir.magnitude))
        {
            worldMousPos = hit.point;
        }

        dir = worldMousPos - pos;*/
        float distance = dir.magnitude;

        GameObject web = Instantiate(webPrefab);
        web.transform.position = pos + dir / 2;
        web.transform.LookAt(worldMousPos);
        web.transform.Rotate(90, 0, 0);

        web.transform.localScale = new Vector3(web.transform.localScale.x, distance/2, web.transform.localScale.z);    }
}
