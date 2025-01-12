using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddTimerToManager : MonoBehaviour
{
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        BugCollectManager.instance.timeLeft = timer;
        BugCollectManager.instance.stopwatch = GetComponent<TextMeshProUGUI>();
    }
}
