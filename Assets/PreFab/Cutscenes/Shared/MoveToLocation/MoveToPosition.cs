using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPosition : CutSceneClass
{
    public string TargetObject;
    public string ReferenceObject;
    public Vector3 PositionOffset;
    public bool Wait = true;
    private GameObject TObject;
    private GameObject RObject;
    private Vector3 waypointPosition;

    private NavMeshAgent agent;

    override public bool Activate()
    {
        if(TargetObject == string.Empty)
        {
            TObject = OverworldController.Player;
            agent = TObject.GetComponent<NavMeshAgent>();
            agent.enabled = true;
        } else {
            TObject = OverworldController.findCharacterByName(TargetObject, OverworldController.CharacterList).CharacterObject;
            agent = TObject.GetComponent<NavMeshAgent>();
        }
        
        if (ReferenceObject == string.Empty)
        {
            RObject = parent;
        } else {
            RObject = GameObject.Find(ReferenceObject);
            if (RObject == null)
            {
                RObject = OverworldController.findCharacterByName(TargetObject, OverworldController.CharacterList).CharacterObject;
            }
        }

        waypointPosition = RObject.transform.position;
        waypointPosition += PositionOffset;
        NavMeshHit hit;
        NavMesh.SamplePosition(waypointPosition, out hit, 2, NavMesh.AllAreas);
        waypointPosition = hit.position;
        agent.SetDestination(waypointPosition);
        
        active = true;
        return Wait;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (active)
        {
            Debug.Log(agent.remainingDistance);
            if (Vector2.Distance(new Vector2(waypointPosition.x, waypointPosition.z), new Vector2(TObject.transform.position.x, TObject.transform.position.z)) < 0.1)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
