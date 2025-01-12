using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    private Transform player;
    private CinemachineVirtualCamera cinemachine;

    
    public float StartWaitTime = 2f;

    public float zoomSpeed = 2f; // Speed for zooming
    public float panSpeed = 2f; // Speed for panning

    public float targetFOV = 40f; // Target FOV for zoom
    public float transitionDuration = 1f; // Duration to transition to player follow

    public float sequenceDuration = 3f;
    

    void Start(){
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player").transform;

        StartCoroutine("StartSequence");
    }

    IEnumerator StartSequence(){

        yield return new WaitForSecondsRealtime(StartWaitTime);

        Vector3 Start = transform.position;
        Vector3 End = new Vector3(player.position.x, player.position.y, -25f);

        Vector3 Startrot = Vector3.zero;
        Vector3 EndRot = new Vector3(8f,0,0);
        
        float elapsed = 0f;
        while (elapsed < sequenceDuration){
            float ratio = elapsed/sequenceDuration;

            Vector3 currentPos = Vector3.Slerp(Start,End, ratio);
            transform.position = currentPos;

            Vector3 currentRot = Vector3.Slerp(Startrot, EndRot, ratio);
            transform.eulerAngles = currentRot;
            
            elapsed += Time.deltaTime;

            yield return null;
        }

        // Step 3: Transition to player follow
        // cinemachine.LookAt = player; // Set look at the player
        cinemachine.Follow = player; // Set follow to the player
        
    }
}
