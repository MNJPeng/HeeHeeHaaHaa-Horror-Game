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
    public CanvasGroup tooltipCanvas;
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
		tooltipCanvas.alpha = 0f;
    }

    void OnDisable() => interactAction.Disable();

    // Update is called once per frame
    void Update()
    {
        currInteractObj = null;
		tooltipCanvas.alpha = 0f;
		
		if (PauseMenu.isPaused) {
			return;
		}
		
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));;

        if (Physics.SphereCast(ray, interactRadius, out RaycastHit hitInfo, interactRange) &&
            hitInfo.collider.TryGetComponent(out Interactable interactObj))
        {
            if (!interactObj.CheckIsInteractable()) return;

            currInteractObj = interactObj;
			tooltipCanvas.alpha = 1f;
            tooltipText.text = interactObj.GetInteractTip();
        }

        if (currInteractObj != null && interactAction.WasPressedThisFrame()) {
            currInteractObj.Interact();
        }
    }
}
