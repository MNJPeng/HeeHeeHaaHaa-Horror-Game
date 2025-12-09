using UnityEngine;

public class SampleScannable : MonoBehaviour, Scannable
{
    public void Scan() {
		Debug.Log("Scanned Object");
	}
}
