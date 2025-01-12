using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExteriorFadeTwo : MonoBehaviour
{

    [SerializeField] Material[] fadeMats;
    [SerializeField] float fadeSpeed = 5f;
    // private bool isInside = false;

    
    void OnTriggerEnter(Collider other){

        if (other.tag != "Player") return;
        Debug.Log("entered");
        FadeMat(false);
    }

    void OnTriggerExit(Collider other){
        if (other.tag != "Player") return;
        FadeMat(true);
    }

    // void OnTriggerExit(Collider other){
    //     if (other.tag != "Player") return;
    //     Debug.Log("exited");
    //     FadeMat(true);
    // }


    public void FadeMat(bool fadeIn){

        foreach (Material mat in fadeMats){
            StartCoroutine(FadeSequence(mat, fadeIn));
        }

    }

    IEnumerator FadeSequence(Material mat, bool fadeIn){
        
        if (!fadeIn){
            
            mat.SetFloat("_Surface", 0); // 0 = Opaque
            mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_SURFACE_TYPE_OPAQUE");

            float Alpha = 1f;

            while (Alpha > 0f){
                Alpha -= Time.deltaTime;
                Color col1 = mat.color;
                col1.a = Alpha;
                mat.color = col1;

                // Debug.Log("1 " + material.color.a);

                yield return null;
            }

        } else{

            mat.SetFloat("_Surface", 1); // 1 = Transparent
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.DisableKeyword("_SURFACE_TYPE_OPAQUE");

            float Alpha = 0f;

            while (Alpha < 1f){
                Alpha += Time.deltaTime;
                Color col1 = mat.color;
                col1.a = Alpha;
                mat.color = col1;

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
