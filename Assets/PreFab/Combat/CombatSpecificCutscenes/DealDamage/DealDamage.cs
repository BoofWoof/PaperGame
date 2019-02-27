using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DealDamage : CutSceneClass
{
    public int amount;
    public FighterClass.attackType type;
    public FighterClass.statusEffects effects;
    public FighterClass.attackLocation location;
    public GameObject source;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent.GetComponent<FighterClass>().attackEffect(amount, type, effects, location, source);
        cutsceneDone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
