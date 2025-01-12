using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class Glove : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachine;
    private CinemachineTransposer transposer;


    public float zoomSpeed = 5f;

    void Start(){   
        transposer = cinemachine.GetCinemachineComponent<CinemachineTransposer>();
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("hellop");
        
        if (other.tag != "Player") return;

        StartCoroutine(zoomCamera(true));
    }

    void OnTriggerExit(Collider other){
        if (other.tag != "Player") return;

        StartCoroutine(zoomCamera(false));
        
    }

    IEnumerator zoomCamera(bool fadein){

        if (fadein){
            while (transposer.m_FollowOffset.z <= -15){
                Vector3 offset = transposer.m_FollowOffset;
                offset.z += Time.deltaTime*zoomSpeed;
                transposer.m_FollowOffset = offset;
                yield return null;
            }

            Vector3 aoffset = transposer.m_FollowOffset;
            aoffset.z = -15;
            transposer.m_FollowOffset = aoffset;
                
        } else {
            while (transposer.m_FollowOffset.z >= -25){
                Vector3 offset = transposer.m_FollowOffset;
                offset.z -= Time.deltaTime*zoomSpeed;
                transposer.m_FollowOffset = offset;
                yield return null;
            }

            Vector3 aoffset = transposer.m_FollowOffset;
            aoffset.z = -25;
            transposer.m_FollowOffset = aoffset;
        }
    }

}
