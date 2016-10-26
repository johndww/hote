using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Alchemist : Hero
{

    public override HeroType GetHeroType()
    {
        return HeroType.Alchemist;
    }

    public override void Selected()
    {
        //        Debug.Log("Alchemist hero selected");
    }

    public override void Attack(AttackType type)
    {
        Debug.Log("Alchemist attacking with: " + type);
	}

	public override void StartAutoAttack (GameObject target) {
		Debug.Log ("Alchemist autoattacking" + target);
	}

	public override Boolean StopAttack() {
		return true;
	}
}