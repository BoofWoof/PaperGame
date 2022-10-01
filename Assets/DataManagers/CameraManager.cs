using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CameraManager
{
    public static List<GameObject> cameras = new List<GameObject>();

    public static Vector3 DefaultCameraOffset = new Vector3(0, 2.5f, -4);
    public static Vector3 DefaultRotationSpeed = Vector3.one;
    public static Vector3 DefaultSpeed = new Vector3(8f, 1f, 8f);
    public static float CameraHeading = 0f;
    public static GameObject ActiveCamera = null;
    public static GameObject TrackTarget = null;

    public static GameObject CreateNewCamera(GameObject cameraResource, GameObject target, Vector3 startingOffset, Vector3 startingRotation, bool setActiveCamera)
    {
        TrackTarget = target;
        GameObject newCamera =  Object.Instantiate<GameObject>(cameraResource, target.transform.position + startingOffset, Quaternion.Euler(startingRotation));
        cameras.Add(newCamera);
        return newCamera;
    }

    public static void ClearCameraList()
    {
        cameras.Clear();
    }

    public static void SetNewCameraHeading(float heading)
    {

    }
}
