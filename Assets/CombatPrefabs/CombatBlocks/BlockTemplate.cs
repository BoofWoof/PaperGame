using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTemplate : GridObject
{
    public List<PlayerEnterTriggerInfo> PlayerEnterTriggers = new List<PlayerEnterTriggerInfo>();

    [Header("PuzzleSettings")]
    public bool GoalTile = false;

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
    
    private float startingOffset = 10;
    public Vector3 finalPosition;
    private float timeTillArrived;
    private float startTimeTillArrived;
    public bool tileReady = false;

    public virtual void Start()
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
        finalPosition = transform.position;
        timeTillArrived = Random.Range(1f, 3f);
        startingOffset = Random.Range(4f, 3f);
        startTimeTillArrived = timeTillArrived;
        transform.position = finalPosition - new Vector3(0, 100, 0);
    }

    public virtual void Update()
    {
        if(timeTillArrived > 0)
        {
            timeTillArrived -= Time.deltaTime;
            if(timeTillArrived < 0)
            {
                timeTillArrived = 0;
                tileReady = true;
            }
            transform.position = finalPosition - new Vector3(0, startingOffset * Mathf.Pow(timeTillArrived / startTimeTillArrived, 5) * Mathf.Cos(3.25f*2*Mathf.PI*(1 - timeTillArrived/startTimeTillArrived)), 0);
        }
    }


    public virtual void TileEntered(FighterClass enteredCharacter)
    {
        foreach (PlayerEnterTriggerInfo playerEnterTrigger in PlayerEnterTriggers)
        {
            if (enteredCharacter.objectID == 0 && playerEnterTrigger.Clip || (enteredCharacter.objectID <= 10 && enteredCharacter.objectID > 0 && playerEnterTrigger.Partner))
            {
                if (CombatExecutor.CutsceneDataManager.TriggerATrigger(playerEnterTrigger.Label))
                {
                    GameDataTracker.combatExecutor.AddCutsceneToQueue(Resources.Load<DialogueContainer>(playerEnterTrigger.CutscenePath), name, gameObject);
                }
            }
        }
    }

    public virtual void EndTurnOn(FighterClass enteredCharacter)
    {

    }

    public virtual void ObjectTileEntered(ObjectTemplate enteredObject)
    {

    }
}
