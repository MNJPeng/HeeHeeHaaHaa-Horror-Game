using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Header("Input Setup")]
    public InputActionReference zoomInput;

    [Header("Settings")]
    public float zoomedFOV = 30f; // How "zoomed in" you want to be
    public float smoothSpeed = 10f; // Higher number = snappier zoom

    private float defaultFOV;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        // Automatically remember the FOV you set in the Inspector as the "default"
        defaultFOV = cam.fieldOfView;
    }

    private void OnEnable()
    {
        zoomInput.action.Enable();
    }

    private void OnDisable()
    {
        zoomInput.action.Disable();
    }

    void Update()
    {
        // 1. Determine the Target FOV
        // If button is held -> Target is zoomedFOV. If released -> Target is defaultFOV.
        float targetFOV = zoomInput.action.IsPressed() ? zoomedFOV : defaultFOV;

        // 2. Smoothly move Current FOV -> Target FOV
        // Lerp ensures that if you spam the button, it changes direction smoothly
        // from wherever it currently is, rather than snapping.
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}