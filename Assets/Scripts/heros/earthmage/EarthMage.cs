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
	public GameObject prefabRedAttack;

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

    public override bool BlueAttack()
    {
		var attackUIOverride = GetComponent<AttackUIOverride>();

		if (attackUIOverride.isActiveAndEnabled) {
			attackUIOverride.enabled = false;

			if (!attackUIOverride.IsLocationSelected()) {
				attackUIOverride.Reset();
				return false;
			}
			var coroutine = DoBlueAttack(attackUIOverride.GetLocation());
			this.attackState = AttackState.create(coroutine, false);
			StartCoroutine(coroutine);
			attackUIOverride.Reset();
			return true;
		}
		else {
			GetComponent<AttackUIOverride>().enabled = true;
			return false;
		}
    }

	IEnumerator DoBlueAttack (Vector3 location)
	{
		this.immobile = true;
		this.invulnerable = true;
		yield return new WaitForSeconds (2.0f);

		transform.position = location;

		yield return new WaitForSeconds (2.0f);

		this.immobile = false;
		this.invulnerable = false;
		this.attackState.finished = true;
	}

	public override bool GreenAttack()
    {
        Debug.Log("earth mage attacking with green");
        //TODO start this as a coroutine with destroy. prevent incoming damage. heal. fix animator. prevent walking
		var coroutine = DoGreenAttack();
		this.attackState = AttackState.create(coroutine, false);
		StartCoroutine(coroutine);
		return true;
    }

	private IEnumerator DoGreenAttack ()
	{
		this.immobile = true;
		this.invulnerable = true;
		var rocks = GameObject.Instantiate(this.prefabGreenAttack, gameObject.transform.position, Quaternion.identity) as GameObject;

		yield return new WaitForSeconds(1.0f);

		Heal(300);
		Debug.Log("earth mage attempted healing for 300. new hp: " + this.hp);

		yield return new WaitForSeconds(2.3f);
		Destroy(rocks);
		this.immobile = false;
		this.invulnerable = false;
		this.attackState.finished = true;
	}

	public override bool PurpleAttack()
    {
        Debug.Log("earth mage attacking with purple");
		return true;
    }

	public override bool RedAttack(GameObject target)
    {
		if (OutOfRange(target)) {
			return false;
		}

		var coroutine = DoRedAttack(target);
		this.attackState = AttackState.create(coroutine, false);
		StartCoroutine(coroutine);
		return true;
    }

	IEnumerator DoRedAttack (GameObject target)
	{
		this.immobile = true;

		var spikes = GameObject.Instantiate(this.prefabRedAttack, target.transform.position, Quaternion.identity) as GameObject;
		target.GetComponent<Hero>().TakeDamageOverTime(100, 4, 1);

		yield return new WaitForSeconds (2);
		Destroy(spikes);

		this.attackState.finished = true;
		this.immobile = false;
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
		if (attackState.getCoroutine() != null) {
			StopCoroutine(attackState.getCoroutine());
		}
		attackState.finished = true;
	}
}
