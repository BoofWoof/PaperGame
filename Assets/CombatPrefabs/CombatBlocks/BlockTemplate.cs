using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTemplate : MonoBehaviour
{
    [Header("Passability")]
    public bool Walkable = true;
    public bool Flyable = true;
    public bool Swimable = false;

    [Header("Viable Interactions")]
    public bool PlaceObjectsOn = true;
    public bool Alterable = true;

    [Header("Routing Info")]
    public float WalkCost = 1;
    public float FlyCost = 1;
    public float SwimCost = 1;

    [Header("Routing Info")]
    public GameObject[] tileVariations;
    public float[] tileWeights;

    public void Start()
    {
        if (tileVariations.Length > 0)
        {
            float totalWeights = 0;
            foreach (int weight in tileWeights)
            {
                totalWeights += weight;
            }
            float blockKeep = Random.Range(0f, 1f);
            float prevTotal = 0;
            for (int blockIdx = 0; blockIdx < tileVariations.Length; blockIdx++)
            {
                float weight = tileWeights[blockIdx] / totalWeights;
                if (blockKeep < prevTotal || blockKeep > prevTotal + weight)
                {
                    Destroy(tileVariations[blockIdx]);
                }
                prevTotal += weight;
            }
        }
    }


    public virtual void TileEntered(FighterClass enteredCharacter)
    {

    }

    public virtual void EndTurnOn(FighterClass enteredCharacter)
    {

    }

    public virtual void ObjectTileEntered(ObjectTemplate enteredObject)
    {

    }
}
