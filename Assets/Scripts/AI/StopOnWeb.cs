using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnWeb : MonoBehaviour
{
    public AI ai;

    void Start()
    {
        ai = GetComponent<AI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Web")
        {
            ai.enabled = false;
        }
    }
}
