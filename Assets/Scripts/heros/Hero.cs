using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

abstract class Hero : MonoBehaviour
{
	protected int hp = 2000;

    public abstract HeroType GetHeroType();

    public abstract void Selected();

    public abstract void Attack(AttackType attack);

	/// <summary>
	/// Autoattack the enemy playerTarget. This method assumes the target is within range of it's autoattack.
	/// However, it does _not_ assume the character's autoattack if off cooldown.
	/// </summary>
	public abstract void StartAutoAttack (GameObject playerTarget);

	public abstract bool IsAttacking();

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
	public Boolean TakeDamage (int damage) {
		if (isDead()) {
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
