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
    private InputAction attachToWebAction;
    public bool attached;
    public float radiusToAttach;
    public LayerMask webLayerMask;
    public 


    void Start()
    {
        playerActions.Enable();
        moveAction = playerActions.FindActionMap("Player").FindAction("Move");
        attachToWebAction = playerActions.FindActionMap("Player").FindAction("AttachToWeb");
        attachToWebAction.performed += Attach;
        rb = GetComponent<Rigidbody>();
    }

    void Attach(InputAction.CallbackContext ctx)
    {
        Debug.Log("attach");
        Collider[] cols = Physics.OverlapSphere(transform.position, radiusToAttach, webLayerMask);
        if (cols[0] != null)
        {
        }
    }

    void FixedUpdate()
    {
        if (!attached)
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


            //slop snapping
            if (prevOnSlope != SlopeCheck(hit))
            {
                rb.velocity = new Vector3(rb.velocity.x, -slopeSnapping, rb.velocity.z);
            }

            prevOnSlope = SlopeCheck(hit);
        }
    }

    bool SlopeCheck(RaycastHit hit)
    {
        return !(Vector3.Angle(hit.normal, Vector3.up) < 0.01);
    }
}
