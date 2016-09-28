using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

/// <summary>
/// Movement for a Character/Player
/// 
/// Movement based on https://www.youtube.com/watch?v=k7yx7D6MU6w
/// 
/// Synchronization based on https://unity3d.com/learn/tutorials/topics/multiplayer-networking/introduction-simple-multiplayer-example?playlist=29690
/// </summary>
public class ClickToMove : NetworkBehaviour {


    //how fast the player moves.
    [SerializeField] [Range(1, 20)]
    private float speed = 1;

    //where we want to travel too. synchronized so players move on all clients
    [SyncVar]
    private Vector3 targetPosition;

    //toggle to check track if we are moving or not. synchronized so players move on all clients
    [SyncVar]
    private bool isMoving;

    //a more visual description of what the left mouse button code is.
    const int LEFT_MOUSE_BUTTON = 0;

    // The Animator is what controls the switching from one animator to the other
    private Animator anim;



    /// <summary>
    /// Initialization of the script
    /// </summary>
    void Start()
    {
        targetPosition = transform.position;        
        isMoving = false;
        anim = gameObject.GetComponentInChildren<Animator>();
    }



    /// <summary>
    /// Evaluated every frame
    /// </summary>
    void Update()
    {
        // detect clicks only for the local player otherwise one player will control all characters
        if (hasAuthority && Input.GetMouseButton(LEFT_MOUSE_BUTTON))
        {
            FindAndSetTargetPosition();
        }

        if (isMoving)
        {
            StartWalkingAnimationIfNeeded();
            MovePlayer();
        }
        else {
            // AnimationParameter is an integer that's used to switch between animations. When it's set to 0, the idle animation is being played.
            anim.SetInteger("AnimationParameter", 0); 
        }
    }

    private void StartWalkingAnimationIfNeeded()
    {
        if (anim.GetInteger("AnimationParameter") != 1)
        {
            anim.SetInteger("AnimationParameter", 1);
        }
    }

    /// <summary>
    /// Sets the target position we will travel too.
    /// </summary>
    private void FindAndSetTargetPosition()
    {
        Plane plane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        if (plane.Raycast(ray, out point))
        {
            CmdSetTargetPosition(ray.GetPoint(point));
        }

        // broadcast to everyone that this unit should start moving
        CmdSetMovement(true);
    }

    /// <summary>
    /// Moves the player in the right direction and also rotates them to look at the target position.
    /// When the player gets to the target position, stop them from moving.
    /// </summary>
    private void MovePlayer()
    {
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //if we are at the target position, then stop moving
        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
            
        Debug.DrawLine(transform.position, targetPosition, Color.red);
    }

    /// <summary>
    /// 
    /// 
    /// 
    /// All Commands below are used to send RPC (remote) calls from client->server
    /// 
    /// 
    /// 
    /// </summary>

    [Command]
    private void CmdSetMovement(Boolean val)
    {
        isMoving = val;
    }

    [Command]
    private void CmdSetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
