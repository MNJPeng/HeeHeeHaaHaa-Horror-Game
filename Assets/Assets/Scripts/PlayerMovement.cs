using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 15f;

    [Header("References")]
    public Rigidbody rb;
    public PlayerInput playerInput;
    public Transform cameraTransform; // We need to rotate the camera separately

    private InputAction moveAction;
    private InputAction lookAction;
    private float xRotation = 0f; // Stores the current vertical angle

    void Start()
    {
        // 1. Lock the cursor to the center of the screen so it doesn't click off
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. Setup Actions
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"]; // "Look" is the default name for mouse input
    }

    void Update()
    {
        rb.angularVelocity = Vector3.zero; // Fixes issue where collision causes repeated spinning
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        // We use "transform.right" and "transform.forward" so we move 
        // relative to where the player is facing, not global North/South.
        Vector3 moveDirection = transform.right * inputVector.x + transform.forward * inputVector.y;
        
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    void HandleLook()
    {
        // 1. Read Mouse Input
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();
        
        // Adjust for sensitivity and frame rate (deltaTime)
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        // 2. Rotate Player Body (Left/Right)
        // We rotate the parent object (the capsule)
        transform.Rotate(Vector3.up * mouseX);

        // 3. Rotate Camera (Up/Down)
        xRotation -= mouseY;
        // Clamp rotation so you can't look too far up/down (prevent neck breaking)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera transform only
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}