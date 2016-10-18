using UnityEngine;
using System.Collections;

public class CharacterAnimationsPreview : MonoBehaviour {

    // These will be turned on in the inspector to specify which animation I want the model to play.
    public bool Idle;
    public bool Green;
    public bool Blue;
    public bool Red;
    public bool Purple;
    public bool AutoAttacks;
    public bool AutoAttacksWalking;
    public bool Walking;
    public bool Death;

    // Animation requirements
    private Animator anim;


    // Use this for initialization
    void Start () {

        anim = gameObject.GetComponentInChildren<Animator>();

        if (Idle) {
            anim.SetInteger("animation", 0);
        }

        if (Green)
        {
            anim.SetInteger("animation", 3);
        }

        if (Blue)
        {
            anim.SetInteger("animation", 4);
        }

        if (Red)
        {
            anim.SetInteger("animation", 5);
        }

        if (Purple)
        {
            anim.SetInteger("animation", 6);
        }

        if (AutoAttacks)
        {
            anim.SetInteger("animation", 2);
        }

        if (Walking)
        {
            anim.SetInteger("animation", 1);
        }
        if (AutoAttacksWalking)
        {
            anim.SetInteger("animation", 8);
        }
        if (Death)
        {
            anim.SetInteger("animation", 7);

        }

    }
	
}
