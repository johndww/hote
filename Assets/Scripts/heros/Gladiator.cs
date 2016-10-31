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

    public override bool BlueAttack()
    {
        Debug.Log("earth mage attacking with blue");
		return true;
    }

    public override bool GreenAttack()
    {
        Debug.Log("earth mage attacking with green");
		return true;
    }

    public override bool PurpleAttack()
    {
        Debug.Log("earth mage attacking with purple");
		return true;
    }

	public override bool RedAttack(GameObject target)
    {
        Debug.Log("earth mage attacking with red");
		return true;
    }

    public override void StartAutoAttack (GameObject target) {
		Debug.Log ("Gladiator autoattacking" + target);
	}

	public override Boolean StopAttack() {
		return true;
	}

	public override bool IsAttacking() {
		return false;
	}
}