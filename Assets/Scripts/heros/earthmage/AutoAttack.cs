using System;
using UnityEngine;
using System.Collections;


public abstract class AutoAttack : ScriptableObject {
	protected GameObject prefab;
	protected int damage;
	protected float speed;

	public abstract void fire(GameObject source, GameObject target);

	public abstract float waitTime ();
}

class WaveOne : AutoAttack {

	public static AutoAttack create(GameObject prefab, float speed) {
		WaveOne attack = ScriptableObject.CreateInstance<WaveOne>();
		attack.prefab = prefab;
		attack.damage = 30;
		attack.speed = speed;
		return attack;
	}

	public override void fire(GameObject source, GameObject target) {
		GameObject projectile = GameObject.Instantiate(prefab, source.transform.position, Quaternion.identity) as GameObject;
		projectile.GetComponent<ProjectileSeeker>().seek(target, speed, damage);
	}

	public override float waitTime() {
		return 0.1f;
	}
}


class WaveTwo : AutoAttack {

	public static AutoAttack create(GameObject prefab, float speed) {
		WaveTwo attack = ScriptableObject.CreateInstance<WaveTwo>();
		attack.prefab = prefab;
		// damage half of total damage since two projectiles are created
		attack.damage = 25;
		attack.speed = speed;
		return attack;
	}

	public override void fire(GameObject source, GameObject target) {
		GameObject projectile1 = Instantiate(prefab, new Vector3(-2,0,0) + source.transform.position, Quaternion.identity) as GameObject;
		GameObject projectile2 = Instantiate(prefab, new Vector3(2,0,0) + source.transform.position, Quaternion.identity) as GameObject;
		projectile1.GetComponent<ProjectileSeeker>().seek(target, speed, damage);
		projectile2.GetComponent<ProjectileSeeker>().seek(target, speed, damage);
	}

	public override float waitTime() {
		return 0.7f;
	}
}


class WaveThree : AutoAttack {

	public static AutoAttack create(GameObject prefab) {
		WaveThree attack = ScriptableObject.CreateInstance<WaveThree>();
		attack.prefab = prefab;
		attack.damage = 70;
		// speed not used
		attack.speed = -1;
		return attack;
	}

	public override void fire(GameObject source, GameObject target) {
		GameObject spikes = Instantiate(prefab, target.transform.position, Quaternion.identity) as GameObject;
		spikes.GetComponent<TimedStationaryAttack>().init(target, damage, 2);
	}

	public override float waitTime() {
		return 1.8f;
	}
}
