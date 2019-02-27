using System.Collections;
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

    //MoveDirection
    public float moveHorizontal = 0;
    public float moveVertical = 0;
    public bool stopOnCutscene = true;

    void Start()
    {
        OverworldController.Player = gameObject;
        cc = GetComponent<CharacterController>();
        lastground = cc.transform.position;
        sprite = ((GameObject)Instantiate(spriteObject, cc.transform.position - new Vector3(0,cc.height/2,0), Quaternion.identity)).GetComponent<SpriteRenderer>();
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
            lastground = cc.transform.position;
            spriteAnimate.SetTrigger("Land");
        }
        else
        {
            jump = jump + (gravity * Time.deltaTime);
        }
        //POSITION RESET IF FALLEN START---------------------------
        if (cc.transform.position.y < -5)
        {
            print(lastground);
            jump = 0;
            cc.Move(new Vector3(lastground.x - cc.transform.position.x, lastground.y - cc.transform.position.y, lastground.z - cc.transform.position.z));
        }
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
        //Stop Movement
        if ((jump > jumpForce * .9) || (jump < -jumpForce * .9))
        {
            spriteAnimate.SetTrigger("Jump");
        }
        cc.Move(new Vector3(0, jump * Time.deltaTime, 0));
        //JUMP END-----------------------------------------
    }


    // Physics Update
    void FixedUpdate()
    {
        //Stop Movement
        //if (GameController.gameMode == "Mobile")
        //{
        //MOVEMENT START---------------------------------------------------------------------------------
        if (OverworldController.gameMode != OverworldController.gameModeOptions.Cutscene)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        } else if (stopOnCutscene)
        {
            moveHorizontal = 0;
            moveVertical = 0;
        }
            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            movement = speed * movement / movement.magnitude;
            cc.Move(movement);
            if ((moveVertical != 0) || (moveHorizontal != 0))
            {
                spriteAnimate.SetTrigger("Go");
            }
            else
            {
                spriteAnimate.SetTrigger("Stop");
            }
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

    public void jumpCall()
    {
        jump = jumpForce;
    }
}
