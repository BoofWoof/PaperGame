using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToTrack;
    public Vector3 tolerance = new Vector3(0, 0, 0);
    public Vector3 offset = new Vector3(0, 0, 0);
    public float dialogueOffsetMultiplier = 0.5f;
    public float speed = 0.1f;

    public bool OverworldCamera = true;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 objectPosition = ObjectToTrack.transform.position;
        Vector3 cameraPosition = cameraTransform.transform.position;
        Vector3 cameraGoal = cameraTransform.transform.position;
        //CAMERA GOAL DIALOGUE START-----------------------------------------------
        if ((OverworldController.gameMode != OverworldController.gameModeOptions.Mobile)&&(OverworldCamera))
        {
            cameraGoal = new Vector3(objectPosition.x + dialogueOffsetMultiplier*offset.x, objectPosition.y + 0.5f + dialogueOffsetMultiplier * offset.y, objectPosition.z + dialogueOffsetMultiplier * offset.z);
            //CAMERA MOVE START--------------------------------------
            //Vector3 xdif = new Vector3(cameraGoal.x - cameraPosition.x, cameraGoal.y - cameraPosition.y, cameraGoal.z - cameraPosition.z);
            //cameraTransform.transform.position = cameraPosition + (xdif * speed) * Time.deltaTime;
            //CAMERA MOVE END--------------------------------------
        }
        //CAMERA GOAL DIALOGUE END-----------------------------------------------

        //CAMERA GOAL MOBILE START-----------------------------------------------
        if ((OverworldController.gameMode == OverworldController.gameModeOptions.Mobile) ||(OverworldCamera==false))
        {        
            // X POSITION FIXING
            if ((cameraPosition.x - tolerance.x) > (objectPosition.x + offset.x))
            {
                cameraGoal = new Vector3(objectPosition.x + tolerance.x + offset.x, cameraGoal.y, cameraGoal.z);
            }
            if ((cameraPosition.x + tolerance.x) < (objectPosition.x + offset.x))
            {
                cameraGoal = new Vector3(objectPosition.x - tolerance.x + offset.x, cameraGoal.y, cameraGoal.z);
            }
            // Z POSITION FIX
            if ((cameraPosition.z - tolerance.z) > (objectPosition.z + offset.z))
            {
                cameraGoal = new Vector3(cameraGoal.x, cameraGoal.y, objectPosition.z + tolerance.z + offset.z);
            }
            if ((cameraPosition.z + tolerance.z) < (objectPosition.z + offset.z))
            {
                cameraGoal = new Vector3(cameraGoal.x, cameraGoal.y, objectPosition.z - tolerance.z + offset.z);
            }
            // Y POSITION FIX
            if ((cameraPosition.y - tolerance.y) > (objectPosition.y + offset.y))
            {
                cameraGoal = new Vector3(cameraGoal.x, objectPosition.y + tolerance.y + offset.y, cameraGoal.z);
            }
            if ((cameraPosition.y + tolerance.y) < (objectPosition.y + offset.y))
            {
                cameraGoal = new Vector3(cameraGoal.x, objectPosition.y - tolerance.y + offset.y, cameraGoal.z);
            }
            //CAMERA MOVE START--------------------------------------
            //cameraTransform.transform.position = cameraGoal;
            //CAMERA MOVE END--------------------------------------
        }
        //CAMERA GOAL MOBILE END-----------------------------------------------
        Vector3 xdif = new Vector3(cameraGoal.x - cameraPosition.x, cameraGoal.y - cameraPosition.y, cameraGoal.z - cameraPosition.z);
        if (xdif.magnitude != 0)
        {
            //cameraTransform.transform.position = 
            cameraTransform.transform.position = cameraPosition + (xdif * speed * Time.deltaTime) / xdif.magnitude;
        }

        if (xdif.magnitude < 0.1)
        {
            cameraTransform.transform.position = cameraGoal;
        }
    }
}
