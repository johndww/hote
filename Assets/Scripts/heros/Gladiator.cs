using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Gladiator : Hero
{

    public override HeroType GetHeroType()
    {
        return HeroType.Gladiator;
    }

    public override void Selected()
    {
//        Debug.Log("Gladiator hero selected");
    }

    public override void Attack(AttackType type)
    {
        Debug.Log("Gladiator attacking with: " + type);
	}

	public override void AutoAttack (GameObject target) {
		Debug.Log ("gladiator autoattacking" + target);
	}

	public override Boolean StopAttack() {
		return true;
	}
}