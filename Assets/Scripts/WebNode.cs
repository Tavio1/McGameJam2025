using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class WebNode
{
    public Vector3 pos;
    public List<WebNode> adjacent;

    public WebNode(Vector3 p) {
        pos = p;
        adjacent = new List<WebNode>();
    }

    public void addAdjacent(WebNode webNode) {
        adjacent.Add(webNode);
    }

    public void removeAdjacent(WebNode webNode) {
        adjacent.Remove(webNode);
    }
}
