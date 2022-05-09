using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class UserMovement : MonoBehaviour
{
    [SerializeField]
    private float jumpForce;
    
    //multiply gravity while in mid air
    [SerializeField] 
    private float gravityMultiplier;

    //max speed a player can travel
    [SerializeField] 
    private float maxSpeed;

    //variable to alter the move speed of player.
    [SerializeField] 
    private float movementSpeed;

    //Speed at which player rotates.
    [SerializeField] 
    private float rotationSpeed;

    //in use for checking jump.
    private float distanceToGround = 1.1f;

    //Get the movement Vector2
    private Vector2 inputVector;

    //Components
    [SerializeField]
    private Animator animator;
    private PlayerControls playerControls;
    private InputAction movement;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Transform mainCameraTransform;
    

    //Movement variables
    private Vector3 move;
    private float targetAngle;
    private Quaternion rotation;
    private Vector3 velocity;
    
    
    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        mainCameraTransform = Camera.main.transform;
        velocity = new Vector3(0f, 0f, 0f);
        
    }

    private void OnEnable()
    {
        movement = playerControls.Player.Movement;
        movement.Enable();
        playerControls.Player.Jump.Enable();
        playerControls.Player.Jump.performed += Jump;
    }

   
    
    private void FixedUpdate()
    {
        MovePlayer();
        CheckAndSetGravity();
    }

    /**
     * Mid air gives the player extra downward force to make the player feel heavier and more interesting.
     */
    private void CheckAndSetGravity()
    {
        if (!IsGrounded() && rb.velocity.y <= 5)
        {
            rb.AddRelativeForce(0f,gravityMultiplier ,0f, ForceMode.Acceleration);
        }
    }


    /**
     * The player jumps with an upward force.
     */
    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.AddForce(0f,jumpForce,0f, ForceMode.Impulse);
        }
    }

    /**
     * Moves the player with respect to camera. Makes sure that player loses momentum when buttons are left.
     */
    private void MovePlayer()
    {
        //updates the vector with latest input from player.
        inputVector = movement.ReadValue<Vector2>();

        if (IsGrounded() && rb.velocity.magnitude > maxSpeed)
        {
            move = new Vector3(inputVector.x, 0, inputVector.y);
            move = mainCameraTransform.forward * move.z + mainCameraTransform.right * move.x;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else if (IsGrounded())
        {
            move = new Vector3(inputVector.x, 0, inputVector.y);
            move = mainCameraTransform.forward * move.z + mainCameraTransform.right * move.x;
            move.y = 0f;
            rb.AddForce(move * movementSpeed, ForceMode.VelocityChange);
        }

        if (inputVector != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(inputVector.x, inputVector.y) * Mathf.Rad2Deg +
                          mainCameraTransform.eulerAngles.y;
            rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        //if (inputVector == Vector2.zero && IsGrounded())
        //{
        //    velocity.y = rb.velocity.y;
        //    rb.velocity = velocity;
        //}
    }

    /**
     * Checks if the player is grounded, used to make player move on ground and jump.
     */
    private bool IsGrounded()
    {
        
        return Physics.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, distanceToGround);
    }

    
}