using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockTrigger : MonoBehaviour
{
    public GameObject reference_position;
    public GameObject camera_offset;
    public float new_tilt = -1;
    public float new_heading = 0;

    public void OnTriggerEnter (Collider trigger)
    {
        if(trigger.gameObject == OverworldController.Player)
        {
            CameraFollow camera_info = CameraManager.ActiveCamera.GetComponent<CameraFollow>();
            if(reference_position == null)
            {
                camera_info.target = OverworldController.Player;
                camera_info.offset = CameraManager.DefaultCameraOffset;
                camera_info.linear_move = false;
            }
            else
            {
                camera_info.target = reference_position;
                camera_info.offset = Quaternion.AngleAxis(new_heading, Vector3.up) * (camera_offset.transform.position - reference_position.transform.position);
                camera_info.linear_move = true;
            }
            camera_info.goal_tilt = new_tilt;
            camera_info.goal_heading = new_heading;
        }
    }
}
