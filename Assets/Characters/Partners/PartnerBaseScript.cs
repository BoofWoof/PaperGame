using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PartnerBaseScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public float walkRadius;
    public float secondsTillMove = 2;
    public float secondsTillMoveRange = 1;
    private float wait_count = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDataTracker.gameMode != GameDataTracker.gameModeOptions.Cutscene)
        {
            agent.SetDestination(OverworldController.Player.transform.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        Vector3 pos_change = OverworldController.Player.transform.position - transform.position;
        if (pos_change.x > 0.2) GetComponent<SpriteFlipper>().setFacingRight();
        if (pos_change.x < -0.2) GetComponent<SpriteFlipper>().setFacingLeft();
    }
}
