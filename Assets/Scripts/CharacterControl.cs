using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Movement for a Character
/// 
/// Synchronization based on https://unity3d.com/learn/tutorials/topics/multiplayer-networking/introduction-simple-multiplayer-example?playlist=29690
/// </summary>
public class CharacterControl : MonoBehaviour {
    private static string ANIM_STATE = "animation";
    private static Dictionary<AttackType, Anim> attackAnimMap = generateAttackTypeToAnimMap();

    // The Animator is what controls the switching from one animator to the other
    private Animator anim;
    private NavMeshAgent navMesh;

    private Boolean isWalking;

    // gameobject who this player is currently targetting
    private GameObject playerTarget;

    /// <summary>
    /// Initialization of the script
    /// </summary>
    void Start()
    {
        this.anim = GetComponentInChildren<Animator>();
        this.navMesh = GetComponent<NavMeshAgent>();

        // characters can walk through each other
        Physics.IgnoreLayerCollision(8, 8);
    }

    /// <summary>
    /// Evaluated every frame
    /// </summary>
    void Update()
    {
        if (isWalking)
        {
            WalkControl();
        }
    }

    //private void selectTarget()
    //{
    //    RaycastHit hitInfo = new RaycastHit();
    //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Player")
    //    {
    //        if (playerTarget != null)
    //        {
    //            // deactivate old player target
    //            playerTarget.GetComponentInChildren<Canvas>().enabled = false;
    //        }

    //        playerTarget = hitInfo.collider.gameObject;
    //        playerTarget.GetComponentInChildren<Canvas>().enabled = true;
    //    }
    //}

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200))
        {
            navMesh.destination = hit.point;

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
        GetComponent<Hero>().Attack(type);
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
