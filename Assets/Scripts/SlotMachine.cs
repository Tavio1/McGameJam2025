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

    public Color[] rolled = new Color[3];


    void Start(){
        PointsText.text = $"Points: {BugCollectManager.totalScore}";
    }

    public void StartSlots(int amount){
        
        if (BugCollectManager.totalScore < 5){
            Debug.Log("you're too poor");
            return;

        } 

        BugCollectManager.totalScore -= 5;

        PointsText.text = $"Points: {BugCollectManager.totalScore}";
        
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
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void SlotResult(){
        Color goal = rolled[0];
        bool result = true;

        for (int i = 1; i<3; i++){
            if (rolled[i] != goal){
                result = false;
                break;
            }
        }

        ResultText.text = result ? $"You won {goal}" : "You didn't win anything!";
    }
}
