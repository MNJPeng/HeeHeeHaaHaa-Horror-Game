using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [Header("Audio")]
    public AudioClip walkSound;        
    public AudioClip runSound;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction; // NEW: Reference to the sprint input
	private InputAction analyzeAction;
    private float xRotation = 0f;
	
	private float analyzeTime = 0f;
	private bool canAnalyze = true;
    private AudioSource audioSource;
	[SerializeField] private Image analyzeCircle;
	[SerializeField] private Image validAnalyze;
	[SerializeField] private Image invalidAnalyze;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();

        // Setup Actions
        // Make sure the name "Sprint" matches exactly what you typed in the Input Actions window
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        sprintAction = playerInput.actions["Sprint"]; // NEW: Get the action
		analyzeAction = playerInput.actions["Analyze"];
    }

    void Update()
    {
        rb.angularVelocity = Vector3.zero;
		rb.linearVelocity = Vector3.zero;
		if (analyzeAction.IsPressed()) {
			HandleAnalyze();
		} else {
			analyzeTime = 0f;
			analyzeCircle.fillAmount = 0f;
			canAnalyze = true;
			invalidAnalyze.enabled = false;
			validAnalyze.enabled = false;
			HandleMovement();
			HandleLook();
		}
    }

	void HandleAnalyze()
	{
		analyzeTime += Time.deltaTime;
		analyzeCircle.fillAmount = analyzeTime / 3f;
		if (analyzeTime >= 3f && canAnalyze) {
			analyzeCircle.fillAmount = 0f;
			invalidAnalyze.enabled = true;
			
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));;
			if (Physics.SphereCast(ray, 0.1f, out RaycastHit hitInfo, 2f) &&
				hitInfo.collider.TryGetComponent(out Scannable scannedObj))
			{
				validAnalyze.enabled = true;
				invalidAnalyze.enabled = false;
				scannedObj.Scan();
			}
			canAnalyze = false;
		}
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
        if (!audioSource.isPlaying)
        {
            if (sprintAction.IsPressed() && isMovingForward){
            
                audioSource.PlayOneShot(runSound);
            }
            else if (currentSpeed != 0)
            {
                audioSource.PlayOneShot(walkSound);
            }
        }
        

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