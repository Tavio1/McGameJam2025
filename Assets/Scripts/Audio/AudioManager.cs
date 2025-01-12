using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager INSTANCE;



    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource UISource;
    [SerializeField]
    private AudioSource GameSource;
    [SerializeField]
    private AudioSource BackgroundSource;
    [SerializeField]
    private AudioSource WalkingSource;
    [SerializeField]
    private AudioSource ClockSource;


    [Header("Audio Clips")]
    [SerializeField]
    private List<string> UISFXNames;
    [SerializeField]
    private List<AudioClip> UISFXClips;
    [SerializeField]
    private List<string> GameSFXNames;
    [SerializeField]
    private List<AudioClip> GameSFXClips;
    [SerializeField]
    private List<string> BackgroundSFXNames;
    [SerializeField]
    private List<AudioClip> BackgroundSFXClips;

    private Dictionary<string, AudioClip> UISFX = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> GameSFX = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> BackgroundSFX = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;


        // Setup the dictionaries of the SFX Names and Audio Clips
        for(int i = 0; i < UISFXNames.Count; i++ ){
            UISFX.Add(UISFXNames[i], UISFXClips[i]);            
        }


        for (int i = 0; i < GameSFXNames.Count; i++)
        {
            GameSFX.Add(GameSFXNames[i], GameSFXClips[i]);
        }


        for (int i = 0; i < BackgroundSFXNames.Count; i++)
        {
            BackgroundSFX.Add(BackgroundSFXNames[i], BackgroundSFXClips[i]);
        }


        // Start the background music
        //BackgroundSource.mute = true;
        //startMainMenuMusic();
        startClassroomAmbient();
        
    }


    // GAME SFX
    public void playGulp() {
        GameSource.PlayOneShot(GameSFX["Gulp"], 0.2f);
    }

    public void playWebShoot() {
        GameSource.PlayOneShot(GameSFX["WebShoot"], 0.2f);
    }

    public void playWebCollide()
    {
        GameSource.PlayOneShot(GameSFX["WebCollide"], 0.2f);
    }

    public void startWalkOnGround() {
        startSourceWithLoopingClip(WalkingSource, GameSFX["PlayerMoveGround"]);
    }

    public void startWalkOnWeb()
    {
        startSourceWithLoopingClip(WalkingSource, GameSFX["WebClimb"]);
    }

    public void stopWalkingSound() {
        WalkingSource.Stop();
    }

    public void startClassroomAmbient() {
        // source should be at 0.02
        startSourceWithLoopingClip(BackgroundSource, BackgroundSFX["ClassroomAmbient"]);
    }

    public void startClockTicking() {
        ClockSource.clip = GameSFX["ClockTicking"];
        ClockSource.Play();
    }

    public void stopClockTicking() {
        ClockSource.Stop();
    }

    // UI SFX
    public void playUIClick()
    {
        UISource.PlayOneShot(UISFX["UIClick"], 0.2f);
    }

    public void startMainMenuMusic() {
        // source should be at 0.007
        startSourceWithLoopingClip(BackgroundSource, BackgroundSFX["MainMenuMusic"]);
    }

    public void stopBackgroundSound() { 
        BackgroundSource.Stop();
    }



    // Helper functions
    private void startSourceWithLoopingClip(AudioSource pSource, AudioClip pClip) {

        // If the source is already playing the given clip, just skip 
        if (pSource.isPlaying && pSource.clip == pClip)
        {
            return;
        }


        if (pSource.isPlaying)
        {
            pSource.Stop();
        }

        pSource.clip = pClip;
        pSource.Play();
    }

}
