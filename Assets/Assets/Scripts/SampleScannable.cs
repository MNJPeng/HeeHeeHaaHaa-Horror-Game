using UnityEngine;

public class SampleScannable : MonoBehaviour, Scannable
{
	public DialogueManager dialogueManager;
    public Dialogue dialogue;
	
	[SerializeField] private Material unscanned;
	[SerializeField] private Material scanned;
	[SerializeField] private MeshRenderer mat;
	
    public void Scan() {
		dialogueManager.StartDialogue(dialogue);
        DialogueManager.onDialogueEnd += OnDialogueEnd;
		Material[] mats = mat.materials;
		mats[1] = scanned;
		mat.materials = mats;
	}
	
	public void OnDialogueEnd(Dialogue dialogueEnded) {
        if (dialogueEnded == dialogue) {
			Debug.Log("unsubscribe");
            DialogueManager.onDialogueEnd -= OnDialogueEnd;
        }
    }
}
