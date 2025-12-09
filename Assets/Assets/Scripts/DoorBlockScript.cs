using UnityEngine;
using System.Collections.Generic;

public class DoorBlockScript : MonoBehaviour
{
    public List<DoorInteractable> lockedDoors;
	public bool hasTriggered = false;
	
	void OnTriggerEnter(Collider other) {
		if (hasTriggered) return;
		foreach (DoorInteractable door in lockedDoors) {
			door.OnNextMissionStart();
			hasTriggered = true;
		}
	}
}
