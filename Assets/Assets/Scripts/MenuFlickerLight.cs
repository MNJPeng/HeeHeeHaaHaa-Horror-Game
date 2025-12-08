using UnityEngine;
using System.Collections.Generic;

public enum LightPhase {
    None,
    Single,
    Double
}
public class MenuFlickerLight : MonoBehaviour {
    public float switchDuration = 0.5f;
    public float minFlickerGap = 1f;
    public float maxFlickerGap = 4f;
    public float targetGap = 3f;
    public float currentWait = 0f;

    public bool isFlickering = false;
    public LightPhase nextPhase = LightPhase.None;
    
    void Update() {
        if (nextPhase == LightPhase.None) {
            float rng = Random.value;
            if (rng < 0.5f) {
                nextPhase = LightPhase.Single;
            } else {
                nextPhase = LightPhase.Double;
            }
            targetGap = Random.Range(minFlickerGap, maxFlickerGap);
        }

        if (!isFlickering) {
            currentWait += Time.deltaTime;

            if (currentWait >= targetGap) {
                currentWait = 0f;
                
            }
        }


    }

}