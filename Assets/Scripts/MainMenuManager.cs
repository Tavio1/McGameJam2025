using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedMenuManager : MonoBehaviour
{
    [SerializeField] GameObject SkinMenu;
    [SerializeField] GameObject GachaMenu;

    private CanvasGroup skinCG;
    private CanvasGroup gachaCG;

    void Start(){
        SkinMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,1200);
        GachaMenu.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-1200);
        SkinMenu.SetActive(false);
        GachaMenu.SetActive(false);

        skinCG = SkinMenu.GetComponent<CanvasGroup>();
        gachaCG = GachaMenu.GetComponent<CanvasGroup>();

        skinCG.alpha = 0f;
        gachaCG.alpha = 0f;
    }    


    public void OpenSkinMenu(bool open){
        StartCoroutine(OpenSkinMenuProcess(open));
    }

    IEnumerator OpenSkinMenuProcess(bool open){

        if (open) {
            AudioManager.INSTANCE.playUIClick();
            SkinMenu.SetActive(true);
            SkinMenu.GetComponent<SwitchSkinMenu>().EvaluateAvailableSkins();
        }

        LeanTween.moveLocalY(SkinMenu, open ? 0 : 1200f, 0.8f);
        LeanTween.alphaCanvas(skinCG, open ? 1 : 0, 0.8f);
        yield return new WaitForSecondsRealtime(0.8f);

        if (!open) SkinMenu.SetActive(false);

    }

    public void OpenGachaMenu(bool open){
        StartCoroutine(OpenGachaMenuProcess(open));
    }

    IEnumerator OpenGachaMenuProcess(bool open){

        if (open) {
            AudioManager.INSTANCE.playUIClick();
            GachaMenu.SetActive(true);
        }

        LeanTween.moveLocalY(GachaMenu, open ? 0 : -1200f, 0.8f);
        LeanTween.alphaCanvas(gachaCG, open ? 1 : 0, 0.8f);
        yield return new WaitForSecondsRealtime(0.8f);

        if (!open) GachaMenu.SetActive(false);

    }
}

