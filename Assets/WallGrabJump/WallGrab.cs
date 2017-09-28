using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrab : MonoBehaviour {

    CharacterController player;

    public float speedMove;
    public float speedTurn;
    Vector3 velocity = Vector3.zero;
    bool isJumping = false;
    bool isOnWall = false;
    bool didWallJump = false;
    public float impulseJump = 15;
    public float impulseOffWall = 10;
    public float baseGravityMultiplier = 1;
    public float jumpGravityMultiplier = .5f;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<CharacterController>();
        speedMove = 5;
        speedTurn = 180;

    }

    // Update is called once per frame
    void Update()
    {
        float axisV = Input.GetAxis("Vertical");
        float axisH = Input.GetAxis("Horizontal");

        //transform.Rotate(0, axisH * speedTurn * Time.deltaTime, 0);
        Vector3 move = transform.forward * axisV * speedMove;
        if (!didWallJump)
        {
            velocity.x = move.x;
            velocity.z = move.z;
        }
        else
        {
            velocity.x = -move.x;
            velocity.z = move.z;
        }
        
        
        //print(velocity.x);

        float gravityScale = baseGravityMultiplier;
        if (player.isGrounded)
        {
            velocity.y = 0;
            didWallJump = false;
            if (Input.GetButtonDown("Jump"))
            {
                //move.y += 10;
                isJumping = true;
                velocity.y = impulseJump;
                gravityScale = jumpGravityMultiplier;
            }
        }
        else
        {
            if (Input.GetButton("Jump"))
            {
                if (isJumping == true && velocity.y > 0) gravityScale = jumpGravityMultiplier;
            }
            else
            {
                isJumping = false;
            }
        }

        if((player.collisionFlags & CollisionFlags.Sides) != 0)
        {
            //print("Touched a wall");
            if(isOnWall == false) velocity.y = 0;
            gravityScale = 0;
            
            isOnWall = true;
            //didWallJump = false;
        }
        else
        {
            isOnWall = false;

        }

        if (isOnWall)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                isOnWall = false;
                didWallJump = !didWallJump;
                velocity.y = impulseJump;
                //gravityScale = jumpGravityMultiplier;
                if(velocity.x < 0)
                {
                    velocity.x = 0;
                    velocity.x = impulseOffWall;
                }
                else
                {
                    velocity.x = 0;
                    velocity.x = -impulseOffWall;
                }
            }
        }
        
        //player.GetComponent<Collider2D>().Raycast(new Vector2(1, 0), );

        velocity += Physics.gravity * Time.deltaTime * gravityScale;
        player.Move(velocity * Time.deltaTime);
    }
}
