using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExteriorFade : MonoBehaviour
{

    [SerializeField] Material fadeMat;
    [SerializeField] float fadeSpeed = 5f;

    
    void OnTriggerEnter(Collider other){

        Debug.Log("entered");
        if (other.tag != "Player") return;
        Debug.Log("and is player");
        FadeMat(false);
    }

    void OnTriggerExit(Collider other){
        if (other.tag != "Player") return;

        FadeMat(true);
    }


    public void FadeMat(bool fadeIn){
        StartCoroutine(FadeSequence(fadeIn));
    }

    IEnumerator FadeSequence(bool fadeIn){
        if (!fadeIn){

            float Alpha = 1f;

            while (Alpha > 0f){
                Alpha -= Time.deltaTime;
                Color col1 = fadeMat.color;
                col1.a = Alpha;
                fadeMat.color = col1;

                // Debug.Log("1 " + material.color.a);

                yield return null;
            }

        } else{

            float Alpha = 0f;

            while (Alpha < 1f){
                Alpha += Time.deltaTime;
                Color col1 = fadeMat.color;
                col1.a = Alpha;
                fadeMat.color = col1;

                // Debug.Log("1 " + material.color.a);

                yield return null;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
