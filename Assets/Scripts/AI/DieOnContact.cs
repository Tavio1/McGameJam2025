using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnContact : MonoBehaviour
{
    [SerializeField]
    private LayerMask contactMask;

    [SerializeField]
    private int fly0Ant1 = 0;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CONTACT");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (BugCollectManager.instance != null)
                BugCollectManager.instance.CollectBug(fly0Ant1);

            Destroy(gameObject);
        }
    }
}
