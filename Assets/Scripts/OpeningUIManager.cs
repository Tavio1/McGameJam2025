using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningUIManager : MonoBehaviour
{
    [SerializeField] GameObject Buttons;
    public Animator start;
    public Animator end;
    public float duration;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        end.SetTrigger("Start");

        LeanTween.alphaCanvas(Buttons.GetComponent<CanvasGroup>(), 1f, 1.5f)
        .setIgnoreTimeScale(true);
        LeanTween.moveY(Buttons,160f,1.5f)
        .setIgnoreTimeScale(true);
        yield return new WaitForSecondsRealtime(1f);

        Buttons.GetComponent<CanvasGroup>().interactable = true;
    }

    public void open(int num){
        StartCoroutine(load(num));
    }

    IEnumerator load(int num)
    {
        start.SetTrigger("Start");
        yield return new WaitForSeconds(duration);
        SceneManager.LoadSceneAsync(num);
    }

    public void ExitGame(){
        Application.Quit();
    }

}
