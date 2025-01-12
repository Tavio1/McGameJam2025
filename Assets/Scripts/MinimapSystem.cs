using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapSystem : MonoBehaviour
{
    [SerializeField] RectTransform playerMini;
    [SerializeField] Transform player;

    public const float mapWidth = 80f;
    public const float mapHeight = 80f;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.position;

        float relX = (pos.x / mapWidth) * 390;
        float relY = (pos.y/ mapHeight) * 290;

        playerMini.anchoredPosition = new Vector2(relX, relY);
    }
}
