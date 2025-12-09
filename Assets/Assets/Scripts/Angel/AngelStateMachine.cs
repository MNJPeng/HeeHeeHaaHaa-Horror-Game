using UnityEngine;
using System.Collections;

public class AngelStateMachine : MonoBehaviour
{
    AngelBaseState currentState;
    public AngelPodiumState PodiumState = new AngelPodiumState();
    public AngelPointState PointState = new AngelPointState();
    public AngelDarkState DarkState = new AngelDarkState();
    public AngelChaseState ChaseState = new AngelChaseState();

    public Light podiumLight;
    public float lightIntensity;
    public Transform playerCamera; // Usually the camera attached to the player
    public LayerMask obstacleMask;
    public float moveSpeed;
    public float maxLookAngle = 30f; // degrees
    public Vector3 podiumPosition; 
    void Start()
    {
        lightIntensity = podiumLight.intensity;
        podiumPosition = transform.position;

        currentState = PodiumState; 
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this, Time.deltaTime);
    }

    public void ChangeState(AngelBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    public AngelBaseState checkCurrentState()
    {
        return currentState;
    }

    public void RunCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void StopAllRoutines()
    {
        StopAllCoroutines();
    }

    public void LightOn()
    {
        //play sfx here
        podiumLight.intensity = lightIntensity;
    }

    public void LightOff()
    {
        //play sfx here
        podiumLight.intensity = 0f;
    }

    public bool IsLooked()
    {
        Vector3 dirToAngel = (transform.position - playerCamera.position).normalized;

        // 1. Angle check
        float dot = Vector3.Dot(playerCamera.forward, dirToAngel);
        float cosThreshold = Mathf.Cos(maxLookAngle * Mathf.Deg2Rad);
        if (dot < cosThreshold)
            return false; // Angel is outside player's view cone

        // 2. Line-of-sight check
        float distance = Vector3.Distance(playerCamera.position, transform.position);
        if (Physics.Raycast(playerCamera.position, dirToAngel, out RaycastHit hit, distance, obstacleMask))
        {
            // Something is blocking the view
            return false;
        }

        return true; // Player is looking at the angel
    }
}
