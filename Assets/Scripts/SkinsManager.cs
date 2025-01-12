using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager instance;

    public Skin[] availableSkins;
    public HashSet<Skin> ownedSkins = new HashSet<Skin>();

    void Awake(){

        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void CollectSkin(Skin skin){
        ownedSkins.Add(skin);
    }





    
}
