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
        CharacterMovementOverworld PlayerController = transform.parent.GetComponent<CharacterMovementOverworld>();
        PlayerController.stopOnCutscene = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPosition = transform.parent.transform.position;
        CharacterMovementOverworld PlayerController = transform.parent.GetComponent<CharacterMovementOverworld>();
        if (travelDirection == SceneMover.exitDirectionOptions.down)
        {
            if (PlayerPosition.z < endPosition.z)
            {
                PlayerController.moveHorizontal = 0;
                PlayerController.moveHorizontal = 0;
                PlayerController.stopOnCutscene = true;
                cutsceneDone();
            }
        }
        if (travelDirection == SceneMover.exitDirectionOptions.left)
        {
            if (PlayerPosition.x < endPosition.x)
            {
                PlayerController.moveHorizontal = 0;
                PlayerController.moveHorizontal = 0;
                PlayerController.stopOnCutscene = true;
                cutsceneDone();
            }
        }
        if (travelDirection == SceneMover.exitDirectionOptions.right)
        {
            if (PlayerPosition.x > endPosition.x)
            {
                PlayerController.moveHorizontal = 0;
                PlayerController.moveHorizontal = 0;
                PlayerController.stopOnCutscene = true;
                cutsceneDone();
            }
        }
        if (travelDirection == SceneMover.exitDirectionOptions.up)
        {
            if (PlayerPosition.z > endPosition.z)
            {
                PlayerController.moveHorizontal = 0;
                PlayerController.moveHorizontal = 0;
                PlayerController.stopOnCutscene = true;
                cutsceneDone();
            }
        }
    }
}
