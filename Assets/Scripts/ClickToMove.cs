using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Movement for a Character/Player
/// 
/// Movement based on https://www.youtube.com/watch?v=k7yx7D6MU6w
/// 
/// Synchronization based on https://unity3d.com/learn/tutorials/topics/multiplayer-networking/introduction-simple-multiplayer-example?playlist=29690
/// </summary>
public class ClickToMove : NetworkBehaviour {

    const int LEFT_MOUSE_BUTTON = 0;
    const int RIGHT_MOUSE_BUTTON = 1;
    // max number of historical positions tracked to determine if we should give up moving
    const int MAX_VELOCITY_HISTORY_COUNT = 10;
    // the minimum velocity avg that determines we are stuck
    const int MIN_STUCK_VELOCITY_AVG = 2;
    // tolerance for how close we need to get to a target before giving up
    const float TARGET_POSITION_TOLERANCE = .2F;

    //how fast the player moves.
    [SerializeField] [Range(1, 20)]
    private float speed = 1;

    private Queue<float> velocityHistory = new Queue<float>();

    //where we want to travel too. synchronized so players move on all clients
    [SyncVar]
    private Vector3 targetPosition;

    //toggle to check track if we are moving or not. synchronized so players move on all clients
    [SyncVar]
    private bool isMoving;

    // The Animator is what controls the switching from one animator to the other
    private Animator anim;

    // gameobject who this player is currently targetting
    private GameObject playerTarget;

    enum PlayerInput { MOVE, SELECT, BUTTON1, BUTTON2, BUTTON3, BUTTON4, CHAR1, CHAR2, CHAR3, NONE }

    /// <summary>
    /// Initialization of the script
    /// </summary>
    void Start()
    {
        targetPosition = transform.position;        
        isMoving = false;
        anim = gameObject.GetComponentInChildren<Animator>();

        // by default, show only your self health bar - not enemies
        if (hasAuthority)
        {
            GetComponentInChildren<Canvas>().enabled = true;
        }
    }



    /// <summary>
    /// Evaluated every frame
    /// </summary>
    void Update()
    {
        PlayerInput playerInput = getPlayerInput();

        if (playerInput == PlayerInput.SELECT)
        {
            selectTarget();
        }
  

        // detect clicks only for the local player otherwise one player will control all characters
        if (playerInput == PlayerInput.MOVE && hasAuthority)
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

    private void selectTarget()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Player")
        {
            if (playerTarget != null)
            {
                // deactivate old player target
                playerTarget.GetComponentInChildren<Canvas>().enabled = false;
            }

            playerTarget = hitInfo.collider.gameObject;
            playerTarget.GetComponentInChildren<Canvas>().enabled = true;
        }
    }

    private void StartWalkingAnimationIfNeeded()
    {
        if (anim.GetInteger("AnimationParameter") != 1)
        {
            anim.SetInteger("AnimationParameter", 6);
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
        CharacterController cc = GetComponent<CharacterController>();
        if (withinTargetPosition() || cantReachPosition())
        {
            isMoving = false;
            this.velocityHistory.Clear();
            return;
        }
        trackVelocityHistory(cc);

        transform.LookAt(targetPosition);

        // find the target position relative to the player:
        Vector3 dir = this.targetPosition - transform.position;
        // calculate movement at the desired speed:
        Vector3 movement = dir.normalized * speed * Time.deltaTime;
        // limit movement to never pass the target position:
        if (movement.magnitude > dir.magnitude)
        {
            movement = dir;
        }

        // move the character:
        cc.Move(movement);
            
        Debug.DrawLine(transform.position, this.targetPosition, Color.red);
    }

    /// <summary>
    /// Determines if we're close enough to the target position we're trying to move to
    /// </summary>
    /// <returns>true if we're within position</returns>
    private bool withinTargetPosition()
    {
        Vector3 difference = transform.position - this.targetPosition;
        return Math.Abs(difference.magnitude) < TARGET_POSITION_TOLERANCE;
    }

    /// <summary>
    /// Enqueues the current velocity in the stateful velocity tracker each frame
    /// </summary>
    /// <param name="cc">character controller we're tracking</param>
    private void trackVelocityHistory(CharacterController cc)
    {
        if (this.velocityHistory.Count >= MAX_VELOCITY_HISTORY_COUNT)
        {
            this.velocityHistory.Dequeue();
        }
        this.velocityHistory.Enqueue(cc.velocity.magnitude);
    }

    /// <summary>
    /// Determines if after MAX_VELOCITY_HISTORY_COUNT frames that we can't reach
    /// a position we've been trying to move towards
    /// </summary>
    /// <returns>true if we should give up trying to move to the target position</returns>
    private bool cantReachPosition()
    {
        if (this.velocityHistory.Count < MAX_VELOCITY_HISTORY_COUNT)
        {
            // we don't have enough velocity history to determine that we're stuck
            return false;
        }

        // we have enough history to determine if we're stuck
        float sumVelocities = 0;
        foreach (float velocity in this.velocityHistory)
        {
            sumVelocities += velocity;
        }
        return sumVelocities / this.velocityHistory.Count < MIN_STUCK_VELOCITY_AVG;
    }

    private PlayerInput getPlayerInput()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.tapCount == 1)
                {
                    return PlayerInput.SELECT;
                }
                if (touch.tapCount == 2)
                {
                    return PlayerInput.MOVE;
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(LEFT_MOUSE_BUTTON))
            {
                return PlayerInput.SELECT;
            }
            else if (Input.GetMouseButton(RIGHT_MOUSE_BUTTON))
            {
                return PlayerInput.MOVE;
            }
        }
        return PlayerInput.NONE;
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
