using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinOption : MonoBehaviour
{
    public Skin skin;
    public string SkinName;
    private SwitchSkinMenu menu;

    void Start(){
        menu = transform.root.GetComponentInChildren<SwitchSkinMenu>();
    }

    public void UseThisSkin(){
        menu.UseThisSkin(this);
    }
}
