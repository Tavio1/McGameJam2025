using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.INSTANCE.playGulp();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.INSTANCE.startWalkOnGround();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            AudioManager.INSTANCE.stopWalkingSound();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            AudioManager.INSTANCE.startWalkOnWeb();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            AudioManager.INSTANCE.stopWalkingSound();
        }

        if (Input.GetMouseButtonDown(0)) {
            AudioManager.INSTANCE.playUIClick();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.INSTANCE.playWebShoot();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.INSTANCE.playWebCollide();
        }
    }
}
