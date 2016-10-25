using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

class EarthMage : Hero
{
	public GameObject autoAttackPrefab;

	// initialized in 
	private AttackState attackState;

	private enum AutoAttackSeq { ONE=30, TWO=50, THREE=70 }

	void Awake() {
		this.attackState = AttackState.None();
	}

    public override HeroType GetHeroType()
    {
        return HeroType.EarthMage;
    }

    public override void Selected()
    {
//        Debug.Log("earth mage hero selected");
    }

    public override void Attack(AttackType type)
    {
        Debug.Log("earth mage attacking with: " + type);
    }

	public override void AutoAttack (GameObject target) {
		if (this.attackState.isFinished()) {
			var coroutine = DoAutoAttack(target);
			this.attackState = AttackState.create(coroutine, true);
			StartCoroutine(coroutine);
		}
	}

	IEnumerator DoAutoAttack (GameObject target)
	{
		AutoAttackSeq[] attacks = new AutoAttackSeq[3] { AutoAttackSeq.ONE, AutoAttackSeq.TWO, AutoAttackSeq.THREE };

		foreach (AutoAttackSeq attack in attacks) {
			var isAlive = target.GetComponent<Hero>().IsAlive();
			if (!isAlive) {
				this.attackState.finished = true;
				yield break;
			}

			//FIRE!!!
			FireAutoAttack(target, attack);

			yield return new WaitForSeconds(1.0f);
		}
		this.attackState.finished = true;
	}

	void FireAutoAttack (GameObject target, AutoAttackSeq attack)
	{
//		Instantiate(autoAttackPrefab, target.transform.position, Quaternion.identity) as GameObject;
		target.GetComponent<Hero>().TakeDamage((int)attack);
	}

	public override Boolean StopAttack() {
		if (this.attackState.isFinished()) {
			// no need to stop anything, we're already done!
			return true;
		}
			
		if (!this.attackState.isInterruptable()) {
			return false;
		}

		stop(this.attackState);
		return true;
	}

	void stop (AttackState attackState)
	{
		// protect against the NONE inital state which will have a null enumerator
		if (attackState.getEnumerator() != null) {
			StopCoroutine(attackState.getEnumerator());
		}
		attackState.finished = true;
	}

}