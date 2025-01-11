using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class web : MonoBehaviour, IWeb
{
    public Vector3 start = new Vector3(0,0,0);
    public Vector3 end = new Vector3(0,0,0);
    public Vector3 getStart() 
    {
        return start;
    }
    public Vector3 getEnd()
    {
        return end;
    }
}
