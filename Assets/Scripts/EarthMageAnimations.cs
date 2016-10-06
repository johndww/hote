using UnityEngine;
using System.Collections;

public class EarthMageAnimations : MonoBehaviour {

    // There are a bunch of animation models that I need to hide and then enable only for certain animations. I'm linking them here as game objects so I can turn them on when needed, they're disabled by default.
    public GameObject GreenItemOne;
    public GameObject GreenItemTwo;
    public GameObject GreenItemThree;
    public GameObject GreenItemFour;

    public GameObject AutoAttacksItemOne;
    public GameObject AutoAttacksItemTwo;
    public GameObject AutoAttacksItemThree;
    public GameObject AutoAttacksItemFour;
    public GameObject AutoAttacksItemFive;


    // These will be turned on in the inspector to specify which animation I want the model to play.
    public bool Idle;
    public bool Green;
    public bool Blue;
    public bool Red;
    public bool Purple;
    public bool AutoAttacks;
    public bool Walking;
    public bool Death;

    // Animation requirements
    private Animator anim;


    // Use this for initialization
    void Start () {

        anim = gameObject.GetComponentInChildren<Animator>();

        if (Idle) {
            anim.SetInteger("AnimationPosition", 0);
        }

        if (Green)
        {
            anim.SetInteger("AnimationPosition", 1);
            GreenItemOne.gameObject.SetActive(true);
            GreenItemTwo.gameObject.SetActive(true);
            GreenItemThree.gameObject.SetActive(true);
            GreenItemFour.gameObject.SetActive(true);
        }

        if (Blue)
        {
            anim.SetInteger("AnimationPosition", 2);
        }

        if (Red)
        {
            anim.SetInteger("AnimationPosition", 3);
        }

        if (Purple)
        {
            anim.SetInteger("AnimationPosition", 4);
        }

        if (AutoAttacks)
        {
            anim.SetInteger("AnimationPosition", 5);
            AutoAttacksItemOne.gameObject.SetActive(true);
            AutoAttacksItemTwo.gameObject.SetActive(true);
            AutoAttacksItemThree.gameObject.SetActive(true);
            AutoAttacksItemFour.gameObject.SetActive(true);
            AutoAttacksItemFive.gameObject.SetActive(true);
        }

        if (Walking)
        {
            anim.SetInteger("AnimationPosition", 6);
        }

        if (Death)
        {
            anim.SetInteger("AnimationPosition", 7);

        }

    }
	
}
