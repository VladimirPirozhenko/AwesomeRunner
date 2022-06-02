using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class State<T> where T : MonoBehaviour
{
	abstract public void Tick();
	public virtual void FixedTick() { }
	public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
}

