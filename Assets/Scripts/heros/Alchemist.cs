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

    public override void BlueAttack()
    {
        Debug.Log("earth mage attacking with blue");
    }

    public override void GreenAttack()
    {
        Debug.Log("earth mage attacking with green");
    }

    public override void PurpleAttack()
    {
        Debug.Log("earth mage attacking with purple");
    }

    public override void RedAttack()
    {
        Debug.Log("earth mage attacking with red");
    }

    public override void StartAutoAttack (GameObject target) {
		Debug.Log ("Alchemist autoattacking" + target);
	}

	public override Boolean StopAttack() {
		return true;
	}

	public override bool IsAttacking() {
		return false;
	}
}