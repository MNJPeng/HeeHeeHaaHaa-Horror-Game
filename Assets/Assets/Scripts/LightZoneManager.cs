using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class LightZoneManager : MonoBehaviour
{
    public static LightZoneManager instance;
    public Transform lightParent;

    public HashSet<LightSafeZone> lightsEntered = new HashSet<LightSafeZone>();

    public bool isSafe => lightsEntered.Count > 0;

    public float flickerDuration = 6f;
    public LightSafeZone flickeredLight;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {

    }

    public void EnterZone(LightSafeZone zone)
    {
        lightsEntered.Add(zone);
        Debug.Log("Player entered safe zone.");
    }

    public void ExitZone(LightSafeZone zone)
    {
        lightsEntered.Remove(zone);
        Debug.Log("Player left safe zone.");
    }

    public void DimActiveLight()
    {
        if (!isSafe)
        {
            Debug.Log("not in safe zone, but dim light function called");
            return;
        }

        var currLight = lightsEntered.FirstOrDefault();
        
        if (currLight != null)
        {
            flickeredLight = currLight;
            currLight.BeginFlicker();
        }
    }

    public void StopFlicker()
    {
        if (flickeredLight != null)
        {
            flickeredLight.StopFlicker();
        }
    }

    public bool CheckIsFlickering()
    {
        return flickeredLight != null && flickeredLight.isFlickering;
    }
}
