using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SlotMachine : MonoBehaviour
{
    public bool Running = false;
    public float speed = 500f;

    [SerializeField] SlotRow[] slotRows = new SlotRow[3];

    [SerializeField] TextMeshProUGUI ResultText;
    [SerializeField] TextMeshProUGUI PointsText;

    public int slotsCompleted;

    public Color[] rolled = new Color[3];

    public Skin chosenOne = null;

    void Start(){
        PointsText.text = $"Points: {BugCollectManager.totalScore}";
    }

    public void StartSlots(int amount){
        slotsCompleted = 0;

        if (BugCollectManager.totalScore < 5){
            Debug.Log("you're too poor");
            return;
        } 

        BugCollectManager.totalScore -= 5;
        PointsText.text = $"Points: {BugCollectManager.totalScore}";

        // Decide whether or not something is won
        float gamble = Random.value;
        if (gamble <= 0.99f){
            chosenOne = SkinsManager.instance.availableSkins[Random.Range(0, SkinsManager.instance.availableSkins.Length)];
        } else {
            chosenOne = null;
        }
        
        Running = true;
        ResultText.text = "";
        
        for (int i = 0; i<3; i++){
            slotRows[i].speed = 1000;
        }

        Invoke("StopSlots", 0.5f);
    }

    public void StopSlots(){
        StartCoroutine("stopSlots");
        
    }

    IEnumerator stopSlots(){
        for (int i = 0; i<3; i++){
            slotRows[i].SlowDown();
            yield return new WaitForSecondsRealtime(0.8f);
        }
    }

    public void SlotCompleted(){
        slotsCompleted++;

        if (slotsCompleted == 3){
            ResultText.text = 
                chosenOne == null ? "You didn't win. Try again!" 
                : $"You won the {chosenOne.SkinName} skin!"; 

            if (chosenOne == null)
            {
                AudioManager.INSTANCE.playGatchaFail();
            }
            else {
                AudioManager.INSTANCE.playGatchaSuccess();
            }


            if (chosenOne != null && !SkinsManager.instance.ownedSkins.Contains(chosenOne)){
                SkinsManager.instance.ownedSkins.Add(chosenOne);
            }

        }

        
    }
}
