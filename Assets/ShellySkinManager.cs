using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellySkinManager : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        Skin skin = SkinsManager.instance.selectedSkin;

        foreach (Renderer r in renderers)
        {
            r.material = skin.SkinMaterial;
        }

    }
}
