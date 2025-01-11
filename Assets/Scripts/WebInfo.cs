using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebInfo : MonoBehaviour
{
    public WebNode start;
    public WebNode end;

    public Vector3 startPos;
    public Vector3 endPos;

    void Update() {
        startPos = start.pos;
        endPos = end.pos;
    }

    public WebNode getStart() 
    {
        return start;
    }
    public WebNode getEnd()
    {
        return end;
    }
}
