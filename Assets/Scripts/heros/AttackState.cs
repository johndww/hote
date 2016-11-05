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

	private IEnumerator coroutine;
	private Boolean interruptable;

	public bool isFinished ()
	{
		return this.finished;
	}

	public IEnumerator getCoroutine() {
		return this.coroutine;
	}

	public bool isInterruptable() {
		return this.interruptable;
	}

	private void init (IEnumerator coroutine, bool interruptable)
	{
		this.coroutine = coroutine;
		this.interruptable = interruptable;
	}
		
	public static AttackState create(IEnumerator coroutine, bool interruptable) {
		AttackState attackState = ScriptableObject.CreateInstance<AttackState>();
		attackState.init(coroutine, interruptable);
		return attackState;
	}

	/// <summary>
	/// Creates an uninterruptable attack state that must be manually finished
	/// </summary>
	public static AttackState create() {
		return create(null, false);
	}

	public static AttackState None ()
	{
		var attackState = create(null, true);
		attackState.finished = true;
		return attackState;
	}
}
