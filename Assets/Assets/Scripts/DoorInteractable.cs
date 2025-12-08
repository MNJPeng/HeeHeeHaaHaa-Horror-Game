using UnityEngine;

public class DoorInteractable : Interactable
{

    [SerializeField] private string interactTip; // What word shows up on the tooltip when you look at the object
	private Animator animator;
	private bool isOpen;

	void Start() {
		animator = GetComponent<Animator>();
		isOpen = false;
	}

    public override string GetInteractTip() {
        return interactTip;
    }

    public override void Interact() {
        canInteract = false;
		if (isOpen) {
			animator.SetTrigger("Close");
			interactTip = "Open";
			isOpen = false;
		} else {
			animator.SetTrigger("Open");
			interactTip = "Close";
			isOpen = true;
		}
    }

    public void OnAnimEnd() {
        canInteract = true;
    }

    public override bool CheckIsInteractable() {
        return canInteract;
    }
}
