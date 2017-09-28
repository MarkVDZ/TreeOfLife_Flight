using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glide : MonoBehaviour {

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


    // Use this for initialization
    void Start () {
        player = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        float axisV = Input.GetAxis("Vertical");
        float axisH = Input.GetAxis("Horizontal");

        transform.Rotate(0, axisH * speedTurn * Time.deltaTime, 0);
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

        if (Input.GetButton("Glide") && isJumping == false)
        {
            if(isRising == true)
            {
                velocity.y = .2f;
                isRising = false;
            }
            print("GLIDE!!!");
            isGliding = true;
            isJumping = false;
            if(isGliding) gravityScale = glideGravityMultiplier;
        }
        else
        {
            if (Input.GetButtonUp("Glide"))
            {
                isGliding = false;

            }
        }
        velocity += Physics.gravity * Time.deltaTime * gravityScale;
        player.Move(velocity * Time.deltaTime);
    }
}
