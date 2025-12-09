using UnityEngine;
using System;

public abstract class Objective
{
    public ObjectiveData data;
    public string id;
    public bool isCompleted = false;
    public event Action<Objective> onObjectiveComplete;

    public Objective(ObjectiveData data) {
        this.data = data;
        this.id = data.id;
    }

    public void Complete() {
        isCompleted = true;
        onObjectiveComplete?.Invoke(this);
    }

    public virtual void Cleanup() {

    }
}
