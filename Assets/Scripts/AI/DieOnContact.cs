using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnContact : MonoBehaviour
{
    [SerializeField]
    private LayerMask contactMask;

    public int fly0Ant1 = 0;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("CONTACT");
        if (collision.gameObject.CompareTag("Player"))
        {
            if (BugCollectManager.instance != null)
                BugCollectManager.instance.CollectBug(fly0Ant1, GetComponent<BugStats>().isGolden);

            Destroy(gameObject);

            AudioManager.INSTANCE.playGulp();
        }
    }
}
