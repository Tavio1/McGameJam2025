using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ButtonManager : MonoBehaviour
{
    public VideoPlayer video;
    public int sceneIndex;
    public Animator start;
    public Animator end;
    public float duration;


    private void Start()
    {
        end.SetTrigger("Start");
        video.loopPointReached += loadGame;
    }
    public void open(){
        StartCoroutine(load(sceneIndex));
    }

    IEnumerator load(int num)
    {
        start.SetTrigger("Start");
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(num);
    }

    void loadGame(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
