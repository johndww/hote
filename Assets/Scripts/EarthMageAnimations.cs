using UnityEngine;
using System.Collections;

public class EarthMageAnimations : MonoBehaviour {


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
