using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Syphoness : Hero
{

    public override HeroType GetHeroType()
    {
        return HeroType.Syphoness;
    }

    public override void Selected()
    {
        //        Debug.Log("Syphoness hero selected");
    }

    public override void Attack(AttackType type)
    {
        Debug.Log("Syphoness attacking with: " + type);
    }

    public override void AutoAttack(GameObject target)
    {
        Debug.Log("Syphoness autoattacking" + target);
    }

    public override Boolean StopAttack()
    {
        return true;
    }
}