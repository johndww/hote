using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Attack state data object that contains information about an ongoing, or completed
/// attack.  This is a mutable object.
/// </summary>
public class AttackState : ScriptableObject
{
	public Boolean finished;

	private IEnumerator enumerator;
	private Boolean interruptable;

	public bool isFinished ()
	{
		return this.finished;
	}

	public IEnumerator getEnumerator() {
		return this.enumerator;
	}

	public bool isInterruptable() {
		return this.interruptable;
	}

	private void init (IEnumerator coroutine, bool interruptable)
	{
		this.enumerator = coroutine;
		this.interruptable = interruptable;
	}

	public static AttackState create(IEnumerator coroutine, Boolean interruptable) {
		AttackState attackState = ScriptableObject.CreateInstance<AttackState>();
		attackState.init(coroutine, interruptable);
		return attackState;
	}

	public static AttackState None ()
	{
		var attackState = create(null, true);
		attackState.finished = true;
		return attackState;
	}
}
