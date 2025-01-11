using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Movement")]
    public float speed;
    public float accel;
    public float moveDir;
    private bool prevOnSlope;
    public float slopeSnapping;

    [Header("Input")]
    public InputActionAsset playerActions;
    private InputAction moveAction;

    [Header("Groundcheck")]
    public float rayLength;
    public LayerMask groundLayerMask;

    [Header("Web")]
    public bool attached;

    //Movement
    public float webSpeed;
    public Vector2 webMoveDir;
    public float radiusToAttach;
    public LayerMask webLayerMask;

    public float movementAngle;

    // Graph Stuff
    private WebNode startNode;
    private WebNode destNode;
    private InputAction attachToWebAction;
    private InputAction moveOnWebAction;

    void Start()
    {
        playerActions.Enable();
        //Movement Actions
        moveAction = playerActions.FindActionMap("Basic").FindAction("Move");
        attachToWebAction = playerActions.FindActionMap("Basic").FindAction("AttachToWeb");
        attachToWebAction.performed += Attach;

        //Web Actions
        moveOnWebAction = playerActions.FindActionMap("Web").FindAction("Move");

        rb = GetComponent<Rigidbody>();
    }

    void Attach(InputAction.CallbackContext ctx)
    {
        if (attached)
        {
            Detach();
        }
        else
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, radiusToAttach, webLayerMask, QueryTriggerInteraction.Collide);
            if (cols.Length > 0)
            {
                if (cols[0].gameObject.layer == 9)
                {
                    InitializeWebWalk(cols[0].GetComponent<WebInfo>());
                }
            }
        }
    }

    void Detach()
    {
        attached = false;
        rb.useGravity = true;

    }

    void InitializeWebWalk(WebInfo web)
    {
        attached = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        startNode = new WebNode(new Vector3(5, 1, 0));
        destNode = new WebNode(new Vector3(1, 5, 0));
        transform.position = startNode.pos + Vector3.Project(transform.position - startNode.pos, destNode.pos - startNode.pos);
    }

    void FixedUpdate()
    {
        if (!attached)
        {
            MovementControl();
        }
        else
        {
            WebControl();
        }
        Debug.DrawLine(new Vector3(1, 5, 0), new Vector3(5, 1, 0));
    }

    void WebControl()
    {
        webMoveDir = moveOnWebAction.ReadValue<Vector2>();
        if (webMoveDir != Vector2.zero)
        {
            if (Vector3.Angle(webMoveDir, destNode.pos - startNode.pos) < movementAngle)
            {
                transform.position = Vector3.MoveTowards(transform.position, destNode.pos, Time.deltaTime * webSpeed);
            }
            else if (Vector3.Angle(webMoveDir, startNode.pos - destNode.pos) < movementAngle)
            {
                transform.position = Vector3.MoveTowards(transform.position, startNode.pos, Time.deltaTime * webSpeed);
            }
        }
    }

    void MovementControl()
    {
        moveDir = moveAction.ReadValue<float>();

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayerMask);

        //if you're in the air
        if (hit.normal == Vector3.zero)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, new Vector3(moveDir * speed, rb.velocity.y, 0), Time.deltaTime * accel);
        }
        //if you're on the ground
        else
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.Cross(hit.normal, Vector3.forward * moveDir).normalized * speed, Time.deltaTime * accel);
        }


        //slope snapping
        if (prevOnSlope != SlopeCheck(hit))
        {
            rb.velocity = new Vector3(rb.velocity.x, -slopeSnapping, rb.velocity.z);
        }

        prevOnSlope = SlopeCheck(hit);
    }

    bool SlopeCheck(RaycastHit hit)
    {
        return !(Vector3.Angle(hit.normal, Vector3.up) < 0.01);
    }
}
