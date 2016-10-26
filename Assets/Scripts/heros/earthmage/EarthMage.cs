﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

class EarthMage : Hero
{
	public GameObject prefabProjectile;
	public GameObject prefabSpikes;
	public float projectileSpeed = 40;

	// initialized in awake
	private AttackState attackState;

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

	public override void StartAutoAttack (GameObject target) {
		if (this.attackState.isFinished()) {
			var coroutine = DoAutoAttack(target);
			this.attackState = AttackState.create(coroutine, true);
			StartCoroutine(coroutine);
		}
	}

	IEnumerator DoAutoAttack (GameObject target)
	{
		var autoAttackWaves = generateAutoAttacks();

		foreach (AutoAttack attack in autoAttackWaves) {
			var isAlive = target.GetComponent<Hero>().IsAlive();
			if (!isAlive) {
				this.attackState.finished = true;
				yield break;
			}

			//FIRE!!!
			attack.fire(gameObject, target);

			yield return new WaitForSeconds(1.0f);
		}
		this.attackState.finished = true;
	}

	AutoAttack[] generateAutoAttacks ()
	{
		AutoAttack[] autoAttackWaves = new AutoAttack[3];
		autoAttackWaves[0] = WaveOne.create(this.prefabProjectile, projectileSpeed);
		autoAttackWaves[1] = WaveTwo.create(this.prefabProjectile, projectileSpeed);
		autoAttackWaves[2] = WaveThree.create(this.prefabSpikes);
		return autoAttackWaves;
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