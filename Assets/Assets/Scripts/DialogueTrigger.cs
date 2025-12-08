using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string filename;
    [SerializeField] private bool isOneTime = true;
    
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
        if (coll.gameObject.tag == "Player" && (!isOneTime || isOneTime && !hasTriggered)) {
            Debug.Log("trigger");
        }
    }

    void TriggerDialogue(string filename) {

    }
}
