using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EarthMage : Hero
{

    public override HeroType GetHeroType()
    {
        return HeroType.EarthMage;
    }

    public override void Selected()
    {
        Debug.Log("earth mage hero selected");
    }

    public override void Attack(AttackType type)
    {
        Debug.Log("earth mage attacking with: " + type);
    }
}