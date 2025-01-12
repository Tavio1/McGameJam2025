using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Sockets;
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
    public float jumpHeight;
    public bool jumping;

    [Header("Input")]
    public InputActionAsset playerActions;
    private InputAction moveAction;
    private InputAction jumpAction;

    [Header("Groundcheck")]
    public float rayLength;
    public bool grounded;
    public float groundcheckHeight;
    public LayerMask groundLayerMask;

    [Header("Web")]
    public bool attached;
    public WebSpawner spawner;

    //Movement
    public float webSpeed;
    public Vector2 webMoveDir;
    public float radiusToAttach;
    public LayerMask webLayerMask;
    public float movementAngle;
    public float radiusToConnectToNode;

    // Graph Stuff
    private WebInfo attachedWeb;
    private WebNode startNode;
    private WebNode destNode;
    public bool onNode;

    private bool closeToStartNode
    {
        get
        {
            return Vector3.Distance(startNode.pos, transform.position) < radiusToConnectToNode;
        }
    }
    private bool closeToEndNode
    {
        get
        {
            return Vector3.Distance(destNode.pos, transform.position) < radiusToConnectToNode;
        }
    }
    private InputAction attachToWebAction;
    private InputAction moveOnWebAction;
    private InputAction shootWebAction;

    void Start()
    {
        playerActions.Enable();
        //Movement Actions
        moveAction = playerActions.FindActionMap("Basic").FindAction("Move");
        attachToWebAction = playerActions.FindActionMap("Basic").FindAction("AttachToWeb");
        attachToWebAction.performed += Attach;
        shootWebAction = playerActions.FindActionMap("Basic").FindAction("ShootWeb");
        shootWebAction.performed += ShootWeb;
        jumpAction = playerActions.FindActionMap("Basic").FindAction("Jump");
        jumpAction.performed += Jump;

        //Web Actions
        moveOnWebAction = playerActions.FindActionMap("Web").FindAction("Move");

        spawner = GetComponent<WebSpawner>();
        rb = GetComponent<Rigidbody>();
    }

    void ShootWeb(InputAction.CallbackContext ctx)
    {
        if(onNode) {
            TooCloseToNode();
            return;
        }
        Collider[] cols = Physics.OverlapSphere(transform.position, 0.05f, webLayerMask, QueryTriggerInteraction.Collide);
        if(attached && cols.Length > 0 && cols[0].gameObject.tag == "Web") {
            attachedWeb = cols[0].GetComponent<WebInfo>();
        }
        WebNode newStartNode = spawner.SpawnWeb(transform.position, attachedWeb);
        if (newStartNode != null && attached)
        {
            transform.position = newStartNode.pos;
            startNode = newStartNode;
            destNode = null;
        }
    }

    void Jump(InputAction.CallbackContext ctx) {
        if(!grounded) {
            return;
        }
        jumping = true;
        if (attached)
        {
            Detach();
        }
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
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
                if (cols[0].gameObject.tag == "Web")
                {
                    InitializeWebWalk(cols[0].GetComponent<WebInfo>());
                }
            }
            grounded = true;
        }
    }

    void Detach()
    {
        attached = false;
        rb.useGravity = true;
        attachedWeb = null;
        onNode = false;
    }

    void InitializeWebWalk(WebInfo web)
    {
        attached = true;
        attachedWeb = web;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        startNode = web.start;
        destNode = web.end;
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
            grounded = true;
            WebControl();
        }
    }

    void WebControl()
    {
        webMoveDir = moveOnWebAction.ReadValue<Vector2>();
        if (webMoveDir != Vector2.zero)
        {
            if (destNode != null)
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
            if (destNode != null && closeToEndNode && !onNode)
            {
                startNode = destNode;
                destNode = null;
                onNode = true;
                //transform.position = startNode.pos;
                Debug.Log("on end node: " + startNode.pos);
            }
            else if (startNode != null && closeToStartNode && !onNode)
            {
                destNode = null;
                onNode = true;
                //transform.position = startNode.pos;
                Debug.Log("returned to start node: " + startNode.pos);
            }
            else if (!closeToStartNode && destNode != null && !closeToEndNode)
            {
                onNode = false;
            }
            if (onNode)
            {
                float minAngle = 90;
                WebNode minNode = null;
                foreach (WebNode node in startNode.adjacent)
                {
                    float angle = Vector3.Angle(webMoveDir, node.pos - startNode.pos);
                    if (angle < minAngle && angle < movementAngle)
                    {
                        minNode = node;
                        minAngle = angle;
                    }
                }
                destNode = minNode;
                if (destNode != null)
                {
                    Debug.Log("dest node: " + destNode.pos);
                }
            }
        }
        if (Input.GetKey(KeyCode.R))
        {
            string adj = "";
            foreach (WebNode node in startNode.adjacent)
            {
                adj += node.pos + "\n";
            }
            Debug.Log(adj);
        }
    }

    void MovementControl()
    {
        moveDir = moveAction.ReadValue<float>();

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayerMask);

        //if you're in the air or on flat ground
        if (hit.normal == Vector3.zero || Vector3.Angle(hit.normal, Vector3.up) < 0.1f || jumping)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, new Vector3(moveDir * speed, rb.velocity.y, 0), Time.deltaTime * accel);
            Debug.Log(hit.normal);
        }
        //if you're on a slope
        else
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.Cross(hit.normal, Vector3.forward * moveDir).normalized * speed, Time.deltaTime * accel);
        }

        if(Physics.Raycast(transform.position, Vector3.down, groundcheckHeight, groundLayerMask)) {
            grounded = true;
        } else {
            grounded = false;
        }


        //slope snapping
        if (prevOnSlope != SlopeCheck(hit) && !jumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, -slopeSnapping, rb.velocity.z);
        }

        prevOnSlope = SlopeCheck(hit);
    }

    bool SlopeCheck(RaycastHit hit)
    {
        return !(Vector3.Angle(hit.normal, Vector3.up) < 0.01);
    }

    public void TooCloseToNode() {

    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 6) {
            jumping = false;
        }
    }
}
