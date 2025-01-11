using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningUIManager : MonoBehaviour
{
    [SerializeField] GameObject Buttons;

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

    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        Application.Quit();
    }

}
