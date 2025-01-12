using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayBottomTextManager : MonoBehaviour
{

    public static GameplayBottomTextManager INSTANCE;

    public TMPro.TextMeshPro textMeshPro;

    private string eTipText = "Press 'e' to climb on web";
    private bool eTipShown = false;

    private string tooCloseText = "Press 'e' to climb on web";

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        eTipShown = false;
        textMeshPro.text = "";
    }

    public bool tipShown() {
        return eTipShown;
    }

    public void showETip() {
        textMeshPro.text = eTipText;
    }

    public void stopETip() {
        textMeshPro.text = "";
    }

    public void webClimbed() {
        eTipShown = true;
        stopETip();
    }








}
