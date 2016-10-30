using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

class EarthMage : Hero
{
	public GameObject prefabProjectile;
	public GameObject prefabSpikes;
    public GameObject prefabGreenAttack;
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

    public override void BlueAttack()
    {
        Debug.Log("earth mage attacking with blue");
    }

    public override void GreenAttack()
    {
        Debug.Log("earth mage attacking with green");
        //TODO start this as a coroutine with destroy. prevent incoming damage. heal. fix animator. prevent walking
        GameObject.Instantiate(this.prefabGreenAttack, gameObject.transform.position, Quaternion.identity);
    }

    public override void PurpleAttack()
    {
        Debug.Log("earth mage attacking with purple");
    }

    public override void RedAttack()
    {
        Debug.Log("earth mage attacking with red");
    }

    public override void StartAutoAttack (GameObject target) {
		var coroutine = DoAutoAttack(target);
		this.attackState = AttackState.create(coroutine, true);
		StartCoroutine(coroutine);
	}

	public override bool IsAttacking() {
		return !this.attackState.isFinished();
	}

	IEnumerator DoAutoAttack (GameObject target)
	{
		var autoAttackWaves = generateAutoAttacks();

		foreach (AutoAttack attack in autoAttackWaves) {
			if (isDead()) {
				// we're dead, probably can't keep attacking :\
				yield break;
			}

			// timing for each attack wave
			yield return new WaitForSeconds(attack.waitTime());

			if (target.GetComponent<Hero>().IsDead() || OutOfRange(target)) {
				// can't attack anymore
				this.attackState.finished = true;
				yield break;
			}

			// FIRE!!!
			attack.fire(gameObject, target);

		}

		yield return new WaitForSeconds (1.0f);
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
