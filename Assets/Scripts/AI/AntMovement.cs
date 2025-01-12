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
    private float rotateSpeed = 360.0f;

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

    public State directionState;


    public State MovementState
    {
        get => state;
        set
        {
            state = value;
            if (value != State.STILL && directionState != value)
            {
                directionState = value;
                StartCoroutine(SwitchDirection());
            }
        }
    }

    public Vector3 RightDirection
    {
        get => Quaternion.Euler(0, 0, targetAngle) * Vector3.right;
    }

    private Vector3 LeftDirection
    {
        get => -RightDirection;
    }

    private Vector3 DownDirection
    {
        get => Quaternion.Euler(0, 0, targetAngle) * Vector3.down;
    }

    private Vector3 Forward
    {
        get => state == State.RIGHT ? RightDirection : LeftDirection;
    }

    private float targetAngle = 0.0f;
    private bool isRotating = false;

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

        MoveToHit(minHit, false, true);
    }

    private void SnapDown()
    {
        RaycastHit hit;
        Vector3 down = DownDirection;

        const float THRESHOLD = 1.5f;

        if (Physics.Raycast(transform.position - down * 0.3f, down, out hit,
            float.MaxValue, obstacleLayer) && hit.distance < THRESHOLD)
        {
            MoveToHit(hit, true);
        }
        else if (Physics.Raycast(transform.position + down * 0.3f, -Forward, out hit,
            float.MaxValue, obstacleLayer) && hit.distance < THRESHOLD)
        {
            MoveToHit(hit, true, false, false);
        }
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

    private void MoveToHit(RaycastHit hit, bool force = false, bool snap = false, bool longRotation = false)
    {
        Vector3 hitNormal = hit.normal;
        float angle = Mathf.Acos(Vector3.Dot(Vector3.up, hit.normal) / hitNormal.magnitude) * Mathf.Rad2Deg;
        if (Vector3.Cross(Vector3.up, hitNormal).z < 0)
        {
            angle = -angle;
        }

        if (force || !Mathf.Approximately(angle, targetAngle))
        {
            Vector3 movePos = hit.point;
            movePos.z = 0;

            if (movePos == Vector3.zero)
                return;

            //Debug.Log($"from AI: before is {transform.position} after is {movePos} gameobject is {gameObject}");

            transform.position = movePos;

            const float THRESHOLD = 5;

            targetAngle = angle;

            if (snap || Mathf.Abs(transform.rotation.eulerAngles.z - angle) < THRESHOLD)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);

            }
            else
            {
                StartCoroutine(RotateToWall(angle, longRotation));
            }

            //transform.rotation = Quaternion.Euler(0, 0, angle);

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

        CheckWall();

    }

    private IEnumerator RotateToWall(float angle, bool longRotation = false)
    {
        if (!isRotating)
        {
            isRotating = true;

            float angle2 = -360 + angle;
            
            float angle3 = 360 + angle;

            float currentAngle = transform.eulerAngles.z;

            float diff = Mathf.Abs(angle - currentAngle);
            float diff2 = Mathf.Abs(angle2 - currentAngle);
            float diff3 = Mathf.Abs(angle3 - currentAngle);

            if (!longRotation)
            {
                diff = Mathf.Min(diff, diff2, diff3);

                if (diff == diff2)
                    angle = angle2;

                if (diff == diff3)
                    angle = angle3;
            }
            else
            {
                diff = Mathf.Max(diff, diff2);

                if (diff == diff2)
                    angle = angle2;
            }

            Debug.Log($"original angle {currentAngle} destination angle {angle}");

            const float THRESHOLD = 5;
            const float TIME_INTERVAL = 0.01f;

            while (Mathf.Abs((angle + 360) % 360 - transform.eulerAngles.z) > THRESHOLD)
            {
                //currentAngle += TIME_INTERVAL * rotateSpeed;
                currentAngle = Mathf.Lerp(currentAngle, angle, rotateSpeed / 10000);

                transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                yield return new WaitForSeconds(TIME_INTERVAL);
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);

            Debug.Log("DONE");

            isRotating = false;
        }


        
    }

    private IEnumerator SwitchDirection()
    {
        float destinationAngle = directionState == State.RIGHT ? 0 : 180;
        float currentAngle = directionState == State.RIGHT ? 180 : 0;

        float diff = Mathf.Abs(destinationAngle - currentAngle);

        const float THRESHOLD = 3;
        const float TIME_INTERVAL = 0.01f;

        while (Mathf.Abs(destinationAngle - meshParent.transform.localEulerAngles.y) > THRESHOLD)
        {
            //currentAngle += TIME_INTERVAL * rotateSpeed;
            currentAngle = Mathf.Lerp(currentAngle, destinationAngle, rotateSpeed * rotateSpeed * TIME_INTERVAL / (diff * diff));

            meshParent.localRotation = Quaternion.Euler(0, currentAngle, 0);

            yield return new WaitForSeconds(TIME_INTERVAL);
        }

        meshParent.localRotation = Quaternion.Euler(0, destinationAngle, 0);

    }    

    public void WebCaught()
    {
        rb.drag += 5;
    }
}
