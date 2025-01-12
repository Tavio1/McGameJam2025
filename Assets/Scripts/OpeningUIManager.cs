using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningUIManager : MonoBehaviour
{
    [SerializeField] GameObject Buttons;
    public Animator transition;

    // Start is called before the first frame update
    IEnumerator Start()
    {
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
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(num);
    }

    public void ExitGame(){
        Application.Quit();
    }

}
