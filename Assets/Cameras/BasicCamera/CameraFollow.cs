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
        if (combat)
        {
            objectPosition.y = trackingcameraY;
        }
        //CAMERA GOAL DIALOGUE START-----------------------------------------------
        cameraGoal = objectPosition + Quaternion.AngleAxis(OverworldController.CameraHeading, Vector3.up) * new Vector3(dialogueOffsetMultiplier * offset.x,  + dialogueOffsetMultiplier * offset.y, dialogueOffsetMultiplier * offset.z);
        
        gameObject.transform.position = Vector3.MoveTowards(cameraPosition,cameraGoal, speed*Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(25f, OverworldController.CameraHeading, gameObject.transform.rotation.z);
    }
}
