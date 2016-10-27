using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Character control for an indivudal hero.
/// 
/// Synchronization based on https://unity3d.com/learn/tutorials/topics/multiplayer-networking/introduction-simple-multiplayer-example?playlist=29690
/// </summary>
public class CharacterControl : MonoBehaviour {
    private static string ANIM_STATE = "animation";
    private static Dictionary<AttackType, Anim> attackAnimMap = generateAttackTypeToAnimMap();

	public LayerMask heroLayerMask;

    // The Animator is what controls the switching from one animator to the other
    private Animator anim;
    private NavMeshAgent navMesh;
	private GameObject locationPointer;

    private Boolean isWalking;

    // gameobject who this player is currently targeting
    private GameObject playerTarget;

	// bookmark to know we've seen the hero as dead. the death source of truth is the hero
	private bool charIsDead;

	private Hero hero;


    /// <summary>
    /// Initialization of the script
    /// </summary>
    void Start()
    {
        this.anim = GetComponentInChildren<Animator>();
        this.navMesh = GetComponent<NavMeshAgent>();
		this.hero = GetComponent<Hero> ();
		this.locationPointer = GameObject.FindWithTag("location_pointer");

        // characters can walk through each other
        Physics.IgnoreLayerCollision(8, 8);
    }

    /// <summary>
    /// Evaluated every frame
    /// </summary>
    void Update()
    {
		if (isDead()) {
			return;
		}

        if (isWalking)
        {
            WalkControl();
			return;
        }

		if (this.playerTarget == null) {
			// no need to do any more work if we haven't targetted anyone
			return;
		}

		if (RotateTowards(this.playerTarget.transform)) {
			// need to rotate more before we can do anything
			return;
		}

		if (!this.hero.IsAttacking()) {
			AttemptToAutoAttack ();
		}
    }

	/// <summary>
	/// Hacky method. This should simply be represented by this.hero.IsDead(). However,
	/// there seems to be a bug with Unity (or more likely, with our code) that prevents us from
	/// transitioning from Walking to Dead in one frame.  This block of code effectively gives us 1 frame extra
	/// to transition from Walking->Idle and then AnyState->Dead which appears to work.
	/// </summary>
	bool isDead ()
	{
		if (this.charIsDead) {
			if (this.anim.GetInteger(ANIM_STATE) != (int)Anim.DEATH) {
				this.anim.SetInteger(ANIM_STATE, (int)Anim.DEATH);
			}
		}

		if (this.hero.isDead() && !this.charIsDead) {
			this.charIsDead = true;
			if (this.isWalking) {
				StopWalking();
			}
			if (this.hero.IsAttacking()) {
				this.hero.StopAttack();
			}
		}
		return this.charIsDead;
	}

	private void AttemptToAutoAttack ()
	{
		if (this.playerTarget.GetComponent<Hero>().isDead()) {
			this.anim.SetInteger(ANIM_STATE, (int)Anim.IDLE);
			return;
		}

		if (this.hero.isTooFarToAutoAttack()) {
			// move a bit towards target
		}
		else {
			StartAutoAttackAnimation();
			this.hero.StartAutoAttack(this.playerTarget);
		}
	}

	private void StartAutoAttackAnimation ()
	{
		// start autoattack animation if we weren't previously autoattacking
		if (this.anim.GetInteger(ANIM_STATE) != (int)Anim.AUTOATTACK) {
			this.anim.SetInteger(ANIM_STATE, (int)Anim.AUTOATTACK);
		}
		else {
			// if we're already autoattack, we need to reset the animation
			this.anim.Play("AutoAttacks", -1, 0f);
		}
	}

	private bool RotateTowards (Transform target) {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		float angle = Quaternion.Angle(transform.rotation, lookRotation);

		if (angle < 5f) {
			// slerping is strange. give up rotating if we're less than 5 degrees angled from target
			// to smooth out (and speed up) the rotation transition
			return false;
		}

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
		return true;
	}

    public void SetTarget()
    {
		if (this.hero.isDead()) {
			return;
		}

        RaycastHit hitInfo = new RaycastHit();
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, 200, this.heroLayerMask))
        {
			//TODO avoid targetting friends too!

			// can't target ourselves
			var targetted = hitInfo.collider.gameObject;
			if (targetted.GetInstanceID() == gameObject.GetInstanceID()) {
				return;
			}

			// haven't selected anyone yet, just accept the new target
			if (playerTarget == null) {
				playerTarget = hitInfo.collider.gameObject;
			}

			// targetting the same target, nothing to do
			if (targetted.GetInstanceID() == playerTarget.GetInstanceID()) {
				return;
			}

			// only switch to another target if we can stop attacking
			if (GetComponent<Hero>().StopAttack()) {
				playerTarget = hitInfo.collider.gameObject;

				Debug.Log("new target " + playerTarget);
			}
        }
    }

    private void WalkControl()
    {
        // navmesh is buggy. need to check alot to determine that we're done walking (or reached destination)
        if ((!this.navMesh.pathPending)
            && (this.navMesh.remainingDistance <= this.navMesh.stoppingDistance)
            && (!this.navMesh.hasPath || this.navMesh.velocity.sqrMagnitude == 0f))
        {
            StopWalking();
        }
    }

    private void StopWalking()
    {
        this.isWalking = false;
        this.navMesh.Stop();
        this.navMesh.ResetPath();
        this.anim.SetInteger(ANIM_STATE, (int)Anim.IDLE);
    }

    /// <summary>
    /// Moves the player in the right direction and also rotates them to look at the target position.
    /// When the player gets to the target position, stop them from moving.
    /// </summary>

    public void Move()
    {
		if (this.hero.isDead()) {
			return;
		}

		// attempt to stop attacking (if attacking) so we can move
		if (!this.hero.StopAttack()) {
			// can't stop attacking so the move command is ignored
			return;
		}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            navMesh.destination = hit.point;

			this.locationPointer.transform.position = navMesh.destination;
			this.locationPointer.GetComponent<Animator>().SetTrigger("mark");

            // might have already been walking. only start the animation if we just started
            if (!isWalking)
            {
                isWalking = true;
                this.anim.SetInteger(ANIM_STATE, (int) Anim.WALK);
            }
        }
    }

    public void Attack(AttackType type)
    {
		if (this.hero.isDead()) {
			return;
		}

        StopWalking();
        this.anim.SetInteger(ANIM_STATE, (int)attackAnimMap[type]);
		this.hero.Attack(type);
    }

    private static Dictionary<AttackType, Anim> generateAttackTypeToAnimMap()
    {
        Dictionary<AttackType, Anim> map = new Dictionary<AttackType, Anim>();
        map.Add(AttackType.GREEN, Anim.GREEN);
        map.Add(AttackType.BLUE, Anim.BLUE);
        map.Add(AttackType.RED, Anim.RED);
        map.Add(AttackType.PURPLE, Anim.PURPLE);
        return map;
    }

    enum Anim { IDLE, WALK, AUTOATTACK, GREEN, BLUE, RED, PURPLE, DEATH}
}
