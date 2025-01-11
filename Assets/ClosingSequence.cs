using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ClosingSequence : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI points;
    [SerializeField] TextMeshProUGUI highScore;

    // Start is called before the first frame update
    IEnumerator Start(){
        CanvasGroup cg = points.GetComponent<CanvasGroup>();
        points.rectTransform.anchoredPosition = new Vector2(0,-130);
        cg.alpha = 0f;        

        points.text = $"{(BugCollectManager.instance == null ? 5 : BugCollectManager.score)} points";

        LeanTween.alphaCanvas(cg, 1f, 1f).setIgnoreTimeScale(true);
        LeanTween.moveLocalY(points.gameObject, -75, 1f).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(1f);

        // Optional High Score thing
        if (BugCollectManager.instance != null){

            int score = BugCollectManager.score;
            int high = BugCollectManager.highScore;

            highScore.text = 
                score > high ?  
                    "New High Score!" :
                    $"High Score: {high}";
            
            BugCollectManager.highScore = Math.Max(high, score);
        }

        LeanTween.alphaCanvas(highScore.GetComponent<CanvasGroup>(),1f,0.8f);


    }

    public void OpenSwitchSkin(){

    }
}
