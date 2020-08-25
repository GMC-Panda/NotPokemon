using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float accelerationModifier = 800f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float jumpHeightMultiplier = 2f;

    new Rigidbody rigidbody;

    PlayerControl playerControl;
    bool isGrounded = true;
    BoxCollider boxCollider;
    bool manipulateGravity = false;

    Keyboard kb;
    Gamepad gp;

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        gp = InputSystem.GetDevice<Gamepad>();
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
            Quaternion.identity, 0.5f, layerMask);
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
    private void FixedUpdate()
    {
        GravityManipulation();
    }
    void Update()
    {
        Moving();
        Jump();                 
    }
    public void Moving()
    {
        var movementInput = playerControl.Player.Move.ReadValue<Vector2>();

        var movement = new Vector3(movementInput.x, 0f, movementInput.y);

        transform.Translate(movement * moveSpeed * Time.deltaTime);
        Sprint(movement);        
    }
    public void Jump()
    {
        if (kb.spaceKey.wasPressedThisFrame || gp.buttonSouth.wasPressedThisFrame)             
        {
            if (GroundCheck()) 
            {                
                Vector3 jumpHeightVector = new Vector3(0, jumpHeight, 0);
                 
                rigidbody.AddForce(jumpHeightVector,ForceMode.VelocityChange);                                
            }
         
        }
    }
    public void Sprint(Vector3 movement)
    {

        if ((kb.shiftKey.IsPressed(0) || gp.rightTrigger.IsPressed(0)) && isGrounded && rigidbody.velocity.magnitude < maxSpeed)
        {                    
            rigidbody.AddForce(movement * accelerationModifier,ForceMode.VelocityChange);                    
        }       
    }
    public void GravityManipulation()
    {
        if (rigidbody.velocity.y < 0)            
            rigidbody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rigidbody.velocity.y > 0 && !kb.spaceKey.isPressed)
           rigidbody.velocity += Vector3.up * Physics.gravity.y * (jumpHeightMultiplier - 1) * Time.deltaTime;
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
   
