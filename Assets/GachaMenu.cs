using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaMenu : MonoBehaviour
{
    [SerializeField] ClosedMenuManager manager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            manager.OpenGachaMenu(false);
        }
    }
}
