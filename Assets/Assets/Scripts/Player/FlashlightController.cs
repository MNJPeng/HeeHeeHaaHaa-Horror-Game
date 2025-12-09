using UnityEngine;
using UnityEngine.InputSystem; // Required namespace

public class FlashlightController : MonoBehaviour
{
    [Header("Input Setup")]
    // Drag your "Flashlight" action from the Input Actions asset into this slot
    public InputActionReference flashlightInput;

    private Light flashlight;

    void Awake()
    {
        flashlight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        // Enable the action so the system listens for it
        flashlightInput.action.Enable();
        
        // "Subscribe" to the event: When "performed" happens, run ToggleLight()
        flashlightInput.action.performed += ToggleLight;
    }

    private void OnDisable()
    {
        // Clean up: Stop listening when this object is disabled
        flashlightInput.action.performed -= ToggleLight;
        flashlightInput.action.Disable();
    }

    // The function that runs when F is pressed
    private void ToggleLight(InputAction.CallbackContext context)
    {
        flashlight.enabled = !flashlight.enabled;
    }
}