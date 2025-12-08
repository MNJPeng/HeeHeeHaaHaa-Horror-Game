using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private bool isOneTime = true;

    [Header("References")]
    public DialogueManager dialogueManager;
    
    private bool hasTriggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll) {
        if (!coll.CompareTag("Player")) return;
        if (isOneTime && hasTriggered) return;

        dialogueManager.StartDialogue(dialogue);
        hasTriggered = true;
    }
}
