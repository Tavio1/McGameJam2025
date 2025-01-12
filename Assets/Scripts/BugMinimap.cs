using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BugMinimap : MonoBehaviour
{

    private RectTransform bugMini;
    private Transform bug;

    private bool active = false;

    void Start(){
        bugMini = GetComponent<RectTransform>();
    }

    public void Initialize(Transform bug){
        this.bug = bug;
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(),1f,0.5f);
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;

        CalcPosition();
    }

    private void CalcPosition(){
        Vector3 pos = bug.position;

        float relX = ((pos.x + 23) / MinimapSystem.mapWidth) * 400;
        float relY = (pos.y/ MinimapSystem.mapHeight) * 173;

        bugMini.anchoredPosition = new Vector2(relX, relY);
    }
}
