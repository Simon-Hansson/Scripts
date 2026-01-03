using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse_PlayerMovement : MonoBehaviour
{
    //Input
    private Default Default;
    private InputAction mousePositionAction;

    // Movement
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
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
        Default.Player.MouseClick.performed += MouseClickPerformed;
        Default.Player.MouseClick.canceled += MouseClickCanceled;
        mousePositionAction = Default.Player.MousePosition;
    }

    private void MouseClickPerformed(InputAction.CallbackContext context)
    {
        isMoving = true;
    }
    private void MouseClickCanceled(InputAction.CallbackContext context)
    {
        isMoving = false;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * groundCheckRayLength, Color.red);
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();
            Ray ray = playerCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                Vector3 direction = (targetPosition - transform.position).normalized;
                input = new Vector2(direction.x, direction.z);
                forceDirection = new(input.x, 0, input.y);
                Debug.Log(input.normalized);
            }
        }
        else
        {
            forceDirection = Vector3.zero;
        }
        rb.AddForce(forceDirection * movementForce, ForceMode.Impulse);

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