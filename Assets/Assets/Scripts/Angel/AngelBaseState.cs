using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AngelBaseState
{
    public abstract void EnterState(AngelStateMachine angel);

    public abstract void UpdateState(AngelStateMachine angel, float deltaTime);

    public abstract void ExitState(AngelStateMachine angel);
}
