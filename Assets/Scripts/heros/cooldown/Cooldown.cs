using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

class Cooldown : ScriptableObject
{
	private int duration;

	private bool inProgress;
	private int countdown;

	private GameObject cooldownGameObject;

	public static Cooldown create(GameObject cooldownGameObject) {
		var cooldown = ScriptableObject.CreateInstance<Cooldown>();
		cooldown.cooldownGameObject = cooldownGameObject;
		return cooldown;
	}

	public IEnumerator Activate(int duration) {
		this.duration = duration;
		this.countdown = duration;

		this.cooldownGameObject.SetActive(true);
		return start();
	}

	private IEnumerator start ()
	{
		this.cooldownGameObject.GetComponentInChildren<Text>().text = countdown.ToString();

		this.inProgress = true;
		for (int i = 0; i < duration; i++) {
			yield return new WaitForSeconds (1);
			countdown--;
			this.cooldownGameObject.GetComponentInChildren<Text>().text = countdown.ToString();
		}
		this.cooldownGameObject.SetActive(false);
		this.inProgress = false;
	}

	public bool InProgress ()
	{
		return inProgress;
	}
}


