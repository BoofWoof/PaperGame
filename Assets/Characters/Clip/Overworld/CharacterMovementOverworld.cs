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
    public Animator spriteAnimate;
    private bool hittingWithStick = false;

    //MoveDirection
    public float moveHorizontal = 0;
    public float moveVertical = 0;
    public bool movementLock = false;

    //HitscanVariables
    private Vector2 hitDirection;
    public float explosionHitRadius = 2.0f;
    private Vector2 lastNonZeroDirection;
    private float full_height;
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

    public bool DebugDisable = false;

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
        lastNonZeroDirection = new Vector2(-1, 0);
        prevY = transform.position.y;
        spriteFlipper = GetComponent<SpriteFlipper>();
        OverworldController.Player = gameObject;
        bc = GetComponent<BoxCollider>();

        width = bc.bounds.size.x;
        full_height = bc.bounds.size.y;
        height = bc.bounds.size.y - maxStepSize;
        length = bc.bounds.size.z;
        scanWidthSize = width/ (float)(scanWidthCount-1f);
        scanLengthSize = length/ (float)(scanLengthCount-1f);
        scanHeightSize = height/ (float)(scanHeightCount-1f);

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
        GameObject bestObject = null;
        Vector3 hitPoint = Vector3.zero;
        float bestHitHeight = 0;
        float closestCast = 10000.0f;

        float next_jump;
        //Update jump and check if you will collide.
        if (controls.OverworldControls.MainAction.phase == UnityEngine.InputSystem.InputActionPhase.Performed)
        {
            next_jump = jump + (gravity * Time.deltaTime);
        }
        else
        {
            next_jump = jump + (gravity * 2.5f * Time.deltaTime);
        }
        float maxPossibleVerticleDistance = 0.05f + maxStepSize + Time.deltaTime * Mathf.Abs(next_jump);

        //Rotation
        Quaternion rayRotation = Quaternion.AngleAxis(GameDataTracker.CameraHeading, Vector3.up);
        //GroundScan
        for (int i = 0; i < scanWidthCount; i++)
        {
            for (int j = 0; j < scanLengthCount; j++)
            {
                Vector3 rayOrgin = transform.position + rayRotation * new Vector3(-width / 2f + i * scanWidthSize, -full_height / 2f + maxStepSize, -length / 2f + j * scanLengthSize);
                if(Physics.Raycast(rayOrgin, Vector3.down, out hit, maxPossibleVerticleDistance, ~ignoreLayer))
                {
                    Debug.DrawRay(rayOrgin, Vector3.down * maxPossibleVerticleDistance, Color.green);
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
                        bestHitHeight = hit.point.y;
                        bestObject = hit.collider.gameObject;
                        hitPoint = hit.point;
                    }
                }
            }
        }
        Debug.DrawRay(hitPoint, new Vector3(0, width/2f, 0), Color.red);
        if (closestCast <= maxPossibleVerticleDistance)
        {
            hitGround = true;
        }
        //UPDATE FALL------------------------------
        if (hitGround == true)
        {
            spriteAnimate.SetTrigger("Land");
            if (movingDown && transform.parent == null)
            {
                groundPlayerRaycast(bestHitHeight);
                jumped = false;
                jump = 0;
                lastground = transform.position;
                if (bestObject.tag == "MovingObject") transform.parent = bestObject.transform;
            }
            if (controls.OverworldControls.SecondaryAction.triggered)
            {
                hitDirection = controls.OverworldControls.Movement.ReadValue<Vector2>();
                spriteAnimate.SetTrigger("StickHit");
                StartCoroutine(HitWithStick(hitDirection));
            }
            if (jump == 0)
            {
                if (closestCast < maxStepSize && transform.parent == null)
                {
                    groundPlayerRaycast(bestHitHeight);
                }
                OverworldController.updateTrackingCameraY(transform.position.y);
            } 
        }
        else
        {
            jump = next_jump;
            transform.parent = null;
            spriteAnimate.SetTrigger("Jump");
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
        if (!movementLock)
        {
            if (GameDataTracker.gameMode != GameDataTracker.gameModeOptions.Cutscene && GameDataTracker.gameMode != GameDataTracker.gameModeOptions.AbilityFreeze)
            {
                Vector2 stickPosition = controls.OverworldControls.Movement.ReadValue<Vector2>();
                moveHorizontal = stickPosition[0];
                moveVertical = stickPosition[1];
            }
            else
            {
                moveHorizontal = 0;
                moveVertical = 0;
            }
        }
        if(moveHorizontal != 0 || moveVertical != 0) lastNonZeroDirection = new Vector2(moveHorizontal, moveVertical);
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = movement * Time.deltaTime * speed;

        closestCast = 10000.0f;
        for (int i = 0; i < scanWidthCount; i++)
        {
            for (int j = 0; j < scanHeightCount; j++)
            {
                Vector3 rayOrgin = transform.position + rayRotation * new Vector3(-width / 2f + i * scanWidthSize, -full_height / 2f + maxStepSize + j * scanHeightSize, 0);
                if(Physics.Raycast(rayOrgin, rayRotation * new Vector3(0, 0, movement.z), out hit, 2f, ~ignoreLayer))
                {
                    Debug.DrawRay(rayOrgin, rayRotation * new Vector3(0, 0, movement.z) * 10f, Color.red);
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
                Vector3 rayOrgin = transform.position + rayRotation * new Vector3(0, -full_height / 2f + maxStepSize  + j * scanHeightSize, -length / 2f + k * scanLengthSize);
                if(Physics.Raycast(rayOrgin, rayRotation * new Vector3(movement.x, 0, 0), out hit, 2f, ~ignoreLayer))
                {
                    Debug.DrawRay(rayOrgin, rayRotation * new Vector3(movement.x, 0, 0) * 10f, Color.red);
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
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 3f, ~ignoreLayer))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + full_height / 2f, transform.position.z);
            prevY = transform.position.y;
        }
    }
    public void groundPlayerRaycast(float bestHitHeight)
    {
        transform.position = new Vector3(transform.position.x, bestHitHeight + full_height / 2f, transform.position.z);
        prevY = transform.position.y;
    }

    IEnumerator HitWithStick(Vector2 hitDirection)
    {
        if(GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile)
        {
            GameDataTracker.gameModePre = GameDataTracker.gameMode;
            GameDataTracker.gameMode = GameDataTracker.gameModeOptions.AbilityFreeze;
            yield return new WaitForSeconds(0.25f);
            if (hitDirection == Vector2.zero) hitDirection = lastNonZeroDirection;
            if (spriteAnimate.GetCurrentAnimatorStateInfo(0).IsName("ClipSmack"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, new Vector3(hitDirection.x, 0, hitDirection.y), out hit, 0.75f, ~ignoreLayer))
                {
                    DestructionScript dScript = hit.collider.gameObject.GetComponent<DestructionScript>();
                    if (dScript != null)
                    {
                        bool validHit = true;
                        int layer = hit.collider.gameObject.layer;
                        //8 is the appear layer.
                        if (layer == 6)
                        {
                            if (checkAppearTorches(hit.point)) validHit = false;
                        }
                        //7 is the disappear layer.
                        if (layer == 7)
                        {
                            if (checkDisapearTorches(hit.point)) validHit = false;
                        }
                        if (validHit)
                        {
                            dScript.BreakObject();
                        }
                    }
                }
            }
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionHitRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) rb.AddExplosionForce(100.0f, transform.position, explosionHitRadius, 0.5f);
            }
            yield return new WaitForSeconds(0.25f);
            if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.AbilityFreeze) GameDataTracker.gameMode = GameDataTracker.gameModePre;
        }
    }
}
