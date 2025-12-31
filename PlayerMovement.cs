using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    //Input
    private Default Default;
    private InputAction move;

    // Movement
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float groundCheckRayLength = 2f;
    private Vector2 input = Vector2.zero;
    private Vector3 forceDirection = Vector3.zero;
    private bool isMoving = false;

    // Scripts
    [SerializeField] private Camera playerCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Default = new Default();
        Default.Enable();
        Default.Player.Movement.performed += movementPerformed;
        Default.Player.Movement.canceled += movementCanceled;
        move = Default.Player.Movement;
    }

    private void movementPerformed(InputAction.CallbackContext context)
    {
        isMoving = true;
        input = context.ReadValue<Vector2>();
    }
    private void movementCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckRayLength, Color.red);
    }
    private void FixedUpdate()
    {
        if(isMoving)
        {
            forceDirection = new(input.x, 0, input.y);
            Debug.Log(input);
        }
        else
        {
            forceDirection = Vector3.zero;
        }
        rb.AddForce(forceDirection * movementForce, ForceMode.Impulse);

        //rb.linearDamping = 5f;
            
        if (Physics.gravity.y != -9.81f)
        {
            Physics.gravity = new(0, -9.81f, 0);
        }
    }
    public bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _, groundCheckRayLength))
            return true;
        else return false;
    }
}