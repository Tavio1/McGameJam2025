using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSkinMenu : MonoBehaviour
{
    [SerializeField] ClosedMenuManager manager;

    [SerializeField] TextMeshProUGUI selectedSkin;

    private Skin[] skins;

    [SerializeField] GameObject ButtonPrefab;
    [SerializeField] Transform Content;


    void Start(){
        skins = SkinsManager.instance.availableSkins;

        foreach (Skin s in skins){

            GameObject button = Instantiate(ButtonPrefab, Content);

            button.GetComponent<SkinOption>().skin = s;
            button.GetComponentInChildren<TextMeshProUGUI>().text = s.SkinName;

            button.GetComponent<Button>().interactable = SkinsManager.instance.ownedSkins.Contains(s);
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
        SkinsManager.instance.selectedSkin = skin.skin;
    }

    public void EvaluateAvailableSkins(){
        Button[] buttons = Content.GetComponentsInChildren<Button>();

        foreach (Button b in buttons){
            b.interactable = SkinsManager.instance.ownedSkins.Contains(b.GetComponent<SkinOption>().skin);
        }
    }
}
