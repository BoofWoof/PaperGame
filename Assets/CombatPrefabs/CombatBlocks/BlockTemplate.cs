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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
