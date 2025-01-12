using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddScoreToManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BugCollectManager.score = 0;
        BugCollectManager.instance.scoreIndicator = GetComponent<TextMeshProUGUI>();
    }
}
