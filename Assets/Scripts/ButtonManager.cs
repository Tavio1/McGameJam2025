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

    private void Start()
    {
        video.loopPointReached += loadGame;
    }
    public void open(){
        SceneManager.LoadScene(sceneIndex);
    }

    void loadGame(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneIndex);
    }

}
