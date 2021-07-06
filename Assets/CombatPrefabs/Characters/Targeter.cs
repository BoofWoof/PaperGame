using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : ScriptableObject
{
    public GameObject source;

    public List<GameObject> potentialTargets;
    public moveTemplate.TargetQuantity targetQuantity;
    public Sprite targeterSprite;

    private GameObject indicator;
    private List<GameObject> indicators;
    private int indicatorIdx = 0;

    private List<GameObject> selectedTargets = new List<GameObject>();

    private float heightOverCharacter = 0;

    //UI Info

    //Controller Delay
    private bool lastOn = false;

    public void Activate()
    {
        if (potentialTargets.Count > 0)
        {
            if (targetQuantity == moveTemplate.TargetQuantity.All)
            {
                indicators = new List<GameObject>();
                for (int targetIdx = 0; targetIdx < potentialTargets.Count; targetIdx++)
                {
                    indicator = new GameObject();
                    SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
                    sr.sprite = targeterSprite;
                    Vector3 indicatorLift = new Vector3(0, potentialTargets[targetIdx].GetComponent<FighterClass>().CharacterHeight + heightOverCharacter, 0);
                    indicator.transform.position = potentialTargets[targetIdx].transform.position + indicatorLift;
                    indicators.Add(indicator);
                }
            }
            if (targetQuantity == moveTemplate.TargetQuantity.Single ||
                targetQuantity == moveTemplate.TargetQuantity.Multiple)
            {
                indicator = new GameObject();
                SpriteRenderer sr = indicator.AddComponent<SpriteRenderer>();
                sr.sprite = targeterSprite;

                float closest = 99999;
                int potential_index = indicatorIdx;
                for (int targetIdx = 0; targetIdx < potentialTargets.Count; targetIdx++)
                {
                    GameObject potentialTargetObject = potentialTargets[targetIdx];
                    float distance = Vector2.Distance(
                        new Vector2(source.transform.position.x, source.transform.position.z),
                        new Vector2(potentialTargetObject.transform.position.x, potentialTargetObject.transform.position.z)
                        );
                    float xpos = potentialTargets[targetIdx].transform.position.x;
                    if (distance < closest)
                    {
                        closest = distance;
                        potential_index = targetIdx;
                    }
                }
                indicatorIdx = potential_index;
                Vector3 indicatorLift = new Vector3(0, potentialTargets[indicatorIdx].GetComponent<FighterClass>().CharacterHeight + heightOverCharacter, 0);
                indicator.transform.position = potentialTargets[indicatorIdx].transform.position + indicatorLift;
            }
        }
    }

    public List<GameObject> TargeterUpdate(float horizontalAxis, float verticalAxis, bool Submit)
    {
        if (targetQuantity == moveTemplate.TargetQuantity.All)
        {
            AllTarget();
            if (Submit)
            {
                foreach (GameObject ind in indicators)
                {
                    Destroy(ind);
                }
                return potentialTargets;
            }
        }
        if (targetQuantity == moveTemplate.TargetQuantity.Multiple)
        {
            MultipleTargets(horizontalAxis, Submit);
            foreach (GameObject ind in indicators)
            {
                Destroy(ind);
            }
        }
        if (targetQuantity == moveTemplate.TargetQuantity.Single)
        {
            SingleTarget(horizontalAxis, verticalAxis);
            if (Submit)
            {
                Destroy(indicator);
                selectedTargets.Add(potentialTargets[indicatorIdx]);
                return selectedTargets;
            }
        }
        return null;
    }

    private void faceIndicator(GameObject indicator)
    {
        float positionDif = source.transform.position.x - indicator.transform.position.x;
        if (positionDif > 0.5f)
        {
            source.GetComponent<SpriteFlipper>().setFacingLeft();
        }
        if (positionDif < -0.5f)
        {
            source.GetComponent<SpriteFlipper>().setFacingRight();
        }
    }

    public bool Undo()
    {
        if (targetQuantity == moveTemplate.TargetQuantity.All)
        {
            foreach(GameObject ind in indicators)
            {
                Destroy(ind);
            }
            return true;
        }
        if (targetQuantity == moveTemplate.TargetQuantity.Multiple)
        {
            if (selectedTargets.Count > 0)
            {
                return false;
            }
            foreach (GameObject ind in indicators)
            {
                Destroy(ind);
            }
            return true;
        }
        if (targetQuantity == moveTemplate.TargetQuantity.Single)
        {
            Destroy(indicator);
            return true;
        }
        return true;
    }

    public void SingleTarget(float horizontalAxis, float verticalAxis)
    {
        if (Mathf.Abs(horizontalAxis) > 0.2 || Mathf.Abs(verticalAxis) > 0.2)
        {
            if (lastOn == false)
            {
                int potential_index = indicatorIdx;
                float closest = 99999;
                GameObject currentTargetObject = potentialTargets[indicatorIdx];
                GameObject potentialTargetObject = null;
                if (Mathf.Abs(horizontalAxis) > 0.2)
                {
                    float current_xpos = potentialTargets[indicatorIdx].transform.position.x;
                    if (horizontalAxis > 0)
                    {
                        for (int targetIdx = 0; targetIdx < potentialTargets.Count; targetIdx++)
                        {
                            potentialTargetObject = potentialTargets[targetIdx];
                            float distance = Vector2.Distance(
                                new Vector2(currentTargetObject.transform.position.x, currentTargetObject.transform.position.z),
                                new Vector2(potentialTargetObject.transform.position.x, potentialTargetObject.transform.position.z)
                                );
                            float xpos = potentialTargets[targetIdx].transform.position.x;
                            if (current_xpos < xpos && distance < closest)
                            {
                                closest = distance;
                                potential_index = targetIdx;
                            }
                        }
                    }
                    else
                    {
                        for (int targetIdx = 0; targetIdx < potentialTargets.Count; targetIdx++)
                        {
                            potentialTargetObject = potentialTargets[targetIdx];
                            float distance = Vector2.Distance(
                                new Vector2(currentTargetObject.transform.position.x, currentTargetObject.transform.position.z),
                                new Vector2(potentialTargetObject.transform.position.x, potentialTargetObject.transform.position.z)
                                );
                            float xpos = potentialTargets[targetIdx].transform.position.x;
                            if (current_xpos > xpos && distance < closest)
                            {
                                closest = distance;
                                potential_index = targetIdx;
                            }
                        }
                    }
                }
                if (Mathf.Abs(verticalAxis) > 0.2)
                {
                    float current_zpos = potentialTargets[indicatorIdx].transform.position.z;
                    if (verticalAxis > 0)
                    {
                        for (int targetIdx = 0; targetIdx < potentialTargets.Count; targetIdx++)
                        {
                            potentialTargetObject = potentialTargets[targetIdx];
                            float distance = Vector2.Distance(
                                new Vector2(currentTargetObject.transform.position.x, currentTargetObject.transform.position.z),
                                new Vector2(potentialTargetObject.transform.position.x, potentialTargetObject.transform.position.z)
                                );
                            float zpos = potentialTargets[targetIdx].transform.position.z;
                            if (current_zpos < zpos && distance < closest)
                            {
                                closest = distance;
                                potential_index = targetIdx;
                            }
                        }
                    }
                    else
                    {
                        for (int targetIdx = 0; targetIdx < potentialTargets.Count; targetIdx++)
                        {
                            potentialTargetObject = potentialTargets[targetIdx];
                            float distance = Vector2.Distance(
                                new Vector2(currentTargetObject.transform.position.x, currentTargetObject.transform.position.z),
                                new Vector2(potentialTargetObject.transform.position.x, potentialTargetObject.transform.position.z)
                                );
                            float zpos = potentialTargets[targetIdx].transform.position.z;
                            if (current_zpos > zpos && distance < closest)
                            {
                                closest = distance;
                                potential_index = targetIdx;
                            }
                        }
                    }
                }
                indicatorIdx = potential_index;
                Vector3 indicatorLift = new Vector3(0, potentialTargets[indicatorIdx].GetComponent<FighterClass>().CharacterHeight + heightOverCharacter, 0);
                indicator.transform.position = potentialTargets[indicatorIdx].transform.position + indicatorLift;
            }
            lastOn = true;
        } else
        {
            lastOn = false;
        }
        faceIndicator(indicator);
    }

    public void MultipleTargets(float horizontalAxis, bool Submit)
    {

    }

    public void AllTarget()
    {

    }
}
