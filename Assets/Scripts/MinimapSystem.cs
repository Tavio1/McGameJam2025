using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSystem : MonoBehaviour
{
    [SerializeField] RectTransform playerMini;
    [SerializeField] Transform player;

    public const float mapWidth = 46f;
    public const float mapHeight = 20f;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.position;

        float relX = ((pos.x + 23) / mapWidth) * 400;
        float relY = (pos.y/ mapHeight) * 173;

        playerMini.anchoredPosition = new Vector2(relX, relY);
    }
}
