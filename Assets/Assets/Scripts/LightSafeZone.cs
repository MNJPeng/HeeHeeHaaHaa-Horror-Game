using UnityEngine;
using System.Collections;

public class LightSafeZone : MonoBehaviour
{
    public bool isOn = true;
    public bool isFlickering = false;

    public Light lightSource;

    [Header("Timing")]
    public float duration = 8f; // how long until blackout

    [Header("Settings")]
    public float peakIntensity = 17f;

    // Optional: control how chaotic the flicker becomes over time
    public AnimationCurve flickerFrequencyCurve;

    private BoxCollider capsule;
    public AudioSource source;


    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        capsule = GetComponent<BoxCollider>();
        CheckInitialColliders();
    }

    private void CheckInitialColliders()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        // Calculate the world half extents (scaled with transform)
        Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, transform.lossyScale);

        // The center in world space
        Vector3 worldCenter = transform.TransformPoint(box.center);

        // Get all overlapping colliders
        Collider[] hits = Physics.OverlapBox(worldCenter, halfExtents, transform.rotation);

        foreach (var col in hits)
        {
            if (col.CompareTag("Player") && isOn)
            {
                LightZoneManager.instance.EnterZone(this);
            }
        }
    }


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && isOn) {
            LightZoneManager.instance.EnterZone(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LightZoneManager.instance.ExitZone(this);
        }
    }

    public void BeginFlicker()
    {
        Debug.Log("flickering");
        StartCoroutine(FlickerShutdown());
    }

    public void StopFlicker()
    {
        StopAllCoroutines();
        lightSource.intensity = peakIntensity;
        source.Stop();
        isOn = true;
    }

    private IEnumerator FlickerShutdown()
    {
        float timer = 0f;
        lightSource.intensity = peakIntensity;
        source.Play();
        isFlickering = true;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // Probability of flickering increases over time
            float flickerChance = flickerFrequencyCurve.Evaluate(t);

            // Random "roll" to see if we flicker this frame
            if (Random.value < flickerChance * Time.deltaTime * 10f)
            {
                // do a tiny blackout flick
                lightSource.intensity = 0f;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
                lightSource.intensity = peakIntensity;
            }

            yield return null;
        }

        // Final chaotic sequence
        for (int i = 0; i < 5; i++)
        {
            lightSource.intensity = 0;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));

            lightSource.intensity = peakIntensity;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.12f));
        }

        // Final OFF
        lightSource.intensity = 0;
        isFlickering = false;
        LightZoneManager.instance.ExitZone(this);
        
    }
}
