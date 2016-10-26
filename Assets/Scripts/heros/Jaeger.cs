using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Jaeger : Hero
{

    public override HeroType GetHeroType()
    {
        return HeroType.Jaeger;
    }

    public override void Selected()
    {
        //        Debug.Log("Jaeger hero selected");
    }

    public override void Attack(AttackType type)
    {
        Debug.Log("Jaeger attacking with: " + type);
	}

	public override void StartAutoAttack (GameObject target) {
		Debug.Log ("Jaeger autoattacking" + target);
	}

	public override Boolean StopAttack() {
		return true;
	}
}