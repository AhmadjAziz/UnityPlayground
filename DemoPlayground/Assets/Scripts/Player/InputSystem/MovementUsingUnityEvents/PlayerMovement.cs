using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxSpeed;
    private Rigidbody rb;
    private Vector2 inputVector;
    private PlayerControls playerInputActions;
    private float distToGround = 1f;
    private CapsuleCollider capsuleCollider;
    private float targetAngle;

    // private PlayerInput playerInput;

    private void FixedUpdate()
    {
        MovementPerformed();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //playerInput = GetComponent<PlayerInput>();
        
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerInputActions = new PlayerControls();
        playerInputActions.Player.Enable();
        
    }

    //Moves the player character within a given max speed
    private void MovementPerformed()
    {   
        inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();      
        if (isGrounded() && rb.velocity.z < maxSpeed && rb.velocity.x < maxSpeed) {
            
            rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed, ForceMode.Force);
        }
        else if (isGrounded() && rb.velocity.z >= maxSpeed && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(new Vector3(inputVector.x, 0, 0) * movementSpeed, ForceMode.Force);
        }
        else if (isGrounded() && rb.velocity.z < maxSpeed && rb.velocity.x >= maxSpeed)
        {
            rb.AddForce(new Vector3(0, 0, inputVector.y) * movementSpeed, ForceMode.Force);
        }

    }


    //add an upward force for player to jump with, the force can be controlled from unity inspector.
    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded() && context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
        }
           
             
    }

    //Checks if player is touching the ground
    private bool isGrounded()
    {
        return Physics.CapsuleCast(capsuleCollider.bounds.center,capsuleCollider.bounds.size, 0f, Vector3.down, distToGround);
    }
}
