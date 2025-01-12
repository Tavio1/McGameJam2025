using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using JetBrains.Annotations;

public class SlotRow : MonoBehaviour
{
    public float speed = 500;

    [SerializeField] int index;

    public float deceleration = 20f;
    public SlotMachine machine;

    [SerializeField] RectTransform riggedMiddleOne;

    [SerializeField] float rollTime;

    [SerializeField] RectTransform[] slots = new RectTransform[3]; 

    public void SlowDown(){
        StartCoroutine("slowDown");
    }

    IEnumerator slowDown(){
        Debug.Log(gameObject);


        while (speed > 200f){
            speed -= Time.deltaTime * deceleration;
            yield return null;
        }

        riggedMiddleOne.GetComponent<Slot>().rig = true;
        yield return new WaitForSecondsRealtime(rollTime);


        while (riggedMiddleOne.anchoredPosition.y > 10 || riggedMiddleOne.anchoredPosition.y < -10){
            yield return null;
        }

        speed = 0f;

        riggedMiddleOne.anchoredPosition = new Vector2(riggedMiddleOne.anchoredPosition.x, 0);
        slots[0].anchoredPosition = new Vector2(slots[0].anchoredPosition.x, 225);
        slots[2].anchoredPosition = new Vector2(slots[2].anchoredPosition.x, -225);

        machine.SlotCompleted();
        riggedMiddleOne.GetComponent<Slot>().rig = false;

    }
}
