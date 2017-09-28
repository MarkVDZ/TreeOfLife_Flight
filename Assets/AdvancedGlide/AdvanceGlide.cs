using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceGlide : MonoBehaviour {

    CharacterController player;

    public float speedMove;
    public float speedTurn;
    Vector3 velocity = Vector3.zero;
    bool isJumping = false;
    bool isGliding = false;
    bool isRising = false;
    public float impulseJump = 5;
    public float baseGravityMultiplier = 2;
    public float jumpGravityMultiplier = .5f;
    public float glideGravityMultiplier = .05f;
    public float flyingGravityMultiplier = 0;
    public float terminalVelocity = 20;

    Vector3 airMovement;
    public float forwardAirMovement;
    public float flightAngle;
    public float flightTime = 100;
    public const float FLIGHTCAP = 10;


    // Use this for initialization
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
        float axisV = Input.GetAxis("Vertical");
        float axisH = Input.GetAxis("Horizontal");

        //transform.Rotate(0, axisH * speedTurn * Time.deltaTime, 0);
        Vector3 move = transform.forward * axisV * speedMove;
        velocity.x = move.x;
        velocity.z = move.z;

        float gravityScale = baseGravityMultiplier;
        if (player.isGrounded)
        {
            velocity.y = 0;
            if (Input.GetButtonDown("Jump") && isGliding == false)
            {
                //move.y += 10;
                isJumping = true;
                isRising = true;
                velocity.y = impulseJump;
                gravityScale = jumpGravityMultiplier;
                //print();
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

        /* if (Input.GetButton("Glide") && isJumping == false)
         {
             if (isRising == true)
             {
                 velocity.y = .2f;
                 isRising = false;
             }
             print("GLIDE!!!");
             isGliding = true;
             isJumping = false;
             if (isGliding) gravityScale = glideGravityMultiplier;
         }
         else
         {
             if (Input.GetButtonUp("Glide"))
             {
                 isGliding = false;

             }
         }*/

        if (Input.GetButton("Deploy") && isGliding == false)
        {
            isGliding = true;
            
            forwardAirMovement = velocity.z;

            flightAngle = 0;
            
            gravityScale = flyingGravityMultiplier;
        }

        if (isGliding)
        {
            if (player.isGrounded)
            {
                if (Input.GetButton("Deploy")) isGliding = false; transform.localEulerAngles = Vector3.zero;

            }
            //GlideMode();
            gravityScale = flyingGravityMultiplier;
            //print("FLIGHT SPEED: " + forwardAirMovement);
            if (Input.GetAxis("FlightVertical") > 0)
            {
                flightAngle += .5f;
                transform.localEulerAngles = new Vector3(transform.localRotation.x + flightAngle, transform.localRotation.y);
                
                
               //print("Tilt down");
            }
            else if(Input.GetAxis("FlightVertical") < 0 && forwardAirMovement > 0)
            {
                flightAngle -= .5f;
                transform.localEulerAngles = new Vector3(transform.localRotation.x + flightAngle, transform.localRotation.y);
                //print("Tilt up");
            }

            if(forwardAirMovement >= 0)
            {
                if(flightAngle > 0)
                {
                    forwardAirMovement += flightAngle * Time.deltaTime * .1f;
                    //print("Speed up");
                    
                }
                else if(flightAngle < 0)
                {
                    forwardAirMovement += flightAngle * Time.deltaTime * .02f;
                    //print("Slow down");
                }
            }
            else
            {
                forwardAirMovement = 0;
                flightAngle += 10 * Time.deltaTime;
                transform.localEulerAngles = new Vector3(transform.localRotation.x + flightAngle, transform.localRotation.y);
            }
            //print("FLIGHT ANGLE" + flightAngle);
            //print("FLIGHT SPEED: " + forwardAirMovement);
            if (forwardAirMovement >= terminalVelocity) forwardAirMovement = terminalVelocity;
            velocity.z = forwardAirMovement;
            velocity.y = -flightAngle * .04f ;

            flightTime -= Time.deltaTime;
            //print(flightTime);
            if(flightTime <= 0)
            {
                transform.localEulerAngles = Vector3.zero;
                isGliding = false;
            }
        }

        if (velocity.z >= terminalVelocity) velocity.z = terminalVelocity;

        if (flightTime < FLIGHTCAP && isGliding == false)
        {
            flightTime += Time.deltaTime * 2;
        }
        //print(flightTime);
        velocity += Physics.gravity * Time.deltaTime * gravityScale;
        player.Move(velocity * Time.deltaTime);
    }

    void GlideMode()
    {

    }
}
