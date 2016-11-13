using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

abstract class Hero : MonoBehaviour
{
	public int greenCooldownDuration = 10;
	public int blueCooldownDuration = 10;
	public int redCooldownDuration = 10;
	public int purpleCooldownDuration = 10;

	// non-static for possibility of future update that boots max hp temporarily
	private int maxHp = 2000;

	protected int hp = 2000;

	protected bool immobile = false;
	protected bool invulnerable = false;

	// initialized in awake
	protected AttackState attackState;
	protected Animator anim;
	protected CharacterControl.AttackUIOverrideFunction uiOverrideFunction;
	private Cooldowns cooldowns;

	/// <summary>
	/// Unity's awake initializer
	/// Overriders must call base.Awake() when overriding.
	/// </summary>
	protected virtual void Awake() {
		this.attackState = AttackState.None();
		this.anim = GetComponentInChildren<Animator>();

		// we could just reference charactercontrol here. the function might be not necessary
		// but it seems strange to have the char control->hero->char control cyclic references
		this.uiOverrideFunction = GetComponent<CharacterControl>().GetAttackUIOverrideFunction();
		this.cooldowns = GetComponent<Cooldowns>();
	}

    public abstract HeroType GetHeroType();

	/// <summary>
	/// Called when a hero is selected.
	/// Overriders must call base.Selected first()
	/// </summary>
	public virtual void Selected() {
	}

	/// <summary>
	/// Called when a hero is unselected.
	/// Overriders must call base.Unselected() first.
	/// </summary>
	public virtual void UnSelected() {
	}

	public abstract bool BlueAttack();

    public abstract bool GreenAttack();

	public abstract bool PurpleAttack();

	public abstract bool RedAttack(GameObject target);

    /// <summary>
    /// Autoattack the enemy playerTarget. This method assumes the target is within range of it's autoattack.
    /// However, it does _not_ assume the character's autoattack if off cooldown.
    /// </summary>
    public abstract void StartAutoAttack (GameObject playerTarget);

	public abstract bool IsAttacking();

	public void InitializeCooldowns (GameObject uiAbilitiesGameObject)
	{
		this.cooldowns.Initialize(uiAbilitiesGameObject);
	}

	protected void ActivateCooldown(AttackType type) {
		// this is annoying - should just be a static map. unforutnately unity's
		// initalizers are strange so we can statically define this without jumping through hoops
		int duration;
		switch (type) {
		case AttackType.BLUE:
			duration = blueCooldownDuration;
			break;
		case AttackType.GREEN:
			duration = greenCooldownDuration;
			break;
		case AttackType.PURPLE:
			duration = purpleCooldownDuration;
			break;
		case AttackType.RED:
			duration = redCooldownDuration;
			break;
		default:
			throw new ArgumentException ("unknown type: " + type);
		}
		this.cooldowns.Activate(type, duration);
	}

	public bool CooldownInProgress(AttackType type) {
		return this.cooldowns.InProgress(type);
	}

	public bool IsImmobile ()
	{
		return this.immobile;
	}

	public bool IsInvulnerable() {
		return this.invulnerable;
	}

	public abstract bool StopAttack();

	public virtual float AttackRange() {
		return 40f;
	}

	public bool InRange(GameObject target) {
		return Vector3.Distance(target.transform.position, gameObject.transform.position) <= AttackRange();
	}

	public bool OutOfRange(GameObject target) {
		return !InRange(target);
	}

	public bool IsAlive ()
	{
		return this.hp > 0;
	}

	public bool IsDead() {
		return !IsAlive();
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	/// <returns><c>true</c>, if hero took damage and is still alive, <c>false</c> if dead.</returns>
	/// <param name="damage">Damage.</param>
	public bool TakeDamage (int damage) {
		if (isDead() || IsInvulnerable()) {
			return false;
		}

		var newHp = this.hp - damage;
		if (newHp > 0) {
			this.hp = newHp;
			Debug.Log("Damaged taken: " + damage + ". new hp: " + this.hp + " to: " + gameObject);
			return true;
		}
		Die();
		return false;
	}

	/// <summary>
	/// Takes the damage over time.
	/// </summary>
	/// <param name="damagePerTick">Damage per tick.</param>
	/// <param name="numTicks">Number ticks.</param>
	/// <param name="tickRate">Tick rate (seconds per tick)</param>
	public void TakeDamageOverTime (int damagePerTick, int numTicks, float tickRate)
	{
		//TODO should keep track of these dots
		StartCoroutine(DoTakeDamageOverTime(damagePerTick, numTicks, tickRate));
	}

	IEnumerator DoTakeDamageOverTime (int damagePerTick, int numTicks, float tickRate)
	{
		for (int i = 0; i < numTicks; i++) {
			TakeDamage(damagePerTick);
			yield return new WaitForSeconds (tickRate);
		}
	}

	public bool Heal(int amount) {
		if (this.hp == this.maxHp) {
			return false;
		}
		this.hp = Math.Min(this.maxHp, this.hp + amount);
		return true;
	}

	void Die ()
	{
		this.hp = 0;
		Debug.Log("DEAD: " + gameObject);
	}

	public Boolean isDead() {
		return this.hp <= 0;
	}
}

enum HeroType
{
    EarthMage,
    WindMage,
    Gladiator,
    HolyWarrior,
    Jaeger,
    Syphoness,
    Alchemist
}
