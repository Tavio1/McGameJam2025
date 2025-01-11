using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAI : MonoBehaviour
{
    [Header("AI Properties")]
    [SerializeField]
    private float fleeRadius = 5.0f;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float sameDirectionProbability = 0.5f;
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float minRepathInterval = 3.0f;
    [SerializeField]
    [Range(1.0f, 10.0f)]
    private float maxRepathInterval = 6.0f;

    private AntMovement antMovement;

    // Start is called before the first frame update
    void Start()
    {
        antMovement = GetComponent<AntMovement>();

        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            if (antMovement.MovementState == AntMovement.State.STILL)
            {
                if (Random.Range(0, 2) == 0)
                    antMovement.MovementState = AntMovement.State.LEFT;
                else
                    antMovement.MovementState = AntMovement.State.RIGHT;
            }

            float randomVal = Random.Range(0.0f, 1.0f);
            if (randomVal < sameDirectionProbability)
            {
                if (randomVal < sameDirectionProbability / 2)
                {
                    antMovement.MovementState = AntMovement.State.STILL;
                }
                else
                {
                    switch (antMovement.MovementState)
                    {
                        case AntMovement.State.RIGHT:
                            antMovement.MovementState = AntMovement.State.LEFT;
                            break;
                        case AntMovement.State.LEFT:
                            antMovement.MovementState = AntMovement.State.RIGHT;
                            break;
                        default:
                            if (Random.Range(0, 2) == 0)
                                antMovement.MovementState = AntMovement.State.LEFT;
                            else
                                antMovement.MovementState = AntMovement.State.RIGHT;
                            break;

                    }
                }
            }

            float waitTime = Random.Range(minRepathInterval, maxRepathInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Flee from player when they are within a certain range
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }
}
