using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebManager : MonoBehaviour
{
    public static WebManager Instance { get; private set;}

    public List<WebNode> nodes;

    void Start()
    {  
        if (Instance == null) {
            Instance = this;
        }
        nodes = new List<WebNode>();
    }
}
