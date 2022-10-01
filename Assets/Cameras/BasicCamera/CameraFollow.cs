using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = CameraManager.DefaultCameraOffset;
    public Vector3 speed = CameraManager.DefaultSpeed;
    public float heading = 0;
    void LateUpdate()
    {
        Vector3 trackPosition = CameraManager.TrackTarget.transform.position;
        Vector3 cameraPosition = gameObject.transform.position;

        //CAMERA GOAL DIALOGUE START-----------------------------------------------
        Quaternion rotation = Quaternion.AngleAxis(CameraManager.CameraHeading - heading, Vector3.up);
        cameraPosition = (rotation * (cameraPosition - trackPosition)) + trackPosition;
        heading = CameraManager.CameraHeading;

        Quaternion goal_rotation = Quaternion.AngleAxis(CameraManager.CameraHeading, Vector3.up);
        Vector3 cameraGoal = trackPosition + goal_rotation * new Vector3(offset.x, offset.y, offset.z);
        Vector3 rotatedSpeed = new Vector3(
                Mathf.Abs(8 * Mathf.Cos(heading)) + Mathf.Abs(1 * Mathf.Sin(heading)),
                Mathf.Abs(1 * Mathf.Cos(heading)) + Mathf.Abs(8 * Mathf.Sin(heading)),
                8
            );

        gameObject.transform.position = new Vector3(
            Mathf.MoveTowards(cameraPosition.x, cameraGoal.x, Mathf.Abs(rotatedSpeed.x) * Time.deltaTime),
            Mathf.MoveTowards(cameraPosition.y, cameraGoal.y, Mathf.Abs(rotatedSpeed.y) * Time.deltaTime),
            Mathf.MoveTowards(cameraPosition.z, cameraGoal.z, Mathf.Abs(rotatedSpeed.z) * Time.deltaTime)
        );
        gameObject.transform.rotation = Quaternion.Euler(25f, heading, gameObject.transform.rotation.z);
    }
}
