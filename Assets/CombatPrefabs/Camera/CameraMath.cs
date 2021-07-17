using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMath
{

    public static void GetCameraPosition(Vector2Int mapShape, Vector3 blockOffset, float maxHeight, float downTilt, Camera cameraSource)
    {
        float verticalFOV = cameraSource.fieldOfView;
        float horizontalFOV = Camera.VerticalToHorizontalFieldOfView(verticalFOV, (Screen.width * 1f) / (Screen.height * 1f));
        float middlePoint = mapShape.x * blockOffset.x / 2f;

        float depth = mapShape.y * blockOffset.y;
        float rampAngle = Mathf.Atan(maxHeight / depth);
        float rampDistance = Mathf.Sqrt(Mathf.Pow(maxHeight, 2) + Mathf.Pow(depth, 2));
        float vDistance = rampDistance * 1.2f / (2f * Mathf.Tan(verticalFOV * Mathf.Deg2Rad) / 2f);

        float width = (mapShape.x + 2f) * blockOffset.x * 1.2f;
        float hDistance = width / (2f * Mathf.Tan(horizontalFOV * Mathf.Deg2Rad) / 2f);

        float distance;
        if(hDistance > vDistance)
        {
            distance = hDistance;
        } else
        {
            distance = vDistance;
        }
        
        Vector3 screenPosition;
        float cameraPositionAngleMovement = 0.2f;
        float cameraPositionAngleCurrent = 0f;
        bool lastMoveUp = true;
        cameraSource.gameObject.transform.position = new Vector3(middlePoint, distance * Mathf.Sin(cameraPositionAngleCurrent), -distance * Mathf.Cos(cameraPositionAngleCurrent));
        for (int iterations = 0; iterations < 15; iterations++)
        {
            screenPosition = cameraSource.WorldToScreenPoint(new Vector3(middlePoint, depth / 2f * Mathf.Sin(rampAngle), depth / 2f));
            if (screenPosition.y > Screen.height*5f / 9f)
            {
                cameraPositionAngleCurrent += cameraPositionAngleMovement;
                if (!lastMoveUp) cameraPositionAngleMovement /= 3;
                lastMoveUp = true;
            } else
            {
                cameraPositionAngleCurrent -= cameraPositionAngleMovement;
                if (lastMoveUp) cameraPositionAngleMovement /= 3;
                lastMoveUp = false;
            }
            if (cameraPositionAngleCurrent < 0)
            {
                cameraPositionAngleCurrent = 0;
            }
            cameraSource.gameObject.transform.position = new Vector3(middlePoint, distance * Mathf.Sin(cameraPositionAngleCurrent), -distance * Mathf.Cos(cameraPositionAngleCurrent));
        }

        //float rampBaseToCameraAngle = (180 - rampAngle - downTilt) * Mathf.Deg2Rad;
        //Debug.Log(rampBaseToCameraAngle);
        //float unknownTriangleSide = Mathf.Sqrt(Mathf.Pow(rampDistance, 2) + Mathf.Pow(distance, 2) - 2f * rampDistance * distance * Mathf.Cos(rampBaseToCameraAngle));
        //float visibleSceneAngle = Mathf.Asin(rampDistance * Mathf.Sin(rampBaseToCameraAngle) / unknownTriangleSide);
        //float finalAngle = downTilt * Mathf.Deg2Rad + visibleSceneAngle / 2f;

        //Debug.Log(finalAngle);
        //Debug.Log(distance);
        //Vector3 placeA = new Vector3(middlePoint, distance * Mathf.Sin(finalAngle), distance * Mathf.Cos(finalAngle));
    }
}
