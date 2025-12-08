using UnityEngine;
using System.Collections.Generic;

public enum ObjectiveType {
    Dialogue
}

[System.Serializable]
public class ObjectiveData
{
    public string id;
    public ObjectiveType type;
    public string dialogueId;
}

[CreateAssetMenu(fileName = "MissionSO", menuName = "Scriptable Objects/MissionSO")]
public class MissionSO : ScriptableObject {
    public string id;
    public List<ObjectiveData> objectives = new List<ObjectiveData>();
}
