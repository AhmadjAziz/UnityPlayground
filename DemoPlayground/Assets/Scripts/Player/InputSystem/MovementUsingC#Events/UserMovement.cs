using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class UserMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    
    //max speed a player can travel
    [SerializeField] private float maxSpeed;
    
    //variable to alter the move speed of player.
    [SerializeField] private float movementSpeed;

    //in use for checking jump.
    private float distanceToGround = 1.1f;
    
    //Get the movement Vector2
    private Vector2 inputVector;
    
    //Components
    private PlayerControls playerControls;
    private InputAction movement;
    private Rigidbody rb;
    private CapsuleCollider cp;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        cp = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        movement = playerControls.Player.Movement;
        movement.Enable();
        playerControls.Player.Jump.Enable();
        playerControls.Player.Jump.performed += Jump;
    }

    //makes the player jump with a given force.
    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            Debug.Log("Jumped");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
    }

    //Checks if the player is touching the ground.
    private bool IsGrounded()
    {
        return Physics.CapsuleCast(cp.bounds.center, cp.bounds.size, 0, Vector3.down, distanceToGround);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        //updates the vector with latest inpuyt from player.
        inputVector = movement.ReadValue<Vector2>();
        
        if (IsGrounded() && rb.velocity.z < maxSpeed && rb.velocity.x < maxSpeed)
        {
            Debug.Log("Movement");
            rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed, ForceMode.Force);
        }
        else if (IsGrounded() && rb.velocity.z >= maxSpeed && rb.velocity.x < maxSpeed)
        {
            rb.AddForce(new Vector3(inputVector.x, 0, 0) * movementSpeed, ForceMode.Force);
        }
        else if (IsGrounded() && rb.velocity.z < maxSpeed && rb.velocity.x >= maxSpeed)
        {
            rb.AddForce(new Vector3(0, 0, inputVector.y) * movementSpeed, ForceMode.Force);
        }
    }
}
