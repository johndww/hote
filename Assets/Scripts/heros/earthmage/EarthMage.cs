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
	public GameObject prefabPurpleAttack;

	public float projectileSpeed = 40;

	// initialized in awake
	private AttackState attackState;
	private Animator anim;
	CharacterControl.AttackUIOverrideFunction uiOverrideFunction;

	void Awake() {
		this.attackState = AttackState.None();
		this.anim = GetComponentInChildren<Animator>();

		// we could just reference charactercontrol here. the function might be not necessary
		// but it seems strange to have the char control->hero->char control cyclic references
		this.uiOverrideFunction = GetComponent<CharacterControl>().GetAttackUIOverrideFunction();
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
		CollectLocationUI collectLocationUI = GetComponent<CollectLocationUI>();
		// enable polling for selected location
		collectLocationUI.enabled = true;
		this.uiOverrideFunction(true);

		var coroutine = DoBlueAttack(collectLocationUI);
		this.attackState = AttackState.create(coroutine, true, BlueAttackComplete);
		StartCoroutine(coroutine);

		return true;
    }

	private void BlueAttackComplete() {
		CollectLocationUI collectLocationUI = GetComponent<CollectLocationUI>();
		collectLocationUI.enabled = false;
		collectLocationUI.Reset();
		this.uiOverrideFunction(false);
	}

	private IEnumerator DoBlueAttack (CollectLocationUI collectLocationUI)
	{
		// give us an extra frame for the substate animation control
		yield return null;

		while (!collectLocationUI.IsLocationSelected()) {
			yield return new WaitForSeconds (0.1f);
		}

		this.uiOverrideFunction(false);

		// now that we've actually selected a location, change to be uninterruptable
		this.attackState = AttackState.create();

		// start the travel animation now that we've selected. wait a frame.
		this.anim.SetTrigger("select");

		this.immobile = true;
		this.invulnerable = true;

		yield return new WaitForSeconds (2.0f);

		transform.position = collectLocationUI.GetLocation();
		collectLocationUI.Reset();

		yield return new WaitForSeconds (2.0f);

		// exit and give a frame for the transition
		this.anim.SetTrigger("exitSubState");
		yield return null;

		this.immobile = false;
		this.invulnerable = false;
		this.attackState.finished = true;
	}

	public override bool GreenAttack()
    {
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

		yield return new WaitForSeconds(2.8f);
		Destroy(rocks);
		this.immobile = false;
		this.invulnerable = false;
		this.attackState.finished = true;
	}

	public override bool PurpleAttack()
	{
		CollectLineLocationUI collectLineLocationUI = GetComponent<CollectLineLocationUI>();
		// enable polling for selected location
		collectLineLocationUI.enabled = true;
		this.uiOverrideFunction(true);

		var coroutine = DoPurpleAttack(collectLineLocationUI);
		this.attackState = AttackState.create(coroutine, true, PurpleAttackComplete);
		StartCoroutine(coroutine);

		return true;
	}

	private void PurpleAttackComplete() {
		CollectLineLocationUI collectLineLocationUI = GetComponent<CollectLineLocationUI>();
		collectLineLocationUI.enabled = false;
		this.uiOverrideFunction(false);
		collectLineLocationUI.Reset();
	}

	private IEnumerator DoPurpleAttack (CollectLineLocationUI collectLineLocationUI)
	{
		// give us an extra frame for the substate animation control
		yield return null;

		while (!collectLineLocationUI.IsComplete()) {
			yield return new WaitForSeconds (0.1f);
		}

		this.uiOverrideFunction(false);

		// now that we've actually selected a location, change to be uninterruptable
		this.attackState = AttackState.create();

		// start the travel animation now that we've selected. wait a frame.
		this.anim.SetTrigger("select");

		this.immobile = true;

		var rocks = GameObject.Instantiate(this.prefabPurpleAttack, collectLineLocationUI.GetLocation(), Quaternion.identity) as GameObject;

		//TODO i have no idea how this works. geometry is not my strong suit
		// theres definitely a way i can do this in one operation but this works so im done
		rocks.transform.rotation = Quaternion.LookRotation(collectLineLocationUI.GetMoveDelta());
		rocks.transform.rotation *= Quaternion.Euler(0,90f,0);

		collectLineLocationUI.Reset();

		StartCoroutine(DestroyPurpleWall(rocks));

		yield return new WaitForSeconds (3.0f);

		// exit and give a frame for the transition
		this.anim.SetTrigger("exitSubState");
		yield return null;

		this.immobile = false;
		this.attackState.finished = true;
	}

	IEnumerator DestroyPurpleWall (GameObject rocks)
	{
		yield return new WaitForSeconds (12.0f);
		Destroy(rocks);
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
			//NOTE: stopping a coroutine mid execution nees to be thought through
			// an attackState that is interruptable must only be interruptable if
			// it doesn't need to de-allocate objects other than with the complete function
			StopCoroutine(attackState.getCoroutine());
		}
		// we've interrupted this attack. run the complete function to close resources/reset state
		attackState.getCompleteFunction()();
		attackState.finished = true;
	}
}
