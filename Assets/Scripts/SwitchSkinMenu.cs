using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchSkinMenu : MonoBehaviour
{
    [SerializeField] ClosedMenuManager manager;

    [SerializeField] TextMeshProUGUI selectedSkin;

    private GameObject[] Skins = new GameObject[2];

    [SerializeField] GameObject ButtonPrefab;
    [SerializeField] Transform Content;


    void Start(){
        for (int i = 0; i<Skins.Length; i++){
            // Instantiate(ButtonPrefab, Content);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            manager.OpenSkinMenu(false);
        }
    }


    public void UseThisSkin(SkinOption skin){
        selectedSkin.text = skin.SkinName;
    }
}
