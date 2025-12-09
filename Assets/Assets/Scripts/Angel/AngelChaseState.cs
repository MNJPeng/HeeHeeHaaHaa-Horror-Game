using UnityEngine;

public class AngelChaseState : AngelBaseState
{
    public float minDistance = 5f;
    public float maxDistance = 12f;
    public int maxAttempts = 50;
    public float angelRadius = 1f; // radius for overlap check
    public override void EnterState(AngelStateMachine angel) {
        Debug.Log("angel chase state");
        SpawnAngel(angel);
        
    }

    public override void UpdateState(AngelStateMachine angel, float deltaTime) {
        if (angel.IsLooked())
        {
            Debug.Log("is looked");
            return;
        }

        if (LightZoneManager.instance.isSafe)
        {
            angel.ChangeState(angel.PodiumState);
        }

        var player = angel.playerCamera;
        var speed = angel.moveSpeed;

        Vector3 direction = (player.position - angel.transform.position).normalized;

        // Move toward player
        angel.transform.position += direction * speed * deltaTime;

        // Optional: face the player
        angel.transform.forward = direction;
    }

    public override void ExitState(AngelStateMachine angel) {
        
    }

    public void SpawnAngel(AngelStateMachine angel)
    {
        Vector3 spawnPos = Vector3.zero;
        bool validPosition = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            // Pick a random direction around the player
            Vector2 circle = Random.insideUnitCircle.normalized;
            Vector3 dir = new Vector3(circle.x, 0f, circle.y);

            // Pick a random distance
            float distance = Random.Range(minDistance, maxDistance);
            spawnPos = angel.playerCamera.position + dir * distance;

            if (IsValidSpawn(angel, spawnPos))
            {
                validPosition = true;
                break;
            }

            Debug.Log("Spawn pos: " + spawnPos + "failed, try again");
        }

        if (validPosition)
        {
            angel.transform.position = spawnPos;
        }
        else
        {
            Debug.LogWarning("Could not find a valid spawn position for the Angel.");
        }
    }

    private bool IsValidSpawn(AngelStateMachine angel, Vector3 position)
    {
        var player = angel.playerCamera;
        var obstacleMask = angel.obstacleMask; 

        // 1. Check if position is inside an obstacle
        if (Physics.CheckSphere(position, angelRadius, obstacleMask))
            return false;

        // 2. Check line of sight from player
        Vector3 dir = (position - player.position).normalized;
        float distance = Vector3.Distance(player.position, position);

        if (Physics.Raycast(player.position, dir, distance, obstacleMask))
        {
            // wall is between player and spawn point
            return false;
        }

        return true;
    }
}
