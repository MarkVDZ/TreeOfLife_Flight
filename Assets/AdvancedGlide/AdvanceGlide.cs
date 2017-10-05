using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceGlide : MonoBehaviour {

    CharacterController player;
    public GameObject wings;

    public float speedMove;
    public float speedTurn;
    Vector3 velocity = Vector3.zero;
    bool isJumping = false;
    bool isGliding = false;
    bool isRising = false;
    bool invertedFlight = false;
    bool canBoost = true;
    bool canBreak = true;
    public float impulseJump = 5;
    public float baseGravityMultiplier = 2;
    public float jumpGravityMultiplier = .5f;
    public float glideGravityMultiplier = .05f;
    public float flyingGravityMultiplier = 0;
    public float terminalVelocity = 20;
    public float boostMeter = 10;
    public float brakesMeter = 20;

    Vector3 airMovement;
    public float forwardAirMovement;
    public float flightAngle;
    public float flightTime = 100;
    public const float FLIGHTCAP = 10;

    Vector3 startRot;


    // Use this for initialization
    void Start()
    {
        player = GetComponent<CharacterController>();
        startRot = transform.localEulerAngles;
        print(startRot);
        
    }

    // Update is called once per frame
    void Update () {
        float axisV = Input.GetAxis("Vertical");
        float axisH = Input.GetAxis("Horizontal");

        //transform.Rotate(0, axisH * speedTurn * Time.deltaTime, 0);
        if(isGliding == false)
        {
            Vector3 move = transform.right * axisV * speedMove;
            velocity.x = move.x;
            velocity.z = move.z;
        }
        
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

        if (Input.GetButtonUp("Deploy") /*&& isGliding == false*/)
        {
            if(isGliding == true)
            {
                isGliding = false;
                transform.localEulerAngles = Vector3.zero;
                wings.SetActive(false);
                return;
            }

            isGliding = true;
            forwardAirMovement = velocity.x;
            flightAngle = 0;
            gravityScale = flyingGravityMultiplier;
            wings.SetActive(true);

            if (forwardAirMovement > 0)
            {
                invertedFlight = false;
            } else
            {
                invertedFlight = true;
            }
            

        }

        if (isGliding)
        {
            gravityScale = flyingGravityMultiplier;
            if(invertedFlight == false)
            {
                GlideMode();
            }
            else if (invertedFlight == true)
            {
                InvertedGlideMode();
            }
            
            /*if (player.isGrounded)
            {
                if (Input.GetButton("Deploy")) isGliding = false; //transform.localEulerAngles = new Vector3(0, 90, 0);

            }
            //GlideMode();
            gravityScale = flyingGravityMultiplier;
            //print("FLIGHT SPEED: " + forwardAirMovement);
            if (Input.GetAxis("FlightVertical") < 0)
            {
                flightAngle += .5f;
                //transform.eulerAngles = new Vector3(transform.rotation.x + flightAngle, transform.rotation.y);//, transform.localRotation.z);
                transform.eulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);


                //print("Tilt down");
            }
            else if(Input.GetAxis("FlightVertical") > 0 && forwardAirMovement > 0)
            {
                flightAngle -= .5f;
                //transform.eulerAngles = new Vector3(transform.rotation.x + flightAngle, transform.localRotation.y);//, transform.localRotation.z);
                transform.eulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);
                //print("Tilt up");
            }

            if(Input.GetAxis("Horizontal") > 0 && canBoost)
            {
                boostMeter -= Time.deltaTime * 4;
                forwardAirMovement += 1 * Time.deltaTime * 2f;
                if (boostMeter < 0) canBoost = false;
                print("Boost: "+boostMeter);
            }
            else if (Input.GetAxis("Horizontal") < 0 && canBreak)
            {
                brakesMeter -= Time.deltaTime * 2;
                forwardAirMovement += 1 * Time.deltaTime * -2f;
                if (brakesMeter < 0) canBreak = false;
                print("Brake: "+brakesMeter);
            }

            if(canBoost == false)
            {
                boostMeter += Time.deltaTime;
                if (boostMeter >= 10) canBoost = true;
            }
            if(canBreak == false)
            {
                brakesMeter += Time.deltaTime;
                if (brakesMeter >= 20) canBreak = true;
            }

            if (forwardAirMovement >= 0)
            {
                if(flightAngle < 0)
                {
                    forwardAirMovement += flightAngle * Time.deltaTime * -.1f;
                    print("Speed up");
                    
                }
                else if(flightAngle > 0)
                {
                    forwardAirMovement += flightAngle * Time.deltaTime * -.02f;
                    print("Slow down");
                }
            }
            else
            {
                forwardAirMovement = 0;
                flightAngle -= 10 * Time.deltaTime;
                //transform.localEulerAngles = new Vector3(transform.localRotation.x + flightAngle, transform.localRotation.y);
                transform.localEulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);
            }
            //print("FLIGHT ANGLE" + flightAngle);
            //print("FLIGHT SPEED: " + forwardAirMovement);
            if (forwardAirMovement >= terminalVelocity) forwardAirMovement = terminalVelocity;
                velocity.x = forwardAirMovement;
            velocity.y = flightAngle * .04f ;
            
            //flightTime -= Time.deltaTime;
            //print(flightTime);
            if(flightTime <= 0)
            {
                transform.localEulerAngles = Vector3.zero;
                isGliding = false;
            }*/
        }// end of glide logic
        

        if (velocity.x >= terminalVelocity) velocity.x = terminalVelocity;

        /*if (flightTime < FLIGHTCAP && isGliding == false)
        {
            flightTime += Time.deltaTime * 2;
        }*/
        //print(flightTime);

        velocity += Physics.gravity * Time.deltaTime * gravityScale;
        player.Move(velocity * Time.deltaTime);
    }

    private void InvertedGlideMode()
    {

        //print("FLIGHT SPEED: " + forwardAirMovement);
        if (Input.GetAxis("Vertical"/*"FlightVertical"*/) > 0 && forwardAirMovement < 0)
        {
            flightAngle += .5f;
            //transform.eulerAngles = new Vector3(transform.rotation.x + flightAngle, transform.rotation.y);//, transform.localRotation.z);
            transform.eulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);
            print("Tilt down");
        }
        else if (Input.GetAxis("Vertical"/*"FlightVertical"*/) < 0)
        {
            flightAngle -= .5f;
            //transform.eulerAngles = new Vector3(transform.rotation.x + flightAngle, transform.localRotation.y);//, transform.localRotation.z);
            transform.eulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);
            print("Tilt up");
        }

        if (Input.GetAxis("Horizontal") < 0 && canBoost)
        {
            boostMeter -= Time.deltaTime * 4;
            forwardAirMovement += 1 * Time.deltaTime * -2f;
            if (boostMeter < 0) canBoost = false;
            print("Boost: " + boostMeter);
        }
        else if (Input.GetAxis("Horizontal") > 0 && canBreak)
        {
            brakesMeter -= Time.deltaTime * 2;
            forwardAirMovement += 1 * Time.deltaTime * 2f;
            if (brakesMeter < 0) canBreak = false;
            print("Brake: " + brakesMeter);
        }

        if (canBoost == false)
        {
            boostMeter += Time.deltaTime;
            if (boostMeter >= 10) canBoost = true;
        }
        if (canBreak == false)
        {
            brakesMeter += Time.deltaTime;
            if (brakesMeter >= 20) canBreak = true;
        }

        if (forwardAirMovement <= 0)
        {
            if (flightAngle > 0)
            {
                forwardAirMovement += flightAngle * Time.deltaTime * -.1f;
                print(forwardAirMovement);
                print("Speed up");

            }
            else if (flightAngle < 0)
            {
                forwardAirMovement += flightAngle * Time.deltaTime * -.02f;
                print(forwardAirMovement);
                print("Slow down");
            }
        }
        else
        {
            /*forwardAirMovement = 0;
            flightAngle -= 10 * Time.deltaTime;
            //transform.localEulerAngles = new Vector3(transform.localRotation.x + flightAngle, transform.localRotation.y);
            transform.localEulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);*/
        }
        //print("FLIGHT ANGLE" + flightAngle);
        //print("FLIGHT SPEED: " + forwardAirMovement);
        if (forwardAirMovement >= terminalVelocity) forwardAirMovement = terminalVelocity;
        velocity.x = forwardAirMovement;
        velocity.y = -flightAngle * .04f;

    }

    void GlideMode()
    {
        
        //print("FLIGHT SPEED: " + forwardAirMovement);
        if (Input.GetAxis("Vertical"/*"FlightVertical"*/) < 0 && forwardAirMovement > 0)
        {
            flightAngle += .5f;
            //transform.eulerAngles = new Vector3(transform.rotation.x + flightAngle, transform.rotation.y);//, transform.localRotation.z);
            transform.eulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);


            //print("Tilt down");
        }
        else if (Input.GetAxis("Vertical"/*"FlightVertical"*/) > 0 )
        {
            flightAngle -= .5f;
            //transform.eulerAngles = new Vector3(transform.rotation.x + flightAngle, transform.localRotation.y);//, transform.localRotation.z);
            transform.eulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);
            //print("Tilt up");
        }

        if (Input.GetAxis("Horizontal") > 0 && canBoost)
        {
            boostMeter -= Time.deltaTime * 4;
            forwardAirMovement += 1 * Time.deltaTime * 2f;
            if (boostMeter < 0) canBoost = false;
            print("Boost: " + boostMeter);
        }
        else if (Input.GetAxis("Horizontal") < 0 && canBreak)
        {
            brakesMeter -= Time.deltaTime * 2;
            forwardAirMovement += 1 * Time.deltaTime * -2f;
            if (brakesMeter < 0) canBreak = false;
            print("Brake: " + brakesMeter);
        }

        if (canBoost == false)
        {
            boostMeter += Time.deltaTime;
            if (boostMeter >= 10) canBoost = true;
        }
        if (canBreak == false)
        {
            brakesMeter += Time.deltaTime;
            if (brakesMeter >= 20) canBreak = true;
        }

        if (forwardAirMovement >= 0)
        {
            if (flightAngle < 0)
            {
                forwardAirMovement += flightAngle * Time.deltaTime * -.1f;
                print("Speed up");

            }
            else if (flightAngle > 0)
            {
                forwardAirMovement += flightAngle * Time.deltaTime * -.02f;
                print("Slow down");
            }
        }
        else
        {
            forwardAirMovement = 0;
            flightAngle -= 10 * Time.deltaTime;
            //transform.localEulerAngles = new Vector3(transform.localRotation.x + flightAngle, transform.localRotation.y);
            transform.localEulerAngles = new Vector3(startRot.x, startRot.y, startRot.z + flightAngle);
        }
        //print("FLIGHT ANGLE" + flightAngle);
        //print("FLIGHT SPEED: " + forwardAirMovement);
        if (forwardAirMovement >= terminalVelocity) forwardAirMovement = terminalVelocity;
        velocity.x = forwardAirMovement;
        velocity.y = flightAngle * .04f;

        //flightTime -= Time.deltaTime;
        //print(flightTime);
        if (flightTime <= 0)
        {
            transform.localEulerAngles = Vector3.zero;
            isGliding = false;
        }
    }
}
