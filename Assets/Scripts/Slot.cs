using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private RectTransform rt;
    private SlotMachine slotMachine;
    private SlotRow slot;

    private bool normal = true;
    float speed;

    Color[] testColors = {
        Color.magenta,
        Color.blue,
        Color.red,
        Color.black
    };


    void Start(){
        slotMachine = transform.root.GetComponentInChildren<SlotMachine>();
        slot = transform.parent.GetComponent<SlotRow>();
        rt = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        if (slotMachine.Running){
            

            float yPos = rt.anchoredPosition.y - Time.deltaTime* slot.speed;
            
            
            // Reset its position and change value
            if (yPos <= -450){
                yPos += 3*225;
                GetComponent<Image>().color = testColors[Random.Range(0,testColors.Length)];

            }

            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yPos);

        }
    }

    
}
