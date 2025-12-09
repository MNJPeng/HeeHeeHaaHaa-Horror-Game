using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    // This variable changes automatically as you walk around
    [SerializeField] private RoomManager currentActiveRoom;

    // --- TRACKING LOGIC ---
    public void SetCurrentRoom(RoomManager room)
    {
        currentActiveRoom = room;
        Debug.Log("Entered: " + room.name);
    }

    public void ClearCurrentRoom(RoomManager room)
    {
        // Only clear if the room we are exiting is actually the one we are tracking
        if (currentActiveRoom == room)
        {
            currentActiveRoom = null;
        }
    }

    // --- YOUR GAMEPLAY ACTIONS ---
    
    public void OnPlayerDied()
    {
        Debug.Log("Player Died!");
        
        // Check if we are inside a valid room
        if (currentActiveRoom != null)
        {
            currentActiveRoom.AdvanceRoomStage();
        }
    }
}