using UnityEngine;

public class missionTrigger : MonoBehaviour
{
    public MissionSO missionData;
    public bool hasTriggered = false;

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player" && !hasTriggered) {
            MissionManager.instance.SetCurrentMission(missionData);
            hasTriggered = true;
        }
    }
}
