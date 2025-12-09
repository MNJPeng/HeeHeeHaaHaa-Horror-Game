using UnityEngine;

public class DialogueInteractable : Interactable
{
    public DialogueManager dialogueManager;
    public Dialogue dialogue;

    [SerializeField] private string interactTip; // What word shows up on the tooltip when you look at the object

    public override string GetInteractTip() {
        return interactTip;
    }

    public override void Interact() {
        dialogueManager.StartDialogue(dialogue);
        canInteract = false;
        DialogueManager.onDialogueEnd += OnDialogueEnd;
    }

    public void OnDialogueEnd(Dialogue dialogueEnded) {
        if (dialogueEnded == dialogue) {
            Debug.Log("unsubscribe");
            canInteract = true;
            DialogueManager.onDialogueEnd -= OnDialogueEnd;
        }
    }

    public override bool CheckIsInteractable() {
        return canInteract;
    }
}
