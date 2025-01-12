using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainTransition : MonoBehaviour
{
    public Animator end;
    void Start()
    {
        end.SetTrigger("Start");
    }
}
