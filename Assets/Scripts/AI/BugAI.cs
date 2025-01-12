using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BugAI : AI
{
    [SerializeField]
    private float nextWaypointDistance = 1.0f;
    [SerializeField]
    private LayerMask obstacleLayer;
    [SerializeField]
    private float fleeRadius = 5.0f;

    [Header("Wander Properties")]
    [SerializeField]
    private float recomputationInterval = 3.0f;
    [SerializeField]
    private float arcAngle = 120;
    [SerializeField]
    private float speed = 100.0f;
    [SerializeField]
    private float wanderRadius = 3.0f;
    [SerializeField]
    private float obstacleCheckRay = 2.0f;


    private Seeker seeker;
    private Rigidbody rb;
    private PlayerController player;

    // Path Movement variables
    private Path path;
    public int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Vector3 Position
    {
        get => new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    private float Angle
    {
        get
        {
            if (rb != null && rb.velocity != Vector3.zero)
            {
                float angle = Mathf.Atan(rb.velocity.y / rb.velocity.x);
                if (rb.velocity.x < 0)
                    return Mathf.PI + angle;
                else
                    return angle;
            }
            else
            {
                return transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            }
        }
    }

    //---------------------------------------------------------------

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();

        StartCoroutine(RecomputationCoroutine());
    }

    private IEnumerator RecomputationCoroutine()
    {
        while (true)
        {
            if (IsInFleeRadius())
            {
                ComputeFleePath();
            }
            else
            {
                ComputePath();
            }

            //ComputePath();

            yield return new WaitForSeconds(recomputationInterval);
        }
    }

    private void ComputePath()
    {
        seeker.StartPath(Position, GenerateWanderPoint(), OnPathComplete);
    }

    private void ComputeFleePath()
    {
        Vector3 fleeDir = (Position - player.transform.position).normalized;
        seeker.StartPath(Position, Position + fleeDir * wanderRadius, OnPathComplete);
    }

    private Vector3 GenerateWanderPoint()
    {
        float angle = Random.Range(Angle - arcAngle * Mathf.Deg2Rad / 2,
            Angle + arcAngle * Mathf.Deg2Rad / 2);

        Vector3 delta = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * wanderRadius;

        if (Physics.Raycast(Position, delta, obstacleCheckRay, obstacleLayer))
        {
            delta = -delta;
        }
        return Position + delta;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.Log("Path computation of " + gameObject.name + " failed.");
            Debug.Log(p.errorLog);
        }
    }

    private bool IsInFleeRadius()
    {
        return Vector3.Distance(Position, player.transform.position) < fleeRadius;
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;

            if (IsInFleeRadius())
            {
                ComputeFleePath();
            }
            else
            {
                ComputePath();
            }

            //ComputePath();
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector3 toTarget = path.vectorPath[currentWaypoint] - Position;
        Vector3 direction = toTarget.normalized;
        Vector3 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = toTarget.magnitude;
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the orientation line
        Gizmos.color = Color.magenta;

        const float LINE_LENGTH = 2.0f;

        Vector3 orientationLine = new Vector3(
            Mathf.Cos(Angle), Mathf.Sin(Angle), 0.0f
            ) * LINE_LENGTH;
        Gizmos.DrawLine(transform.position, transform.position + orientationLine);

        // Flee Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Position, fleeRadius);
    }

    public override void Kill()
    {
        rb.drag += 5;
        StopAllCoroutines();
        path = null;
    }

}
