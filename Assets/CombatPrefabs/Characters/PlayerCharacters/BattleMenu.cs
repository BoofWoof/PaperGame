using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleMenu : ScriptableObject
{
    //Target Information
    public GameObject characterTarget;
    public float characterHeight;
    public float characterWidth;

    //Move List
    public GameObject[] movesList;
    public Sprite[] spriteList;
    private GameObject[] spriteObjects;

    //Frequently Used Info
    private int moveCount;
    private Vector3 centerPoint;

    //Rotation Info
    private int goalRotation = 0;
    private float currentRotation = 0;
    public float rotationSpeed = 2f;

    //BattleWheel
    private GameObject selectionWheel;
    private TextMeshPro selectionText;

    //ShadingInfo
    public Material spriteShader;

    public void Activate()
    {
        moveCount = movesList.Length;
        spriteObjects = new GameObject[moveCount];
        centerPoint = new Vector3(characterTarget.transform.position.x - characterWidth, characterTarget.transform.position.y + characterHeight + 0.3f, characterTarget.transform.position.z - 0.2f);

        selectionWheel = Instantiate(characterTarget.GetComponent<FighterClass>().SelectionWheel, centerPoint, Quaternion.identity);
        selectionText = selectionWheel.GetComponent<TextMeshPro>();
        selectionText.text = movesList[goalRotation % moveCount].GetComponent<moveTemplate>().name;

        for (int spriteIdx = 0; spriteIdx < moveCount; spriteIdx++)
        {
            GameObject moveSprite = new GameObject("Menu Sprite");
            SpriteRenderer renderer = moveSprite.AddComponent<SpriteRenderer>();
            renderer.sortingOrder = 999;
            renderer.sprite = spriteList[spriteIdx];
            //renderer.material = spriteShader;
            //moveSprite.AddComponent<SpriteFrontShader>();
            float xOffset = Mathf.Cos(2f * Mathf.PI * ((1.0f * spriteIdx - currentRotation) / moveCount)) * 0.75f;
            float yOffset = Mathf.Sin(2f * Mathf.PI * ((1.0f * spriteIdx - currentRotation) / moveCount)) * 0.75f;
            moveSprite.transform.position = centerPoint + new Vector3(xOffset, yOffset, 0);
            spriteObjects[spriteIdx] = moveSprite;
            movesList[spriteIdx].GetComponent<moveTemplate>().moveIndex = spriteIdx;
            movesList[spriteIdx].GetComponent<moveTemplate>().character = characterTarget;
        }
        movesList[goalRotation % moveCount].GetComponent<moveTemplate>().displayRange();
    }

    public void Deactivate()
    {
        for (int spriteIdx = 0; spriteIdx < moveCount; spriteIdx++)
        {
            GameObject moveSprite = spriteObjects[spriteIdx];
            Destroy(moveSprite);
            Destroy(selectionWheel);
        }
    }

    public GameObject UpdateMenu(float horizontalSpeed, bool selected)
    {
        if (currentRotation == goalRotation)
        {
            if (Mathf.Abs(horizontalSpeed) > 0.3)
            {
                movesList[goalRotation % moveCount].GetComponent<moveTemplate>().hideRange();
                if (horizontalSpeed > 0)
                {
                    goalRotation += 1;
                }
                if (horizontalSpeed < 0)
                {
                    goalRotation -= 1;
                }
                if (goalRotation < 0)
                {
                    goalRotation += moveCount;
                    currentRotation += moveCount;
                }
                movesList[goalRotation % moveCount].GetComponent<moveTemplate>().displayRange();
                selectionText.text = movesList[goalRotation % moveCount].GetComponent<moveTemplate>().name;
            }
        } else
        {
            if (Mathf.Abs(goalRotation - currentRotation) < 0.02)
            {
                currentRotation = goalRotation;
            }
            else
            {
                if (currentRotation > goalRotation)
                {
                    currentRotation -= rotationSpeed * Time.deltaTime;
                }
                else
                {
                    currentRotation += rotationSpeed * Time.deltaTime;
                }
            }
            for (int spriteIdx = 0; spriteIdx < moveCount; spriteIdx++)
            {
                GameObject moveSprite = spriteObjects[spriteIdx];
                float xOffset = Mathf.Cos(2f * Mathf.PI * ((1.0f * spriteIdx - currentRotation) / moveCount)) * 0.75f;
                float yOffset = Mathf.Sin(2f * Mathf.PI * ((1.0f * spriteIdx - currentRotation) / moveCount )) * 0.75f;
                moveSprite.transform.position = centerPoint + new Vector3(xOffset, yOffset, 0);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            return movesList[goalRotation%moveCount];
        }
        return null;
    }
}
