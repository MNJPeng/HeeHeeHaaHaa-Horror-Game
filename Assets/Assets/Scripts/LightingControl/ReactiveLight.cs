using UnityEngine;
using System.Collections.Generic;

public enum LightMode 
{ 
    FullBright, 
    Dim, 
    Flicker, 
    Off 
}

[System.Serializable] // This makes it show up in the Inspector!
public class StageLightSetting 
{
    public string name = "Stage Setting"; // Just for labeling
    public LightMode mode;
    [Range(0, 5)] public float dimIntensity = 0.5f; // Only used if Dim
    [Range(1, 20)] public float flickerSpeed = 10f; // Only used if Flicker
}

public class ReactiveLight : MonoBehaviour
{
    [Header("Global Settings")]
    public float fullIntensity = 1.5f; // How bright is "Full"?
    public float transitionSpeed = 3f; // How fast we fade between modes
    
    [Header("Stage Configuration")]
    // The list where you define what happens in Stage 0, Stage 1, etc.
    public List<StageLightSetting> stageSettings; 

    private Light myLight;
    private StageLightSetting currentSetting;
    private float targetIntensity;
    private float noiseSeed; // Randomizes flicker so lights don't flicker in sync

    void Awake()
    {
        myLight = GetComponent<Light>();
        noiseSeed = Random.Range(0f, 100f); // unique seed for this light
        
        // Initialize with Stage 0 if possible
        UpdateStage(0);
    }

    void OnEnable()
    {
        if (GameStageManager.Instance != null)
            GameStageManager.Instance.OnStageChanged += UpdateStage;
    }

    void OnDisable()
    {
        if (GameStageManager.Instance != null)
            GameStageManager.Instance.OnStageChanged -= UpdateStage;
    }

    void UpdateStage(int stageIndex)
    {
        if (stageIndex < stageSettings.Count)
        {
            currentSetting = stageSettings[stageIndex];
        }
        else
        {
            // Default to OFF if we run out of instructions
            currentSetting = new StageLightSetting { mode = LightMode.Off };
        }
    }

    void Update()
    {
        if (currentSetting == null) return;

        switch (currentSetting.mode)
        {
            case LightMode.FullBright:
                targetIntensity = fullIntensity;
                break;

            case LightMode.Dim:
                targetIntensity = currentSetting.dimIntensity;
                break;

            case LightMode.Off:
                targetIntensity = 0f;
                break;

            case LightMode.Flicker:
                // Spooky Flicker Math:
                // Uses Perlin Noise for smooth random waving + a multiplier for range
                float noise = Mathf.PerlinNoise(Time.time * currentSetting.flickerSpeed, noiseSeed);
                // Map noise (0 to 1) to an intensity range (e.g. 0.1 to Full)
                targetIntensity = Mathf.Lerp(0.1f, fullIntensity, noise);
                break;
        }

        // Apply the result smoothly
        // (If flickering, we snap instantly for jerkiness; otherwise we fade)
        myLight.intensity = targetIntensity;
    }
}