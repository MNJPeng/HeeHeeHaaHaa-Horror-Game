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

    private CapsuleCollider capsule;
    public AudioSource source;


    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }

        capsule = GetComponent<CapsuleCollider>();
        CheckInitialColliders();
    }

    private void CheckInitialColliders()
    {
        Vector3 point1, point2;
        float radius = capsule.radius;

        // Determine capsule ends based on orientation
        Vector3 center = capsule.bounds.center;
        float height = Mathf.Max(capsule.height / 2f - radius, 0f);

        switch (capsule.direction)
        {
            case 0: // X axis
                point1 = center + Vector3.right * height;
                point2 = center - Vector3.right * height;
                break;
            case 1: // Y axis
                point1 = center + Vector3.up * height;
                point2 = center - Vector3.up * height;
                break;
            case 2: // Z axis
            default:
                point1 = center + Vector3.forward * height;
                point2 = center - Vector3.forward * height;
                break;
        }

        // Check for colliders inside capsule
        Collider[] collidersInside = Physics.OverlapCapsule(point1, point2, radius);

        foreach (var col in collidersInside)
        {
            if (col.gameObject.CompareTag("Player") && isOn)
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
