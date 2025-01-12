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

        yield return new WaitForSecondsRealtime(0.5f);


        while (riggedMiddleOne.anchoredPosition.y > 10 || riggedMiddleOne.anchoredPosition.y < -10){
            yield return null;
        }

        speed = 0f;

        Debug.Log($"am i reaching {gameObject}");

        riggedMiddleOne.anchoredPosition = new Vector2(riggedMiddleOne.anchoredPosition.x, 0);
        slots[0].anchoredPosition = new Vector2(slots[0].anchoredPosition.x, 225);
        Debug.Log($"{gameObject} {slots[0].anchoredPosition} {slots[0]}");
        slots[2].anchoredPosition = new Vector2(slots[2].anchoredPosition.x, -225);
        Debug.Log($"{gameObject} {slots[2].anchoredPosition} {slots[2]}");

        
        Debug.Log("Selected " + riggedMiddleOne.GetComponent<Image>().color);

        machine.rolled[index] = riggedMiddleOne.GetComponent<Image>().color;

        if (index == 2){
            machine.SlotResult();
        }
    }
}
