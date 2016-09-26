using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Animator anim; // The Animator is what controls the switching from one animator to the other


    public float smoothing = 71f;
    public Vector3 Target
    {
        get { return target; }
        set
        {
            target = value;

            StopCoroutine("Movement");
            StartCoroutine("Movement", target);
        }
    }


    private Vector3 target;


    void Start ()
    {
        anim = gameObject.GetComponentInChildren<Animator>(); // I need to get the animator component to have access to it
    }

    IEnumerator Movement(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, smoothing * Time.deltaTime);
            anim.SetInteger("AnimationParameter", 1); // When the AnimationParameter is set to 1, the walk animation is being played.
            yield return null;
        }
        anim.SetInteger("AnimationParameter", 0); // AnimationParameter is an integer that's used to switch between animations. When it's set to 0, the idle animation is being played.
    }
}
