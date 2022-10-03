using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = CameraManager.DefaultCameraOffset;
    public Vector3 speed = CameraManager.DefaultSpeed;
    public Vector2 rotation_speed = new Vector2 (90, 90);

    public float old_heading = 0;
    public float heading = 0;
    public float goal_heading = 0;

    public float old_tilt = 0;
    public float tilt = 0;
    public float goal_tilt = 0;

    public bool linear_move = false;

    public GameObject target = null; //if not set, uses current target
    private void Start()
    {
        heading = gameObject.transform.rotation.eulerAngles.y;
        old_heading = gameObject.transform.rotation.eulerAngles.y;
        goal_heading = gameObject.transform.rotation.eulerAngles.y;

        tilt = gameObject.transform.rotation.eulerAngles.x;
        old_tilt = gameObject.transform.rotation.eulerAngles.x;
        goal_tilt = gameObject.transform.rotation.eulerAngles.x;
    }
    void LateUpdate()
    {
        GameObject track_target = CameraManager.TrackTarget;
        if (target != null)
        {
            track_target = target;
        }
        Vector3 trackPosition = track_target.transform.position;
        Vector3 cameraPosition = gameObject.transform.position;

        //CAMERA GOAL DIALOGUE START-----------------------------------------------
        heading = Mathf.MoveTowards(old_heading, goal_heading, rotation_speed.x * Time.deltaTime);
        Quaternion rotation = Quaternion.AngleAxis(heading - old_heading, Vector3.up);
        cameraPosition = (rotation * (cameraPosition - trackPosition)) + trackPosition;
        old_heading = heading;

        tilt = Mathf.MoveTowards(old_tilt, goal_tilt, rotation_speed.y * Time.deltaTime);
        Quaternion rotation2 = Quaternion.AngleAxis(tilt - old_tilt, Vector3.right);
        cameraPosition = (rotation2 * (cameraPosition - trackPosition)) + trackPosition;
        old_tilt = tilt;

        Quaternion goal_rotation = Quaternion.AngleAxis(heading, Vector3.up);
        Vector3 cameraGoal = trackPosition + goal_rotation * new Vector3(offset.x, offset.y, offset.z);

        if (linear_move)
        {
            gameObject.transform.position = Vector3.MoveTowards(cameraPosition, cameraGoal, speed.x * Time.deltaTime);
        } else
        {
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
        }
        gameObject.transform.rotation = Quaternion.Euler(tilt, heading, gameObject.transform.rotation.z);
    }
}
