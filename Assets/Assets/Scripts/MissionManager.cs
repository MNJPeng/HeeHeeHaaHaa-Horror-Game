using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public Mission currentMission;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void SetCurrentMission(MissionSO data) {
        if (currentMission != null) {
            if (currentMission.id == data.id) return;

            currentMission.Cleanup();
        }

        currentMission = new Mission(data);
        Debug.Log("mission set");
    }
}
