using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BugCollectManager : MonoBehaviour
{
    public static BugCollectManager instance;

    [SerializeField] TextMeshProUGUI scoreIndicator;
    [SerializeField] TextMeshProUGUI stopwatch;
    [SerializeField] TimeUpIndicator timeUp;

    private bool thirtyFlash = true;
    private bool tenFlash = true;

    public static int score = 0;
    public static int highScore;

    public static int totalScore = 0;

    public float timeLeft = 90f;

    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }


    }

    /*
    i = 0: denotes a fly collected
    i = 1: denotes an ant collected
    */
    public void CollectBug(int i){
        score += i == 0 ? 5 : 7;
        scoreIndicator.text = $"Score: {score}";
    }


    // ------------------------ UPDATE ------------------------ 
    void Update(){
        timeLeft -= Time.deltaTime;

        int minutes = (int) timeLeft / 60;
        int seconds = (int) timeLeft % 60;

        stopwatch.text = string.Format("{0:00}:{1:00}", minutes,seconds);


        if (minutes == 0 && seconds == 30 && thirtyFlash){
            thirtyFlash = false;
            StartCoroutine(FlashTimer(1));
        }

        if (minutes == 0 && seconds == 10 && tenFlash){
            tenFlash = false;
            StartCoroutine(FlashTimer(10));
        }

        if (minutes <= 0 && seconds <= 0){
            EndGame();
        }
    }

    public void EndGame(){
        this.enabled = false;
        totalScore += score;

        timeUp.StartTimeUp();
    }

    IEnumerator FlashTimer(int seconds){
        int iterations = (int) (seconds/0.4) + 1;

        for (int i = 0; i<iterations; i++){
            stopwatch.color = Color.red;
            yield return new WaitForSecondsRealtime(0.3f);
            stopwatch.color = Color.white;
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }

}
