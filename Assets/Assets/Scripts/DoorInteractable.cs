using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Automatically adds an AudioSource if missing
public class DoorInteractable : Interactable
{

    [SerializeField] private string interactTip; // What word shows up on the tooltip when you look at the object
	[SerializeField] private Mission roomMission;

	[Header("Audio Settings")]
    public AudioClip openSound;  // Drag your "Creak_Open.wav" here
    public AudioClip closeSound; // Drag your "Slam_Close.wav" here

	private Animator animator;
	private AudioSource audioSource;
	private bool isOpen;
	
	private bool isLocked = false;

	void Start() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		isOpen = false;
	}

    public override string GetInteractTip() {
        return interactTip;
    }

    public override void Interact() {
        canInteract = false;
		if (isOpen) {
			if (closeSound != null) audioSource.PlayOneShot(closeSound);
			animator.SetTrigger("Close");
			interactTip = "Open";
			isOpen = false;
		} else {
			if (openSound != null) audioSource.PlayOneShot(openSound);
			animator.SetTrigger("Open");
			interactTip = "Close";
			isOpen = true;
		}
    }

    public void OnAnimEnd() {
		if (isLocked) return;
        canInteract = true;
    }

    public override bool CheckIsInteractable() {
        return canInteract;
    }
	
	public void OnNextMissionStart() {
		isLocked = true;
		canInteract = false;
		animator.SetTrigger("Close");
		if (closeSound != null) audioSource.PlayOneShot(closeSound);
	}
}
