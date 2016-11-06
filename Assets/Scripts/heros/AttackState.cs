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
	private CompleteFunction completeFunction;

	public delegate void CompleteFunction();
	private static void NoopCompleteFunction() {
	}

	public bool isFinished ()
	{
		return this.finished;
	}

	public IEnumerator getCoroutine() {
		return this.coroutine;
	}

	public CompleteFunction getCompleteFunction() {
		return this.completeFunction;
	}

	public bool isInterruptable() {
		return this.interruptable;
	}

	private void init (IEnumerator coroutine, bool interruptable, CompleteFunction completeFunction)
	{
		this.coroutine = coroutine;
		this.interruptable = interruptable;
		this.completeFunction = completeFunction;
	}
		
	public static AttackState create(IEnumerator coroutine, bool interruptable) {
		return create(coroutine, interruptable, NoopCompleteFunction);
	}

	/// <summary>
	/// Creates an attack state.  The complete function is provided if the attack state is interrupted. It's
	/// intended to be run on interrupt (to close additional resources / reset state)
	/// </summary>
	public static AttackState create(IEnumerator coroutine, bool interruptable, CompleteFunction completeFunction) {
		AttackState attackState = ScriptableObject.CreateInstance<AttackState>();
		attackState.init(coroutine, interruptable, completeFunction);
		return attackState;
	}

	/// <summary>
	/// Creates an uninterruptable attack state.
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
