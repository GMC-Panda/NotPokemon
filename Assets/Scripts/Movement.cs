using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] LayerMask layerMask;
    new Rigidbody rigidbody;

    PlayerControl playerControl;
    bool isGrounded = true;
    BoxCollider boxCollider;
    bool manipulateGravity = false;
    float gravity = -9.80665f;
    

    private void Awake()
    {
        playerControl = new PlayerControl();
        boxCollider = GetComponent<BoxCollider>();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        EnablePlayerControls();
        GroundCheck();
    }

    private bool GroundCheck()
    {
        RaycastHit hit;

        isGrounded = Physics.BoxCast(transform.position, transform.lossyScale / 2, Vector3.down, out hit,
            Quaternion.identity, 2f, layerMask);
        return isGrounded;
    }

    private void EnablePlayerControls()
    {
        
        isGrounded = Physics.BoxCast(transform.position, transform.lossyScale/2, Vector3.down, Quaternion.identity, 2f, layerMask) ;
        playerControl.Player.Enable();
      
    }

    private void OnDisable()
    {
        playerControl.Player.Disable();
    }

    void Start()
    {

    }
    void Update()
    {
        Moving();
        Jump();
        GravityManipulation();
       
    }
    public void Moving()
    {
        var movementInput = playerControl.Player.Move.ReadValue<Vector2>();


        var movement = new Vector3(movementInput.x, 0f, movementInput.y);

        transform.Translate(movement * moveSpeed * Time.deltaTime);
        Debug.Log("I'm moving");
    }
    public void Jump()
    {
        Keyboard kb = InputSystem.GetDevice<Keyboard>();
        Gamepad gp = InputSystem.GetDevice<Gamepad>();
        if (kb.spaceKey.wasPressedThisFrame || gp.buttonSouth.wasPressedThisFrame)             
        {
            if (GroundCheck()) //am Boden?
            {
                rigidbody.velocity = new Vector3(0, jumpHeight, 0); 
                manipulateGravity = true;
            }
            else manipulateGravity = false;
        }
    }
    public void GravityManipulation()
    {
        if (manipulateGravity)
            Physics.gravity = new Vector3(0, gravity*2, 0);
        else if (!manipulateGravity)
            Physics.gravity = new Vector3(0, gravity, 0);
    }
    //Code implement Sprint, implement dodgeroll


}


/*     Gizmo für Boxcollider checks
 *     void OnDrawGizmos()
    {
        float maxDistance = 10f;
        RaycastHit hit;

        bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2, Vector3.down, out hit,
            Quaternion.identity, 2f, layerMask);
        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, transform.lossyScale);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Vector3.down * maxDistance);
        }
    }*/
   
