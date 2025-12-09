using UnityEngine;
using System.Collections;

public class AngelPointState : AngelBaseState
{
    public override void EnterState(AngelStateMachine angel) {
        angel.RunCoroutine(Pointing(angel));
    }

    public override void UpdateState(AngelStateMachine angel, float deltaTime) {
        // player has left this safe zone
        if (LightZoneManager.instance.CheckIsFlickering() && !LightZoneManager.instance.isSafe)
        {
            LightZoneManager.instance.StopFlicker();
            angel.ChangeState(angel.PodiumState);
        }
    }

    public override void ExitState(AngelStateMachine angel) {
        angel.StopAllRoutines();
    }

    private IEnumerator Pointing(AngelStateMachine angel)
    {
        angel.LightOff();
        yield return new WaitForSeconds(2f);
        // change poses here, maybe
        angel.LightOn();
        LightZoneManager.instance.DimActiveLight();
        yield return new WaitForSeconds(LightZoneManager.instance.flickerDuration + 3f);
        angel.ChangeState(angel.DarkState);
    }
}
