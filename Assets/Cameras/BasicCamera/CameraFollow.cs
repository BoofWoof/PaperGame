using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToTrack;
    public Vector3 offset = new Vector3(0, 0, 0);
    public float dialogueOffsetMultiplier = 1.0f;
    public float speed = 8f;
    public float trackingcameraY;

    public bool combat;

    public bool OverworldCamera = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 objectPosition = ObjectToTrack.transform.position;
        Vector3 cameraPosition = gameObject.transform.position;
        Vector3 cameraGoal = gameObject.transform.position;
        //CAMERA GOAL DIALOGUE START-----------------------------------------------
        if (combat)
        {
            cameraGoal = new Vector3(objectPosition.x + dialogueOffsetMultiplier * offset.x, objectPosition.y + dialogueOffsetMultiplier * offset.y, objectPosition.z + dialogueOffsetMultiplier * offset.z);
        }
        else
        {
            cameraGoal = new Vector3(objectPosition.x + dialogueOffsetMultiplier * offset.x, trackingcameraY + dialogueOffsetMultiplier * offset.y, objectPosition.z + dialogueOffsetMultiplier * offset.z);
        }
        
        gameObject.transform.position = Vector3.MoveTowards(cameraPosition,cameraGoal, speed*Time.deltaTime);
    }
}
