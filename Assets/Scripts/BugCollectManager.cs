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

    public static int score = 0;
    public static int highScore;

    public static int totalScore = 999;

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

        if (minutes <= 0 && seconds <= 0){
            EndGame();
        }
    }

    public void EndGame(){
        this.enabled = false;

        totalScore += score;
        SceneManager.LoadScene(0);        

    }

}
