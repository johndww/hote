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
		if (this.hero.isDead()) {
			return;
		}

        if (isWalking)
        {
            WalkControl();
        }
		else if (this.playerTarget != null && !this.playerTarget.GetComponent<Hero>().isDead()) {
			AttemptToAutoAttack ();
		}
    }

	void AttemptToAutoAttack ()
	{
		if (this.hero.isTooFarToAutoAttack()) {
			// move a bit towards target
		}
		else {
			
			// autoattack
			RotateTowards(this.playerTarget.transform);
			this.hero.StartAutoAttack(this.playerTarget);
		}
	}

	private void RotateTowards (Transform target) {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

    public void SetTarget()
    {
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
