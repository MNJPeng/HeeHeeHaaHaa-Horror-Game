using UnityEngine;
using System.Collections.Generic;
using System;

public class Mission
{
    public string id;
    public MissionSO missionData;
    public List<Objective> objectives = new();
    public bool isCompleted = false;

    public event Action<Mission> onMissionComplete;

    public Mission(MissionSO so)
    {
        missionData = so;
        id = missionData.id;

        foreach (var obj in missionData.objectives) {
            Objective objective;
            if (obj.type == ObjectiveType.Dialogue) {
                objective = new DialogueObjective(obj);
                objectives.Add(objective);
                objective.onObjectiveComplete += onObjectiveComplete;
            }
        }
    }

    public void onObjectiveComplete(Objective currObj) {
        currObj.onObjectiveComplete -= onObjectiveComplete;

        foreach (Objective obj in objectives) {
            if (!obj.isCompleted) {
                return;
            }
        }

        isCompleted = true;
        Debug.Log("mission complete");
        onMissionComplete?.Invoke(this);
    }

    public void Cleanup()
    {
        foreach (var obj in objectives) {
            obj.Cleanup();
            obj.onObjectiveComplete -= onObjectiveComplete;
        }
            
    }
}
