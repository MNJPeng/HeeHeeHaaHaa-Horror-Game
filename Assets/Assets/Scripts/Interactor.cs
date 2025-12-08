using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange;
    public float interactRadius;

    [Header("References")]
    public PlayerInput playerInput;
    public GameObject tooltipCanvas;
    public TextMeshProUGUI tooltipText;

    private Transform interactorSource;
    private Interactable currInteractObj;
    private InputAction interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactorSource = transform;

        interactAction = playerInput.actions["Interact"];
        interactAction.Enable();
        tooltipCanvas.SetActive(false);
    }

    void OnDisable() => interactAction.Disable();

    // Update is called once per frame
    void Update()
    {
        currInteractObj = null;
        tooltipCanvas.SetActive(false);

        Ray ray = new Ray(interactorSource.position, interactorSource.forward);

        if (Physics.SphereCast(ray, interactRadius, out RaycastHit hitInfo, interactRange) &&
            hitInfo.collider.TryGetComponent(out Interactable interactObj))
        {
            if (!interactObj.CheckIsInteractable()) return;

            currInteractObj = interactObj;
            tooltipCanvas.SetActive(true);
            tooltipText.text = interactObj.GetInteractTip();
        }

        if (currInteractObj != null && interactAction.WasPressedThisFrame()) {
            currInteractObj.Interact();
        }
    }
}
