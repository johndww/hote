using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

abstract class Hero : MonoBehaviour
{
    public abstract HeroType GetHeroType();

    public abstract void Selected();

    public abstract void Attack(AttackType attack);

	public abstract void AutoAttack (GameObject playerTarget);

	public Boolean isTooFarToAutoAttack ()
	{
		//TODO make abstract for heros to determine
		return false;
	}
}

enum HeroType
{
    EarthMage,
    WindMage,
    Gladiator,
    HolyWarrior
}
