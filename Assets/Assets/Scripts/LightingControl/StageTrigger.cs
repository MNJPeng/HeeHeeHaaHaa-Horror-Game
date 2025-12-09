using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    public bool triggerOnlyOnce = true;
    public bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player triggers it
        {
            if (triggerOnlyOnce && hasTriggered) return;

            GameStageManager.Instance.AdvanceStage();
            hasTriggered = true;
            
            // Optional: Destroy this trigger so it can't be hit again
            // Destroy(gameObject); 
        }
    }
}