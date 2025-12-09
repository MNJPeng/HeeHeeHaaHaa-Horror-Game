using UnityEngine;

public class RoomZone : MonoBehaviour
{
    public RoomManager manager; // Drag the specific manager (Kitchen, Library) here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the player: "You are now in THIS room"
            PlayerContext player = other.GetComponent<PlayerContext>();
            if (player != null)
            {
                player.SetCurrentRoom(manager);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Only clear it if we are exiting the CURRENT room 
            // (Prevents clearing if we just walked into an overlapping room)
            PlayerContext player = other.GetComponent<PlayerContext>();
            if (player != null)
            {
                player.ClearCurrentRoom(manager);
            }
        }
    }
}