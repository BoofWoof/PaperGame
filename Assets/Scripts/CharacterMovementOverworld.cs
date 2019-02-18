﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterMovementOverworld : MonoBehaviour
{
    //Character rates.
    public float speed = 0.1f;
    public float gravity = -20.0f;
    private float jump = 0.0f;
    private float jumpForce = 8.0f;

    //Objects and Components
    private SpriteRenderer sprite;
    private CharacterController cc;
    public GameObject spriteObject;

    //Rotation variables
    private float rotated = 0.0f;
    private float prev_rotated = 0.0f;
    public float rotSpeedMagnitude = 20;
    private float rotSpeed;
    private float goal = 0.0f;

    //Stage Fall and Jump Variables
    private Vector3 lastground;
    private bool jumped = false;

    //AnimationInfo
    private Animator spriteAnimate;
    private string directionAnim = "left";

    void Start()
    {
        cc = GetComponent<CharacterController>();
        lastground = cc.transform.position;
        sprite = ((GameObject)Instantiate(spriteObject, cc.transform.position, Quaternion.identity)).GetComponent<SpriteRenderer>();
        sprite.receiveShadows = true;
        sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        rotSpeed = rotSpeedMagnitude;
        sprite.transform.SetParent(this.transform);
        spriteAnimate = sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //JUMP START------------------------------
        if (cc.isGrounded == true)
        {
            jump = 0;
            jumped = false;
        }
        else
        {
            jump = jump + (gravity * Time.deltaTime);
        }
        //Stop Movement
        if (GameController.gameMode == "Mobile")
        {
            if (Input.GetButton("Fire1") && (jumped == false))
            {
                jump = jumpForce;
                jumped = true;
            }
        }
        if ((GameController.gameMode == "Dialogue")&&(jump>0))
        {
            jump = 0;
        }
        cc.Move(new Vector3(0, jump * Time.deltaTime, 0));
        //JUMP END-----------------------------------------

        //SPRITE FRAME UPDATES START----------------------------
        //sprite.transform.position = cc.transform.position;
        //SPRITE FRAME UPDATES END----------------------------

        //POSITION RESET IF FALLEN START---------------------------
        if (cc.isGrounded == true)
        {
            lastground = cc.transform.position;
        }
        if (cc.transform.position.y < -5)
        {
            cc.transform.position = lastground;
        }
        //POSITION RESET IF FALLEN END---------------------------

        //ARE THEY MOVING
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if ((moveVertical != 0) || (moveHorizontal != 0))
        {
            spriteAnimate.SetTrigger("Go");
        } else
        {
            spriteAnimate.SetTrigger("Stop");
        }
    }


    // Physics Update
    void FixedUpdate()
    {
        //Stop Movement
        //if (GameController.gameMode == "Mobile")
        //{
            //MOVEMENT START---------------------------------------------------------------------------------
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * speed, 0, moveVertical * speed);
            cc.Move(movement);
        //MOVEMENT END---------------------------------------------------------------------------------

        
        //SetRotationGoals
        if ((moveHorizontal > 0))
        {
            goal = 180;
        }
        if ((moveHorizontal < 0))
        {
            goal = 0;
        }

        //SPRITE ROTATION START-------------------------------------------------------------------------
        if ((goal > rotated))
        {
            rotSpeed = rotSpeedMagnitude;
            sprite.transform.Rotate(0, rotSpeed, 0);
            rotated = rotated + rotSpeed;
        }
        if ((goal < rotated))
        {
            rotSpeed = -rotSpeedMagnitude;
            sprite.transform.Rotate(0, rotSpeed, 0);
            rotated = rotated + rotSpeed;
        }
        if ((rotated <= 90) && (prev_rotated > 90))
        {
            sprite.transform.Rotate(0, -180, 0);
        }
        if ((rotated >= 90) && (prev_rotated < 90))
        {
            sprite.transform.Rotate(0, 180, 0);
        }
        if (rotated < 90)
        {
            spriteAnimate.SetTrigger("Left");
        }
        if (rotated > 90)
        {
            spriteAnimate.SetTrigger("Right");
        }
        prev_rotated = rotated;
        //SPRITE ROTATION END------------------------------------------------------------------
        
    }
}
