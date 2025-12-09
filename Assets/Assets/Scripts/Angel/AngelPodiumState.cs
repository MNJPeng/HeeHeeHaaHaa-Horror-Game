using UnityEngine;

public class AngelPodiumState : AngelBaseState
{
    public float timeToPoint = 5f;
    public float timer = 0f;
    public float darkTimeLimit = 2f;
    public float darkTimer = 0f;
    public override void EnterState(AngelStateMachine angel) {
        Debug.Log("angel at podium");
        angel.transform.position = angel.podiumPosition;
        angel.LightOn();
        timer = 0f;
        darkTimer = 0f;
    }

    public override void UpdateState(AngelStateMachine angel, float deltaTime) {
        if (LightZoneManager.instance.isSafe)
        {
            timer += deltaTime;
            darkTimer = 0f;
            if (timer >= timeToPoint)
            {
                angel.ChangeState(angel.PointState);
            }
        } else
        {
            darkTimer += deltaTime;
            timer = 0f;
            Debug.Log("dark timer: " + darkTimer);
            if (darkTimer >= darkTimeLimit)
            {
                angel.ChangeState(angel.DarkState);
            }
        }
    }

    public override void ExitState(AngelStateMachine angel) {
        
    }
}
