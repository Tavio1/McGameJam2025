using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Skin")]
public class Skin : ScriptableObject{
    
    public string SkinName;

    public Material SkinMaterial;
}
