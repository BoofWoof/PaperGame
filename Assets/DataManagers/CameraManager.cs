using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CameraManager
{
    public static List<GameObject> cameras = new List<GameObject>();
    public static GameObject TrackTarget = null;

    public static Vector3 DefaultCameraOffset = new Vector3(0, 2.5f, -4);
    public static Vector3 DefaultRotationSpeed = Vector3.one;
    public static Vector3 DefaultSpeed = new Vector3(8f, 3f, 1f);
    public static Vector3 DefaultRotation = new Vector3(25f, 0, 0);

    public static float CameraHeading = 0f;

    public static GameObject ActiveCamera = null;

    public static GameObject CreateNewCamera(GameObject cameraResource, GameObject target, Vector3 startingOffset, Vector3 startingRotation, bool setActiveCamera)
    {
        GameObject newCamera =  Object.Instantiate<GameObject>(cameraResource, target.transform.position + startingOffset, Quaternion.Euler(target.transform.rotation.eulerAngles + startingRotation));
        cameras.Add(newCamera);
        if (setActiveCamera)
        {
            TrackTarget = target;
            ActiveCamera = newCamera;
        }
        return newCamera;
    }

    public static void UpdateHeading()
    {
        CameraHeading = ActiveCamera.GetComponent<CameraFollow>().heading;
    }

    public static void ClearCameraList()
    {
        cameras.Clear();
    }
}
