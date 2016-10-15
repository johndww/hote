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
        Debug.Log("walking: " + isWalking + "DIST: " + this.navMesh.remainingDistance + ". hashPath" + this.navMesh.hasPath + " velocity " + this.navMesh.velocity.sqrMagnitude);

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
        if ((!this.navMesh.pathPending)
            && (this.navMesh.remainingDistance <= this.navMesh.stoppingDistance)
            && (!this.navMesh.hasPath || this.navMesh.velocity.sqrMagnitude == 0f))
        {
            this.isWalking = false;
            this.anim.SetInteger(ANIM_STATE, (int)Anim.IDLE);
        }
    }

    /// <summary>
    /// Moves the player in the right direction and also rotates them to look at the target position.
    /// When the player gets to the target position, stop them from moving.
    /// </summary>

    public void Move()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        Boolean hasHit = Physics.Raycast(ray, out hit, 200);
        Debug.Log("ray: " + ray + "hasHit " + hasHit + " hit: " + hit);

        if (hasHit)
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

    enum Anim { IDLE, WALK, AUTOATTACK, GREEN, BLUE, RED, PURPLE, DEATH}
}
