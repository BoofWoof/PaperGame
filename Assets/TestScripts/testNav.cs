using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testNav : MonoBehaviour
{
    public NavMeshAgent agent;
    public float walkRadius;
    public float secondsTillMove = 2;
    public float secondsTillMoveRange = 1;
    private float wait_count = 0;

    // Update is called once per frame
    void Update()
    {
        if (OverworldController.gameMode != OverworldController.gameModeOptions.Cutscene)
        {
            if (agent.remainingDistance < 0.01 * walkRadius)
            {
                wait_count += Time.deltaTime;
                if (wait_count >= secondsTillMove)
                {
                    wait_count = Random.value * secondsTillMoveRange - secondsTillMoveRange / 2;
                    Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
                    randomDirection += transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas);
                    agent.SetDestination(hit.position);
                }
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        transform.rotation = Quaternion.identity;
    }
}
