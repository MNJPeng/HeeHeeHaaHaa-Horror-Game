using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum LightPhase {
    None,
    Single,
    Double
}
public class MenuFlickerLight : MonoBehaviour {
    public float switchDuration = 0.5f;
    public float minFlickerGap = 3f;
    public float maxFlickerGap = 8f;
    public float targetGap = 3f;
    public float currentWait = 0f;

    public bool isFlickering = false;
    public LightPhase nextPhase = LightPhase.None;

    public Light lightSource;
    
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
                
                if (nextPhase == LightPhase.Single) {
                    StartCoroutine(SingleFlicker());
                } else if (nextPhase == LightPhase.Double) {
                    StartCoroutine(DoubleFlicker());
                }
            }
        }
    }

    private IEnumerator FadeLight(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / duration);
            lightSource.intensity = Mathf.SmoothStep(from, to, progress);
            yield return null;
        }
    }

    private IEnumerator LightOff(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator LightOn(float duration)
    {
        lightSource.enabled = true;
        yield return new WaitForSeconds(duration);
    }


    private IEnumerator SingleFlicker()
    {
        isFlickering = true;
        float startIntensity = lightSource.intensity;
        float targetIntensity = 3f;

        // OFF
        yield return FadeLight(startIntensity, targetIntensity, Random.Range(0.03f, 0.06f));
        yield return LightOff(Random.Range(0.1f, 0.3f));

        // ON
        lightSource.enabled = true;
        targetIntensity = Random.Range(12f, 17f);
        yield return FadeLight(0f, startIntensity, Random.Range(0.1f, 0.15f));

        isFlickering = false;
        nextPhase = LightPhase.None;
    }

    private IEnumerator DoubleFlicker()
    {
        isFlickering = true;
        float startIntensity = lightSource.intensity;
        float targetIntensity = 3f;
        // OFF
        yield return FadeLight(startIntensity, targetIntensity, Random.Range(0.03f, 0.06f));
        yield return LightOff(Random.Range(0.1f, 0.3f));

        // ON
        lightSource.enabled = true;
        targetIntensity = Random.Range(12f, 17f);
        yield return FadeLight(0f, startIntensity, Random.Range(0.1f, 0.15f));
        yield return LightOn(Random.Range(0.1f, 0.2f));

        // Flicker 2
        targetIntensity = 3f;
        yield return FadeLight(startIntensity, targetIntensity, Random.Range(0.1f, 0.2f));
        yield return LightOff(Random.Range(0.1f, 0.2f));

        lightSource.enabled = true;
        targetIntensity = Random.Range(12f, 17f);
        yield return FadeLight(0f, startIntensity, Random.Range(0.1f, 0.15f));
        yield return LightOn(Random.Range(0.1f, 0.2f));

        isFlickering = false;
        nextPhase = LightPhase.None;
    }

}