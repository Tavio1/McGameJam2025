using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnWeb : MonoBehaviour
{
    public AI ai;

    [SerializeField]
    private GameObject cocoonPrefab;

    void Start()
    {
        ai = GetComponent<AI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Web")
        {
            ai.Kill();
            ai.enabled = false;

            //Transform aiTransform = ai.transform;
            //Vector3 velocity = ai.gameObject.GetComponent<Rigidbody>().velocity;

            //Destroy(ai.gameObject);

            //GameObject cocoon = Instantiate(cocoonPrefab, aiTransform.position, aiTransform.rotation);
            //cocoon.tag = "";
        }
    }
}
