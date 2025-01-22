using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClosingSequence : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI points;
    [SerializeField] TextMeshProUGUI highScore;

    [SerializeField] GameObject overlay;
    [SerializeField] CanvasGroup FadeBlack;

    // Start is called before the first frame update
    IEnumerator Start(){
        CanvasGroup cg = points.GetComponent<CanvasGroup>();
        points.rectTransform.anchoredPosition = new Vector2(0,-130);
        cg.alpha = 0f;

        //Debug.Log("Does exist: " + BugCollectManager.instance != null);
        //Debug.Log("Score: " + BugCollectManager.score);
        //points.text = $"{(BugCollectManager.instance != null ? BugCollectManager.score : 5)} points";
        //points.SetText($"{(BugCollectManager.instance == null ? 5 : BugCollectManager.score)} points");
        points.text = BugCollectManager.score.ToString() + " points";

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
        yield return new WaitForSecondsRealtime(0.4f);

        LeanTween.moveY(overlay, 300, 0.5f).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(0.5f);

        LeanTween.moveY(overlay, 320, 0.15f).setIgnoreTimeScale(true);

    }


    // 1 for menu, 2 for play again
    public void PlayAgainAndMenu(int scene){
        StartCoroutine(SwitchScene(scene));
    }

    IEnumerator SwitchScene(int scene){
        LeanTween.alphaCanvas(FadeBlack, 1f, 0.8f).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(0.8f);

        SceneManager.LoadScene(scene);
    }


}
