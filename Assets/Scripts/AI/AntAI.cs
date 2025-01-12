using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAI : AI
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
    private Transform player;
    private bool isFleeing = false;

    // Start is called before the first frame update
    void Start()
    {
        antMovement = GetComponent<AntMovement>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // player = FindObjectOfType<PlayerController>();

        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            if (isFleeing)
            {
                yield return new WaitForSeconds(1.0f);
                continue;
            }

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
        // Flee if in flee range
        if (player != null &&
            Vector3.Distance(player.transform.position, transform.position) < fleeRadius)
        {
            isFleeing = true;

            if (Vector3.Dot(antMovement.RightDirection,
                transform.position - player.transform.position) > 0)
            {
                antMovement.MovementState = AntMovement.State.RIGHT;
            }
            else
            {
                antMovement.MovementState = AntMovement.State.LEFT;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }

    public override void Kill()
    {
        antMovement.WebCaught();
        StopAllCoroutines();
        antMovement.MovementState = AntMovement.State.STILL;
    }
}
