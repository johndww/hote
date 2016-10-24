using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

class EarthMage : Hero
{
	public GameObject autoAttackPrefab;

	private Boolean isAutoAttacking = false;
	private enum AutoAttackSeq { ONE=30, TWO=50, THREE=70 }

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
		if (!this.isAutoAttacking) {
			StartCoroutine(DoAutoAttack(target));
		}
	}

	IEnumerator DoAutoAttack (GameObject target)
	{
		this.isAutoAttacking = true;

		AutoAttackSeq[] attacks = new AutoAttackSeq[3] { AutoAttackSeq.ONE, AutoAttackSeq.TWO, AutoAttackSeq.THREE };

		foreach (AutoAttackSeq attack in attacks) {
			var isAlive = target.GetComponent<Hero>().IsAlive();
			if (!isAlive) {
				this.isAutoAttacking = false;
				yield break;
			}

			//FIRE!!!
			FireAutoAttack(target, attack);

			yield return new WaitForSeconds(1.0f);
		}
		this.isAutoAttacking = false;
	}

	void FireAutoAttack (GameObject target, AutoAttackSeq attack)
	{
//		Instantiate(autoAttackPrefab, target.transform.position, Quaternion.identity) as GameObject;
		target.GetComponent<Hero>().TakeDamage((int)attack);
	}

	public override Boolean StopAttack() {
		//TODO
		return true;
	}
}