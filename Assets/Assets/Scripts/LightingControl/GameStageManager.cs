using UnityEngine;
using System;

public class GameStageManager : MonoBehaviour
{
    public static GameStageManager Instance; // Singleton for easy access

    public int currentStage = 0;
    
    // The "Broadcast" system. Other scripts will listen to this.
    public event Action<int> OnStageChanged;

    void Awake()
    {
        // specific singleton setup
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AdvanceStage()
    {
        currentStage++;
        Debug.Log("Moved to Stage: " + currentStage);

        // Notify all listeners (lights, sounds, etc.)
        // The "?" checks if anyone is listening before shouting
        OnStageChanged?.Invoke(currentStage);
    }
    
    // Optional: Call this if you want to jump to a specific stage
    public void SetStage(int stageIndex)
    {
        currentStage = stageIndex;
        OnStageChanged?.Invoke(currentStage);
    }
}