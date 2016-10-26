using UnityEngine;
using System.Collections;

public class TimedStationaryAttack : MonoBehaviour {

	private GameObject target;
	private int damage;
	private float lifeDuration;

	private bool running;

	// Update is called once per frame
	void Update () {
		if (this.target == null || this.running) {
			return;
		}

		StartCoroutine(DestoryAfterAnimation());
		this.running = true;
	}

	IEnumerator DestoryAfterAnimation ()
	{
		target.GetComponent<Hero>().TakeDamage(damage);
		yield return new WaitForSeconds(lifeDuration);
		Destroy(gameObject);
	}

	public void init (GameObject target, int damage, float lifeDuration)
	{
		this.target = target;
		this.damage = damage;
		this.lifeDuration = lifeDuration;
	}
}
