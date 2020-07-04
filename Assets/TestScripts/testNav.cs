using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testNav : MonoBehaviour
{
    public NavMeshAgent agent;
    public float walkRadius;

    // Update is called once per frame
    void Update()
    {
        if (OverworldController.gameMode != OverworldController.gameModeOptions.Cutscene)
        {
            if (agent.remainingDistance < 0.1 * walkRadius){
                Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas);
                agent.SetDestination(hit.position);
                }
        }
        transform.rotation = Quaternion.identity;
    }
}
