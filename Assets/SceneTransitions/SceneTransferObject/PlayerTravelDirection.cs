using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTravelDirection : CutSceneClass
{
    public SceneMover.exitDirectionOptions travelDirection;
    public Vector3 endPosition;
    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        base.Activate();
        CharacterMovementOverworld PlayerController = parent.GetComponent<CharacterMovementOverworld>();
        PlayerController.movementLock = true;
        if (travelDirection == SceneMover.exitDirectionOptions.down)
        {
            PlayerController.moveHorizontal = 0;
            PlayerController.moveVertical = -1;
        }
        if (travelDirection == SceneMover.exitDirectionOptions.left)
        {
            PlayerController.moveHorizontal = -1;
            PlayerController.moveVertical = 0;
        }
        if (travelDirection == SceneMover.exitDirectionOptions.right)
        {
            PlayerController.moveHorizontal = 1;
            PlayerController.moveVertical = 0;
        }
        if (travelDirection == SceneMover.exitDirectionOptions.up)
        {
            PlayerController.moveHorizontal = 0;
            PlayerController.moveVertical = 1;
        }
        active = true;
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (active)
        {
            Vector3 PlayerPosition = parent.transform.position;
            CharacterMovementOverworld PlayerController = parent.GetComponent<CharacterMovementOverworld>();
            if (travelDirection == SceneMover.exitDirectionOptions.down)
            {
                if (PlayerPosition.z < endPosition.z)
                {
                    PlayerController.moveHorizontal = 0;
                    PlayerController.moveVertical = 0;
                    PlayerController.movementLock = false;
                    return true;
                }
            }
            if (travelDirection == SceneMover.exitDirectionOptions.left)
            {
                if (PlayerPosition.x < endPosition.x)
                {
                    PlayerController.moveHorizontal = 0;
                    PlayerController.moveVertical = 0;
                    PlayerController.movementLock = false;
                    return true;
                }
            }
            if (travelDirection == SceneMover.exitDirectionOptions.right)
            {
                if (PlayerPosition.x > endPosition.x)
                {
                    PlayerController.moveHorizontal = 0;
                    PlayerController.moveVertical = 0;
                    PlayerController.movementLock = false;
                    return true;
                }
            }
            if (travelDirection == SceneMover.exitDirectionOptions.up)
            {
                if (PlayerPosition.z > endPosition.z)
                {
                    PlayerController.moveHorizontal = 0;
                    PlayerController.moveVertical = 0;
                    PlayerController.movementLock = false;
                    return true;
                }
            }

        }
        return false;
    }
}
