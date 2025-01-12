using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeUpIndicator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeUp;
    [SerializeField] CanvasGroup fadeBlack;
    private CanvasGroup cg;


    [Header("Values for Tav to Mess Around With")]
    public float SlideInTime;
    public float WaitTime;
    public float SlideOutTime;

    void Start(){
        cg = timeUp.GetComponent<CanvasGroup>();
        cg.alpha = 0f;
        timeUp.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500,0);
    }


    public void StartTimeUp(){
        StartCoroutine("sequence");
    }

    IEnumerator sequence(){
        Time.timeScale = 0f;

        LeanTween.alphaCanvas(cg, 1f, SlideInTime).setIgnoreTimeScale(true);
        LeanTween.moveLocalX(timeUp.gameObject, 0f, SlideInTime).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(SlideInTime);

        yield return new WaitForSecondsRealtime(WaitTime);

        LeanTween.alphaCanvas(cg,0f,SlideOutTime).setIgnoreTimeScale(true);
        LeanTween.moveLocalX(timeUp.gameObject, 500f, SlideOutTime).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(SlideOutTime);

        LeanTween.alphaCanvas(fadeBlack,1f,1f).setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene(2);

    }
}
