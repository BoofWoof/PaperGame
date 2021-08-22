using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class CharacterMovementOverworld : MonoBehaviour
{
    GameControls controls;

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
    private SpriteFlipper spriteFlipper;

    //Stage Fall and Jump Variables
    private float prevY;
    private Vector3 lastground;
    private bool jumped = false;

    //AnimationInfo
    private Animator spriteAnimate;

    //MoveDirection
    public float moveHorizontal = 0;
    public float moveVertical = 0;
    public bool stopOnCutscene = true;

    //HitscanVariables
    private float height;
    private float width;
    private float length;
    public LayerMask ignoreLayer;
    public float maxStepSize = 0.1f;
    private int scanWidthCount = 3;
    private int scanLengthCount = 3;
    private int scanHeightCount = 3;
    private float scanWidthSize;
    private float scanLengthSize;
    private float scanHeightSize;

    void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

    void Start()
    {
        prevY = transform.position.y;
        spriteFlipper = GetComponent<SpriteFlipper>();
        OverworldController.Player = gameObject;
        bc = GetComponent<BoxCollider>();
        spriteAnimate = sprite.GetComponent<Animator>();

        width = bc.size.x;
        height = bc.size.y - maxStepSize;
        length = bc.size.z;
        scanWidthSize = width/ (float)(scanWidthCount-1);
        scanLengthSize = length/ (float)(scanLengthCount-1);
        scanHeightSize = height/ (float)(scanHeightCount-1);

        groundPlayer();
        lastground = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool movingDown = false;
        float yDelta = transform.position.y - prevY;
        if (yDelta < 0) movingDown = true;
        prevY = transform.position.y;
        if(GameDataTracker.gameMode != GameDataTracker.gameModeOptions.Cutscene)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
        }
        //Check if grounded--------------------
        //Check at two different spots to make sure.
        bool hitGround = false;
        RaycastHit hit;
        RaycastHit besthit;
        float closestCast = 10000.0f;
        //GroundScan
        for (int i = 0; i < scanWidthCount; i++)
        {
            for (int j = 0; j < scanLengthCount; j++)
            {
                Vector3 rayOrgin = transform.position + new Vector3(-width / 2f + i * scanWidthSize, -height/2f + maxStepSize - 0.048f, -length / 2f + j * scanLengthSize);
                if(Physics.Raycast(rayOrgin, -Vector3.up, out hit, 2f, ~ignoreLayer))
                {
                    Vector3 down = transform.TransformDirection(Vector3.down) * 2;
                    Debug.DrawRay(rayOrgin, down, Color.green);
                    int layer = hit.collider.gameObject.layer;
                    if (layer == 6)
                    {
                        if (checkAppearTorches(hit.point)) continue;
                    }
                    //7 is the disappear layer.
                    if (layer == 7)
                    {
                        if (checkDisapearTorches(hit.point)) continue;
                    }
                    if ((closestCast > hit.distance) && (hit.distance > 0))
                    {
                        closestCast = hit.distance;
                        besthit = hit;
                    }
                }
            }
        }
        if (closestCast <= 0.2f + Mathf.Abs(yDelta))
        {
            hitGround = true;
        }
        //UPDATE FALL------------------------------
        if (hitGround == true)
        {
            if (movingDown)
            {
                groundPlayerRaycast(closestCast);
                jumped = false;
                jump = 0;
                lastground = transform.position;
                spriteAnimate.SetTrigger("Land");
                OverworldController.updateTrackingCameraY(transform.position.y);
            }
        }
        else
        {
            spriteAnimate.SetTrigger("Jump");
            if (controls.OverworldControls.MainAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
            {
                jump = jump + (gravity * Time.deltaTime);
            } else
            {
                jump = jump + (gravity * 2.5f * Time.deltaTime);
            }
        }
        //JUMP---------------------------------
        if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile)
        {
            if (jumped == false)
            {
                if (controls.OverworldControls.MainAction.triggered){
                    jump = jumpForce;
                    jumped = true;
                }
            }
        }
        //MOVEMENT START---------------------------------------------------------------------------------
        if (GameDataTracker.gameMode != GameDataTracker.gameModeOptions.Cutscene)
        {
            Vector2 thumbstick_values = controls.OverworldControls.Movement.ReadValue<Vector2>();
            moveHorizontal = thumbstick_values[0];
            moveVertical = thumbstick_values[1];
        }
        else
        {
            moveHorizontal = 0;
            moveVertical = 0;
        }
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = movement * Time.deltaTime * speed;

        closestCast = 10000.0f;
        for (int i = 0; i < scanWidthCount; i++)
        {
            for (int j = 0; j < scanHeightCount; j++)
            {
                Vector3 rayOrgin = transform.position + new Vector3(-width / 2f + i * scanWidthSize, -height / 2f + maxStepSize - 0.048f + j * scanHeightSize, 0);
                if(Physics.Raycast(rayOrgin, new Vector3(0, 0, movement.z), out hit, 2f, ~ignoreLayer))
                {
                    Debug.DrawRay(rayOrgin, new Vector3(0, 0, movement.z) * 10f, Color.red);
                    int layer = hit.collider.gameObject.layer;
                    //8 is the appear layer.
                    if (layer == 6){
                        if (checkAppearTorches(hit.point)) continue;
                    }
                    //7 is the disappear layer.
                    if (layer == 7) {
                        if (checkDisapearTorches(hit.point)) continue;
                    } 
                    if ((closestCast > hit.distance) && (hit.distance > 0))
                    {
                        closestCast = hit.distance;
                    }
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
                Vector3 rayOrgin = transform.position + new Vector3(0, -height / 2f + maxStepSize - 0.048f + j * scanHeightSize, -length / 2f + k * scanLengthSize);
                if(Physics.Raycast(rayOrgin, new Vector3(movement.x, 0, 0), out hit, 2f, ~ignoreLayer))
                {
                    Debug.DrawRay(rayOrgin, new Vector3(movement.x, 0, 0) * 10f, Color.blue);
                    int layer = hit.collider.gameObject.layer;
                    //8 is the appear layer.
                    if (layer == 6)
                    {
                        if (checkAppearTorches(hit.point)) continue;
                    }
                    //7 is the disappear layer.
                    if (layer == 7)
                    {
                        if (checkDisapearTorches(hit.point)) continue;
                    }
                    if ((closestCast > hit.distance) && (hit.distance > 0))
                    {
                        closestCast = hit.distance;
                    }
                }
            }
        }

        if (closestCast < 0.5){
            movement.x = 0;
        }

        movement += new Vector3(0, jump * Time.deltaTime, 0);
        transform.Translate(movement);
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
            spriteFlipper.setFacingRight();
        }
        if ((moveHorizontal < 0))
        {
            spriteFlipper.setFacingLeft();
        }
        /*
        */
    }

    public bool checkDisapearTorches(Vector3 position)
    {
        foreach (MorganTorchScript torchScript in OverworldController.AllMorganTorches)
        {
            if (Vector3.Distance(torchScript.transform.position, position) < torchScript.LightRangeCurrent - 0.2f)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkAppearTorches(Vector3 position)
    {
        foreach (MorganTorchScript torchScript in OverworldController.AllMorganTorches)
        {
            if (Vector3.Distance(torchScript.transform.position, position) < torchScript.LightRangeCurrent - 0.2f)
            {
                return false;
            }
        }
        return true;
    }

    public void groundPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            transform.position = hit.point + new Vector3(0, height/2 + 0.05f, 0);
        }
    }
    public void groundPlayerRaycast(float closestCast)
    {
        transform.position += new Vector3(0, maxStepSize - closestCast, 0);
    }
}
