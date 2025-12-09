using UnityEngine;

public class DialogueObjective : Objective
{
    public string targetDialogueId;

    public DialogueObjective(ObjectiveData data) : base(data) {
        DialogueManager.onDialogueEnd += OnDialogueEnd;
        targetDialogueId = data.dialogueId;
    }

    public void OnDialogueEnd(Dialogue dialogue) {
        if (dialogue.id == targetDialogueId) {
            DialogueManager.onDialogueEnd -= OnDialogueEnd;
            Complete();
        }
    }

    public override void Cleanup()
    {
        DialogueManager.onDialogueEnd -= OnDialogueEnd;
    }
}
