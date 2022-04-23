using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> : State<T> where T : MonoBehaviour 
{
	public State<T> CurrentState { get; private set; }
	public State<T> PreviousState { get; private set; }
 
    private bool isTransition = false;

	public void SetState(State<T> newState)
	{
		if (CurrentState == newState || isTransition)
			return;

		isTransition = true;

		if (CurrentState != null)
			CurrentState.OnStateExit();

		if (PreviousState != null)
			PreviousState = CurrentState;

		CurrentState = newState;

		if (CurrentState != null)
			CurrentState.OnStateEnter();

		isTransition = false;
	}

	public void RevertState()
	{
		if (PreviousState != null)
			SetState(PreviousState);
	}

    public override void Tick()
    {
		CurrentState.Tick();
	}

    public override void OnStateEnter()
    {
		CurrentState.OnStateEnter();
	}

    public override void OnStateExit()
    {
		CurrentState.OnStateExit();
	}
}
