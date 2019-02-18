using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToTrack;
    public float xtolerance = 0.0f;
    public float ytolerance = 0.0f;
    public float ztolerance = 0.0f;
    public float xoffset = 0.0f;
    public float yoffset = 0.0f;
    public float zoffset = 0.0f;
    public float dialogueOffsetMultiplier = 0.5f;
    public float speed = 0.1f;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 objectPosition = ObjectToTrack.transform.position;
        Vector3 cameraPosition = cameraTransform.transform.position;
        Vector3 cameraGoal = cameraTransform.transform.position;
        //CAMERA GOAL DIALOGUE START-----------------------------------------------
        if (GameController.gameMode == "Dialogue")
        {
            cameraGoal = new Vector3(objectPosition.x + dialogueOffsetMultiplier*xoffset, objectPosition.y + 0.5f + dialogueOffsetMultiplier * yoffset, objectPosition.z + dialogueOffsetMultiplier * zoffset);
        }
        //CAMERA GOAL DIALOGUE END-----------------------------------------------

        //CAMERA GOAL MOBILE START-----------------------------------------------
        if (GameController.gameMode == "Mobile")
        {        
            // X POSITION FIXING
            if ((cameraPosition.x - xtolerance) > (objectPosition.x + xoffset))
            {
                cameraGoal = new Vector3(objectPosition.x + xtolerance + xoffset, cameraGoal.y, cameraGoal.z);
            }
            if ((cameraPosition.x + xtolerance) < (objectPosition.x + xoffset))
            {
                cameraGoal = new Vector3(objectPosition.x - xtolerance + xoffset, cameraGoal.y, cameraGoal.z);
            }
            // Z POSITION FIX
            if ((cameraPosition.z - ztolerance) > (objectPosition.z + zoffset))
            {
                cameraGoal = new Vector3(cameraGoal.x, cameraGoal.y, objectPosition.z + ztolerance + zoffset);
            }
            if ((cameraPosition.z + ztolerance) < (objectPosition.z + zoffset))
            {
                cameraGoal = new Vector3(cameraGoal.x, cameraGoal.y, objectPosition.z - ztolerance + zoffset);
            }
            // Y POSITION FIX
            if ((cameraPosition.y - ytolerance) > (objectPosition.y + yoffset))
            {
                cameraGoal = new Vector3(cameraGoal.x, objectPosition.y + ytolerance + yoffset, cameraGoal.z);
            }
            if ((cameraPosition.y + ytolerance) < (objectPosition.y + yoffset))
            {
                cameraGoal = new Vector3(cameraGoal.x, objectPosition.y - ytolerance + yoffset, cameraGoal.z);
            }
        }
        //CAMERA GOAL MOBILE END-----------------------------------------------

        //CAMERA MOVE START--------------------------------------
        Vector3 xdif = new Vector3(cameraGoal.x - cameraPosition.x, cameraGoal.y - cameraPosition.y, cameraGoal.z - cameraPosition.z);
        cameraTransform.transform.position = cameraPosition + (xdif * speed);
        //CAMERA MOVE END--------------------------------------
    }
}
