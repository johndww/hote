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

}

enum HeroType
{
    EarthMage,
    WindMage,
    Gladiator
}
