using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cooldowns : MonoBehaviour
{
	private GameObject uiAbilities;
	private Dictionary<AttackType, Cooldown> cooldowns = new Dictionary<AttackType, Cooldown> ();

	public void Initialize (GameObject uiAbilitiesGameObject)
	{
		this.uiAbilities = uiAbilitiesGameObject;
	}

	public void Activate (AttackType type, int duration)
	{
		if (InProgress(type)) {
			throw new Exception ("type: " + type + " is already in progress. cannot activate");
		}
		var coroutine = GetOrCreateCooldown(type).Activate(duration);
		StartCoroutine(coroutine);
	}

	private Cooldown GetOrCreateCooldown (AttackType type)
	{
		if (!this.cooldowns.ContainsKey(type)) {
			this.cooldowns [type] = Cooldown.create(GetCooldownGameObject(type));
		}
		return this.cooldowns[type];
	}

	public bool InProgress(AttackType type) {
		return !this.cooldowns.ContainsKey(type) ? false : this.cooldowns [type].InProgress();
	}

	private GameObject GetCooldownGameObject (AttackType type)
	{
		switch (type) {
		case AttackType.BLUE:
			return this.uiAbilities.transform.Find("Ability Blue/Cooldown Cover").gameObject;
		case AttackType.GREEN:
			return this.uiAbilities.transform.Find("Ability Green/Cooldown Cover").gameObject;
		case AttackType.PURPLE:
			return this.uiAbilities.transform.Find("Ability Purple/Cooldown Cover").gameObject;
		case AttackType.RED:
			return this.uiAbilities.transform.Find("Ability Red/Cooldown Cover").gameObject;
		default:
			throw new ArgumentException ("unknown type: " + type);
		}
	}
}

