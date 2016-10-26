using UnityEngine;
using System.Collections;

public class ProjectileSeeker : MonoBehaviour {
	
	private GameObject target;
	private float speed;
	private int damage;

	void Update() {
		if (target == null) {
			return;
		}

		if (Vector3.Distance(target.transform.position, transform.position) < .1) {
			target.GetComponent<Hero>().TakeDamage(damage);
			Destroy(gameObject);
		}

		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
	}

	public void seek (GameObject target, float speed, int damage) {
		this.target = target;
		this.speed = speed;
		this.damage = damage;
	}
}
