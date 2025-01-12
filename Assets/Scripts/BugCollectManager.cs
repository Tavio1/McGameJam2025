using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BugCollectManager : MonoBehaviour
{
    public static BugCollectManager instance;

    public TextMeshProUGUI scoreIndicator;
    public TextMeshProUGUI stopwatch;
    public TimeUpIndicator timeUp;

    private bool thirtyFlash = true;
    private bool tenFlash = true;

    public static int score = 0;
    public static int highScore;

    public static int totalScore = 10;

    public float timeLeft = 120f;
    public bool ended;

    void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        ended = false;
    }

    /*
    i = 0: denotes a fly collected
    i = 1: denotes an ant collected
    */
    public void CollectBug(int i, bool isGolden = false){
        int newScore = i == 0 ? 5 : 7;
        if (isGolden)
            newScore *= 3;

        score += newScore;
        scoreIndicator.text = $"Score: {score}";
    }


    // ------------------------ UPDATE ------------------------ 
    void Update(){
        timeLeft -= Time.deltaTime;

        int minutes = (int) timeLeft / 60;
        int seconds = (int) timeLeft % 60;

        if(stopwatch != null) {
            stopwatch.text = string.Format("{0:00}:{1:00}", minutes,seconds);
        }

        if(minutes > 0 && seconds > 0) {
            ended = false;
        }
        
        if (minutes == 0 && seconds == 30 && thirtyFlash){
            thirtyFlash = false;
            StartCoroutine(FlashTimer(1));
        }

        if (minutes == 0 && seconds == 10 && tenFlash){
            tenFlash = false;
            AudioManager.INSTANCE.startClockTicking();
            StartCoroutine(FlashTimer(10));
        }

        if (minutes <= 0 && seconds <= 0 & !ended){
            AudioManager.INSTANCE.stopClockTicking();
            EndGame();
        }
    }

    public void EndGame(){
        totalScore += score;
        ended = true;
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
