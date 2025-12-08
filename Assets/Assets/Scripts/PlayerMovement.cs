using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintSpeed = 10f; // NEW: The speed when holding shift
    public float mouseSensitivity = 15f;

    [Header("References")]
    public Rigidbody rb;
    public PlayerInput playerInput;
    public Transform cameraTransform;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction; // NEW: Reference to the sprint input
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Setup Actions
        // Make sure the name "Sprint" matches exactly what you typed in the Input Actions window
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"]; // NEW: Get the action
    }

    void Update()
    {
        rb.angularVelocity = Vector3.zero;
        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();

        // 1. Check if we are moving forward
        // In Unity Input System, 'W' usually gives a Y value of 1.
        bool isMovingForward = inputVector.y > 0 && inputVector.x == 0;

        // 2. Determine Speed
        // We only sprint if the button is held AND we are actually walking forward.
        // This prevents sprinting while moving backward (S) or purely sideways (A/D).
        float currentSpeed = (sprintAction.IsPressed() && isMovingForward) ? sprintSpeed : moveSpeed;

        Vector3 moveDirection = transform.right * inputVector.x + transform.forward * inputVector.y;
        
        rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
    }

    void HandleLook()
    {
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();
        
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}