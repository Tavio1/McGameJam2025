using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    private RectTransform rt;
    private SlotMachine slotMachine;
    private SlotRow slot;

    public bool rig = false;
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
                
                Skin[] skins = SkinsManager.instance.availableSkins;
                Skin skin;

                if (rig && slotMachine.chosenOne != null){
                    skin = slotMachine.chosenOne;
                } else {
                    skin = skins[Random.Range(0, skins.Length)];
                }
                

                GetComponentInChildren<TextMeshProUGUI>().text = skin.SkinName;

                yPos += 3*225;
            }

            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, yPos);

        }
    }

    
}
