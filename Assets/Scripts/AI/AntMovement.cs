using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    public enum State { STILL, RIGHT, LEFT };

    [SerializeField]
    private State state = State.STILL;

    [SerializeField]
    private float speed = 100.0f;

    [SerializeField]
    private Transform meshParent;

    [SerializeField]
    public LayerMask obstacleLayer;

    [Header("Snapping Properties")]
    [SerializeField]
    private float snapDistance = 1.0f;
    [SerializeField]
    private float snapCooldown = 0.5f;


    private bool canSnap = true;
    private Rigidbody rb;




    public State MovementState
    {
        get => state;
        set => state = value;
    }

    public Vector3 RightDirection
    {
        get => Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * Vector3.right;
    }

    private Vector3 LeftDirection
    {
        get => -RightDirection;
    }

    private Vector3 DownDirection
    {
        get => Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z) * Vector3.down;
    }

    private Vector3 Forward
    {
        get => state == State.RIGHT ? RightDirection : LeftDirection;
    }

    //---------------------------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SnapAllDirections();
    }

    private void SnapAllDirections()
    {
        RaycastHit rightHit;
        RaycastHit leftHit;
        RaycastHit topHit;
        RaycastHit bottomHit;

        bool success = Physics.Raycast(transform.position, Vector3.right, out rightHit, float.MaxValue, obstacleLayer);
        success = Physics.Raycast(transform.position, Vector3.left, out leftHit, float.MaxValue, obstacleLayer) || success;
        success = Physics.Raycast(transform.position, Vector3.up, out topHit, float.MaxValue, obstacleLayer) || success;
        success = Physics.Raycast(transform.position, Vector3.down, out bottomHit, float.MaxValue, obstacleLayer) || success; ;

        if (!success)
            return;

        // Getting closest wall hit
        RaycastHit minHit = rightHit;
        float minDistance = rightHit.distance;

        if (leftHit.distance < minDistance)
        {
            minHit = leftHit;
            minDistance = leftHit.distance;
        }
        if (topHit.distance < minDistance)
        {
            minHit = topHit;
            minDistance = topHit.distance;
        }
        if (bottomHit.distance < minDistance)
        {
            minHit = bottomHit;
        }

        MoveToHit(minHit);
    }

    private void SnapDown()
    {
        RaycastHit hit;
        Vector3 down = DownDirection;
        Physics.Raycast(transform.position - down * 0.1f, down, out hit, float.MaxValue, obstacleLayer);

        MoveToHit(hit);
    }

    private void CheckWall()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position - DownDirection * 0.3f, Forward,
            out hit, snapDistance, obstacleLayer))
        {
            MoveToHit(hit);
            rb.velocity = Forward * rb.velocity.magnitude;
        }    
    }

    private void MoveToHit(RaycastHit hit, bool force = false)
    {
        Vector3 hitNormal = hit.normal;
        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, hit.normal) / hitNormal.magnitude) * Mathf.Rad2Deg;
        if (Vector3.Cross(Vector3.up, hitNormal).z < 0)
        {
            angle = -angle;
        }

        if (force || !Mathf.Approximately(angle, transform.rotation.eulerAngles.z))
        {
            Vector3 movePos = hit.point;
            movePos.z = 0;

            Debug.Log($"from AI: before is {transform.position} after is {movePos} gameobject is {gameObject}");

            transform.position = movePos;

            

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == State.STILL)
        {
            SnapDown();
            return;
        }

        Vector3 moveDirection = state == State.RIGHT ? RightDirection : LeftDirection;

        Vector3 force = Time.deltaTime * moveDirection * speed;
        rb.AddForce(force);

        SnapDown();

        meshParent.localRotation = Quaternion.Euler(0, state == State.RIGHT ? 0 : 180, 0);

        CheckWall();

    }

    public void WebCaught()
    {
        rb.drag += 5;
    }
}
