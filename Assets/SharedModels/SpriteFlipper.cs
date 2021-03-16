using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public bool useAnimatedTurn = false;
    public bool startsLeft = true;
    public GameObject sprite;

    //Rotation variables
    private float rotated = 0.0f;
    private float prev_rotated = 0.0f;
    public float rotSpeedMagnitude = 360;
    private float rotSpeed;
    [HideInInspector]public float goal = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void setFacingRight()
    {
        goal = 180;
    }
    public void setFacingLeft()
    {
        goal = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (!useAnimatedTurn)
        {
            //SPRITE ROTATION START-------------------------------------------------------------------------
            if ((goal > rotated))
            {
                rotSpeed = rotSpeedMagnitude * Time.deltaTime;
                sprite.transform.Rotate(0, rotSpeed, 0);
                rotated = rotated + rotSpeed;
            }
            if ((goal < rotated))
            {
                rotSpeed = -rotSpeedMagnitude * Time.deltaTime;
                sprite.transform.Rotate(0, rotSpeed, 0);
                rotated = rotated + rotSpeed;
            }
            if ((rotated <= 90) && (prev_rotated > 90))
            {
                sprite.transform.Rotate(0, -180, 0);
            }
            if ((rotated >= 90) && (prev_rotated < 90))
            {
                sprite.transform.Rotate(0, 180, 0);
            }
            Vector3 currentScale = sprite.transform.localScale;
            if (rotated < 90)
            {
                sprite.transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
            }
            if (rotated > 90)
            {
                sprite.transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
            }
            prev_rotated = rotated;
        }
    }
}
