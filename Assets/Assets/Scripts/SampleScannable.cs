using UnityEngine;

public class SampleScannable : MonoBehaviour, Scannable
{
	public DialogueManager dialogueManager;
    public Dialogue dialogue;
	
    public void Scan() {
		dialogueManager.StartDialogue(dialogue);
        DialogueManager.onDialogueEnd += OnDialogueEnd;
	}
	
	public void OnDialogueEnd(Dialogue dialogueEnded) {
        if (dialogueEnded == dialogue) {
			Debug.Log("unsubscribe");
            DialogueManager.onDialogueEnd -= OnDialogueEnd;
        }
    }
}
