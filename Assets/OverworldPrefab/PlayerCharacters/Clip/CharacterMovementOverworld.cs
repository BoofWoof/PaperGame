using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class CharacterMovementOverworld : MonoBehaviour
{
    //Character info
    public float height;
    public float width;
    public float length;

    //Character rates.
    public float speed = 3f;
    public float gravity = -30.0f;
    private float jump = 0.0f;
    public float jumpForce = 9.7f;
    public float maxJumpHeight = 1.4f;

    //Objects and Components
    private BoxCollider bc;
    private Rigidbody rb;
    public GameObject sprite;

    //Rotation variables
    private float rotated = 0.0f;
    private float prev_rotated = 0.0f;
    public float rotSpeedMagnitude = 20;
    private float rotSpeed;
    public float goal = 0.0f;

    //Stage Fall and Jump Variables
    private Vector3 lastground;
    private bool jumped = false;

    //AnimationInfo
    private Animator spriteAnimate;

    //MoveDirection
    public float moveHorizontal = 0;
    public float moveVertical = 0;
    public bool stopOnCutscene = true;

    //HitscanVariables
    private int scanWidthCount = 3;
    private int scanLengthCount = 3;
    private int scanHeightCount = 3;
    private float scanWidthSize;
    private float scanLengthSize;
    private float scanHeightSize;

    void Start()
    {
        OverworldController.Player = gameObject;
        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        rotSpeed = rotSpeedMagnitude;
        spriteAnimate = sprite.GetComponent<Animator>();

        width = bc.size.x - 0.05f;
        height = bc.size.y;
        length = bc.size.z - 0.05f;
        scanWidthSize = width/(scanWidthCount-1);
        scanLengthSize = length/(scanLengthCount-1);
        scanHeightSize = height/(scanHeightCount-1);

        groundPlayer();
        lastground = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameDataTracker.gameMode != GameDataTracker.gameModeOptions.Cutscene)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
        }
        //Check if grounded--------------------
        //Check at two different spots to make sure.
        bool isGrounded = false;
        RaycastHit hit;
        RaycastHit besthit;
        float closestCast = 10000.0f;
        float normalCast = 360.0f;
        Physics.Raycast(transform.position + new Vector3(-width / 2, -height / 2 + 0.1f, -length / 2), -Vector3.up, out besthit);
        for (int i = 0; i < scanWidthCount; i++)
        {
            for(int j = 0; j < scanLengthCount; j++)
            {
                Physics.Raycast(transform.position + new Vector3(-width/2 + i*scanWidthSize, -height/2 + 0.1f, -length/2 + j*scanLengthSize), -Vector3.up, out hit);
                normalCast = new Vector3(hit.normal.x, 0, hit.normal.z).magnitude;
                if ((closestCast > hit.distance) && (hit.distance > 0) && (normalCast < 0.1))
                {
                    closestCast = hit.distance;
                    besthit = hit;
                }
            }
        }
        if (closestCast <= 0.16f)
        {
            isGrounded = true;
        }

        //JUMP START------------------------------
        if (isGrounded == true)
        {
            groundPlayerRaycast(besthit);
            jumped = false;
            jump = 0;
            lastground = transform.position;
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
        if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile)
        {
            if (Input.GetButtonDown("Fire1") && (jumped == false))
            {
                jump = jumpForce;
                jumped = true;
            }
        }
        if ((GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Cutscene || GameDataTracker.gameMode == GameDataTracker.gameModeOptions.MobileCutscene) && jump > 0)
        {
            groundPlayer();
            isGrounded = true;
            jump = 0;
        }
        //MOVEMENT START---------------------------------------------------------------------------------
        if (GameDataTracker.gameMode != GameDataTracker.gameModeOptions.Cutscene)
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

        closestCast = 10000.0f;
        for (int i = 0; i < scanWidthCount; i++)
        {
            for (int j = 0; j < scanHeightCount; j++)
            {
                Physics.Raycast(transform.position + new Vector3(-width / 2 + i * scanWidthSize, -(height - 0.05f) / 2 + j * scanHeightSize, 0), new Vector3(0, 0, movement.z), out hit);
                normalCast = new Vector3(hit.normal.x, 0, hit.normal.z).magnitude;
                if ((closestCast > hit.distance) && (hit.distance > 0) && (hit.transform.tag == "Environment"))
                {
                    closestCast = hit.distance;
                }
            }
        }

        if (closestCast < 0.3)
        {
            movement.z = 0;
        }
        closestCast = 10000.0f;
        for (int j = 0; j < scanHeightCount; j++)
        {
            for (int k = 0; k < scanLengthCount; k++)
            {
                Physics.Raycast(transform.position + new Vector3(0, -(height - 0.05f) / 2 + j * scanHeightSize, -length/2 + k*scanLengthSize), new Vector3(movement.x, 0, 0), out hit);
                normalCast = new Vector3(hit.normal.x, 0, hit.normal.z).magnitude;
                if ((closestCast > hit.distance) && (hit.distance > 0) && (hit.transform.tag == "Environment"))
                {
                    closestCast = hit.distance;
                }
            }
        }

        if (closestCast < 0.5){
            movement.x = 0;
        }

        movement += new Vector3(0, jump * Time.deltaTime, 0);
        transform.Translate(movement);
        if(transform.position.y > (lastground.y + maxJumpHeight))
        {
            transform.position = new Vector3(transform.position.x, lastground.y + maxJumpHeight, transform.position.z);
        }
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
        Vector3 currentScale = sprite.transform.localScale;
        if (rotated < 90)
        {
            sprite.transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        if (rotated > 90)
        {
            sprite.transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        prev_rotated = rotated;
        //SPRITE ROTATION END------------------------------------------------------------------
    }
    public void groundPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            transform.position = hit.point + new Vector3(0, height/2 + 0.05f, 0);
        }
    }
    public void groundPlayerRaycast(RaycastHit hit)
    {
        transform.position =  new Vector3(transform.position.x, hit.point.y + height / 2 + 0.05f, transform.position.z);
    }
}
