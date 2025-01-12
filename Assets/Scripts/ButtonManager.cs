using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ButtonManager : MonoBehaviour
{
    public VideoPlayer video;
    public int sceneIndex;
    public Animator start;
    public Animator end;


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
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(num);
    }

    void loadGame(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneIndex);
    }

}
