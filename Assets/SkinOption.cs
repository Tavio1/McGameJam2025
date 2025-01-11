using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinOption : MonoBehaviour
{
    public string SkinName;
    private SwitchSkinMenu menu;

    void Start(){
        menu = transform.root.GetComponentInChildren<SwitchSkinMenu>();
    }

    public void UseThisSKin(){
        menu.UseThisSkin(this);
    }
}
