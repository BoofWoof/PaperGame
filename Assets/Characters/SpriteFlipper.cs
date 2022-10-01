﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    public bool useAnimatedTurn = false;
    public bool startsLeft = true;
    public bool targetLeft = true;
    public GameObject sprite;

    //Rotation variables
    public float rotated = 0.0f;
    public float rotSpeedMagnitude = 360;
    private float rotSpeed;
    [HideInInspector]public float goal = 0.0f;
    
    void Start()
    {
    }

    public void setSpecificGoal(float goal_input)
    {
        goal = goal_input;
    }
    public void setSpecificGoalInstant(float goal_input)
    {
        goal = goal_input;
        rotated = goal_input;
    }

    public void setFacingRight()
    {
        goal = 180;
        targetLeft = false;
    }

    public void setFacingRightInstant()
    {
        goal = 180;
        rotated = 180;
        targetLeft = false;
    }
    public void setFacingLeft()
    {
        goal = 0;
        targetLeft = true;
    }
    public void setFacingLeftInstant()
    {
        goal = 0;
        rotated = 0;
        targetLeft = true;
    }
    public void flip()
    {
        if (targetLeft) setFacingRight();
        else setFacingLeft();
    }
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, CameraManager.CameraHeading, 0);
        if (!useAnimatedTurn)
        {
            ForceUpdate();
        }
    }

    public void ForceUpdate()
    {
        //SPRITE ROTATION START-------------------------------------------------------------------------
        if ((goal > rotated))
        {
            rotSpeed = rotSpeedMagnitude * Time.deltaTime;
            rotated = rotated + rotSpeed;
            if (rotated > goal) rotated = goal;
        }
        if ((goal < rotated))
        {
            rotSpeed = -rotSpeedMagnitude * Time.deltaTime;
            rotated = rotated + rotSpeed;
            if (rotated < goal) rotated = goal;
        }
        if (rotated > 90) sprite.transform.localRotation = Quaternion.Euler(0, 180 + rotated, 0);
        else sprite.transform.localRotation = Quaternion.Euler(0, rotated, 0);
        Vector3 currentScale = sprite.transform.localScale;
        if ((rotated < 90))
        {
            sprite.transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        } else
        {
            sprite.transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
    }
}
