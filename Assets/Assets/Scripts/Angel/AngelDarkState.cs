using UnityEngine;

public class AngelDarkState : AngelBaseState
{
    public override void EnterState(AngelStateMachine angel) {
        Debug.Log("angel dark state");
        angel.LightOff();
        
    }

    public override void UpdateState(AngelStateMachine angel, float deltaTime) {
        // rng, can either taunt or chase
        angel.ChangeState(angel.ChaseState);
    }

    public override void ExitState(AngelStateMachine angel) {
        
    }
}
