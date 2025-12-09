using UnityEngine;
using UnityEngine.InputSystem; // Required namespace
using System.Collections;

public class FlashlightController : MonoBehaviour
{
    [Header("Input Setup")]
    // Drag your "Flashlight" action from the Input Actions asset into this slot
    public InputActionReference flashlightInput;

    private Light flashlight;

    private float maxStamina = 100f; 
    public float currentStamina = 100f;
    public float drainSpeed = 0.2f;
    public float rechargeSpeed = 2f;

    public bool outOfBattery = false;

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
        if (outOfBattery) return;

        flashlight.enabled = !flashlight.enabled;
    }

    void Update()
    {   
        if (outOfBattery)
        {
            currentStamina = Mathf.Min(currentStamina + rechargeSpeed * Time.deltaTime, maxStamina);

            if (currentStamina == maxStamina)
            {
                outOfBattery = false;
                flashlight.enabled = true;
            }

            return;
        }

        if (flashlight.enabled)
        {
            currentStamina = Mathf.Max(currentStamina - drainSpeed * Time.deltaTime, 0f);
        } else
        {
            currentStamina = Mathf.Min(currentStamina + rechargeSpeed * Time.deltaTime, maxStamina);
        }
        
        if (currentStamina == 0)
        {
            outOfBattery = true;
            StartCoroutine(FlashlightFlicker());
        }
    }

    IEnumerator FlashlightFlicker()
    {
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.15f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.15f);
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.15f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.15f);
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.15f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.15f);
        flashlight.enabled = false;
        yield return new WaitForSeconds(0.2f);
        flashlight.enabled = true;
        yield return new WaitForSeconds(0.2f);
        flashlight.enabled = false;
    }
}