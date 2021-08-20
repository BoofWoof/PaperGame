using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    Line,
    Parabola
}

[RequireComponent(typeof(NavMeshAgent))]
public class PartnerBaseScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool travelingMeshLink = false;
    public GameControls controls;
    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;

    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

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
            if (controls.OverworldControls.PartnerAction.triggered) UseAbility();
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        if (agent.isOnOffMeshLink && !travelingMeshLink)
        {
            if (method == OffMeshLinkMoveMethod.Parabola)
                StartCoroutine(Parabola(agent, 3.0f, 1.5f));
        }
        Vector3 pos_change = OverworldController.Player.transform.position - transform.position;
        if (pos_change.x > 0.2) GetComponent<SpriteFlipper>().setFacingRight();
        if (pos_change.x < -0.2) GetComponent<SpriteFlipper>().setFacingLeft();
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        travelingMeshLink = true;
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        JumpToLocation JumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
        JumpTo.endPosition = endPos;
        JumpTo.parent = gameObject;
        JumpTo.heightOverHighestCharacter = 1;
        JumpTo.speed = duration;
        JumpTo.Activate();
        
        while (!JumpTo.Update())
        {
            yield return 0;
        }
        agent.CompleteOffMeshLink();
        travelingMeshLink = false;
    }

    virtual public void UseAbility()
    {

    }
}
