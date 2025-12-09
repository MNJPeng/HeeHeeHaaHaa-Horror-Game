using UnityEngine;
using System.Collections;

public class CreepyFlicker : MonoBehaviour
{
    public Light lightSource;

    [Header("Base Light Settings")]
    public float baseIntensity = 17f;      // Normal brightness
    public float flickerIntensity = 5f;    // Max deviation

    [Header("Flicker Timing")]
    public float minFlickerDuration = 0.05f;  // Fastest flicker
    public float maxFlickerDuration = 0.3f;   // Slowest flicker

    [Header("Blackout Chance")]
    [Range(0f, 1f)]
    public float blackoutChance = 0.1f;       // 10% chance of short blackout
    public float minBlackout = 0.05f;
    public float maxBlackout = 0.2f;

    private void Start()
    {
        if (lightSource == null) lightSource = GetComponent<Light>();
        StartCoroutine(FlickerRoutine());
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

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Decide if we do a blackout
            if (Random.value < blackoutChance)
            {
                Debug.Log("blackout");
                float blackoutTime = Random.Range(minBlackout, maxBlackout);
                yield return FadeLight(lightSource.intensity, 0f, 0.1f);
                yield return new WaitForSeconds(blackoutTime);
            }

            // Flicker normally
            float targetIntensity = baseIntensity + Random.Range(-flickerIntensity, flickerIntensity);
            float duration = Random.Range(minFlickerDuration, maxFlickerDuration);
            float elapsed = 0f;
            float startIntensity = lightSource.intensity;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                lightSource.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                yield return null;
            }
        }
    }
}