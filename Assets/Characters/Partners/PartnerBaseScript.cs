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
    public bool exitSpin = false;

    private bool partnerAbilityTriggered = false;

    private void Awake()
    {
        controls = new GameControls();
    }

    virtual public void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    virtual public void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

    // Start is called before the first frame update
    virtual public void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (GameDataTracker.cutsceneMode != GameDataTracker.cutsceneModeOptions.Cutscene)
        {
            agent.SetDestination(OverworldController.Player.transform.position);
            if (controls.OverworldControls.PartnerAction.triggered) UseAbility();
            if (controls.OverworldControls.PartnerAction.phase == UnityEngine.InputSystem.InputActionPhase.Started) HoldAbility();
            else NoAbility();
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
        Vector3 pos_change = Quaternion.AngleAxis(-OverworldController.CameraHeading, Vector3.up) * (OverworldController.Player.transform.position - transform.position);
        if (exitSpin)
        {
            CleanupAbility();
            GetComponent<SpriteFlipper>().setSpecificGoal(90);
        }
        else
        {
            if (pos_change.x > 0.2) GetComponent<SpriteFlipper>().setFacingRight();
            if (pos_change.x < -0.2) GetComponent<SpriteFlipper>().setFacingLeft();
        }
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
        partnerAbilityTriggered = true;
    }

    virtual public void HoldAbility()
    {

    }

    virtual public void NoAbility()
    {
        if (partnerAbilityTriggered)
        {
            partnerAbilityTriggered = false;
            AbilityReleased();
        }
    }

    virtual public void AbilityReleased()
    {

    }

    virtual public void CleanupAbility()
    {

    }
}
