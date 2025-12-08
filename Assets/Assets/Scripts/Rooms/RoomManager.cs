using UnityEngine;

// Attach this to an empty object acting as the "Brain" of the room (e.g., "Kitchen_Manager")
public class RoomManager : MonoBehaviour
{
    public void AdvanceRoomStage()
    {
        // Your existing logic to update paintings/lights
        Debug.Log($"Advancing stage for room: {gameObject.name}");
    }
}