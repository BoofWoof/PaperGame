using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : ScriptableObject
{
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
                Vector3 indicatorLift = new Vector3(0, potentialTargets[0].GetComponent<FighterClass>().CharacterHeight + heightOverCharacter, 0);
                indicator.transform.position = potentialTargets[0].transform.position + indicatorLift;
            }
        }
    }

    public List<GameObject> TargeterUpdate(float horizontalAxis, bool Submit)
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
            SingleTarget(horizontalAxis);
            if (Submit)
            {
                Destroy(indicator);
                selectedTargets.Add(potentialTargets[indicatorIdx]);
                return selectedTargets;
            }
        }
        return null;
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

    public void SingleTarget(float horizontalAxis)
    {
        if (Mathf.Abs(horizontalAxis) > 0.2)
        {
            if (lastOn == false)
            {
                if (horizontalAxis > 0)
                {
                    indicatorIdx += 1;
                    if (indicatorIdx > (potentialTargets.Count - 1))
                    {
                        indicatorIdx = 0;
                    }
                }
                else
                {
                    indicatorIdx -= 1;
                    if (indicatorIdx < 0)
                    {
                        indicatorIdx += potentialTargets.Count;
                    }
                }
                Vector3 indicatorLift = new Vector3(0, potentialTargets[indicatorIdx].GetComponent<FighterClass>().CharacterHeight + heightOverCharacter, 0);
                indicator.transform.position = potentialTargets[indicatorIdx].transform.position + indicatorLift;
            }
            lastOn = true;
        } else
        {
            lastOn = false;
        }
    }

    public void MultipleTargets(float horizontalAxis, bool Submit)
    {

    }

    public void AllTarget()
    {

    }
}
