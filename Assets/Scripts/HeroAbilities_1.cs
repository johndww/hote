using UnityEngine;
using System.Collections;

public class HeroAbilities_1 : MonoBehaviour {

    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public void AbilityOneActions() // All the actions for this ability
    {
        Debug.Log("Hero 1: Ability 1");
        anim.SetInteger("AnimationParameter", 1);
    }


    public void AbilityTwoActions() // All the actions for this ability
    {
        Debug.Log("Hero 1: Ability 2");
        anim.SetInteger("AnimationParameter", 2);
    }


    public void AbilityThreeActions() // All the actions for this ability
    {
        Debug.Log("Hero 1: Ability 3");
        anim.SetInteger("AnimationParameter", 3);
    }


    public void AbilityFourActions() // All the actions for this ability
    {
        Debug.Log("Hero 1: Ability 4");
        anim.SetInteger("AnimationParameter", 4);
    }

}
