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
    private float jumpForce = 10.0f;

    //Objects and Components
    private SpriteRenderer sprite;
    private BoxCollider bc;
    private Rigidbody rb;
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

    //MoveDirection
    public float moveHorizontal = 0;
    public float moveVertical = 0;
    public bool stopOnCutscene = true;

    void Start()
    {
        OverworldController.Player = gameObject;
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        lastground = bc.transform.position;
        sprite = ((GameObject)Instantiate(spriteObject, bc.transform.position - new Vector3(0,bc.bounds.size.y/2,0), Quaternion.identity)).GetComponent<SpriteRenderer>();
        //sprite.receiveShadows = true;
        //sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        rotSpeed = rotSpeedMagnitude;
        sprite.transform.SetParent(gameObject.transform);
        spriteAnimate = sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if grounded--------------------
        //Check at two different spots to make sure.
        bool isGrounded = false;
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit);
        if ((hit.distance < bc.size.y / 2 + 0.1f) && (new Vector3(hit.normal.x, 0, hit.normal.z).magnitude<0.1))
        {
            isGrounded = true;
        }
        Physics.Raycast(transform.position + new Vector3(-0.25f, 0, 0), -Vector3.up, out hit);
        if ((hit.distance < bc.size.y / 2 + 0.1f) && (new Vector3(hit.normal.x, 0, hit.normal.z).magnitude < 0.1))
        {
            isGrounded = true;
        }
        Physics.Raycast(transform.position + new Vector3(0.25f, 0, 0), -Vector3.up, out hit);
        if ((hit.distance < bc.size.y / 2 + 0.1f) && (new Vector3(hit.normal.x, 0, hit.normal.z).magnitude < 0.1))
        {
            isGrounded = true;
        }
        Physics.Raycast(transform.position + new Vector3(0, 0, 0.15f), -Vector3.up, out hit);
        if ((hit.distance < bc.size.y / 2 + 0.1f) && (new Vector3(hit.normal.x, 0, hit.normal.z).magnitude < 0.1))
        {
            isGrounded = true;
        }
        //JUMP START------------------------------
        if (isGrounded == true)
        {
            jumped = false;
            jump = 0;
            lastground = bc.transform.position;
            spriteAnimate.SetTrigger("Land");
            OverworldController.updateTrackingCameraY(transform.position.y);
        }
        else
        {
            spriteAnimate.SetTrigger("Jump");
            if ((rb.velocity.y < 0))
            {
                jump = jump + (gravity * 1.5f * Time.deltaTime);
            } else
            {
                if (Input.GetButton("Fire1"))
                {
                    jump = jump + (gravity * Time.deltaTime);
                } else
                {
                    jump = jump + (gravity * 2.5f * Time.deltaTime);
                }
            }
        }
        //POSITION RESET IF FALLEN START---------------------------
        /*
        if (bc.transform.position.y < -5)
        {
            jump = 0;
            bc.Move(new Vector3(lastground.x - bc.transform.position.x, lastground.y - bc.transform.position.y, lastground.z - bc.transform.position.z));
        }
        */
        //POSITION RESET IF FALLEN END---------------------------
        if (OverworldController.gameMode == OverworldController.gameModeOptions.Mobile)
        {
            if (Input.GetButton("Fire1") && (jumped == false))
            {
                jump = jumpForce;
                jumped = true;
            }
        }
        if (OverworldController.gameMode != OverworldController.gameModeOptions.Mobile && jump > 0)
        {
            jump = 0;
        }
        //MOVEMENT START---------------------------------------------------------------------------------
        if (OverworldController.gameMode != OverworldController.gameModeOptions.Cutscene)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        else if (stopOnCutscene)
        {
            moveHorizontal = 0;
            moveVertical = 0;
        }
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        if (movement != Vector3.zero && movement.magnitude < speed)
        {
            movement = movement * Time.deltaTime * speed;
        }
        Physics.Raycast(transform.position, movement, out hit);
        if (hit.collider != null){
            if (hit.distance < movement.magnitude + 0.3)
            {
                movement = Vector3.zero;
            }
        }

        movement += new Vector3(0, jump * Time.deltaTime, 0);
        transform.Translate(movement);
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;

        if ((moveVertical != 0) || (moveHorizontal != 0))
        {
            spriteAnimate.SetTrigger("Go");
        }
        else
        {
            spriteAnimate.SetTrigger("Stop");
        }
        //MOVEMENT END---------------------------------------------------------------------------------

        //SetRotationGoals=================================================================================
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
            rotSpeed = rotSpeedMagnitude * Time.deltaTime;
            sprite.transform.Rotate(0, rotSpeed, 0);
            rotated = rotated + rotSpeed;
        }
        if ((goal < rotated))
        {
            rotSpeed = -rotSpeedMagnitude * Time.deltaTime;
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
