using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateLaunch : moveTemplate
{
    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);
        character.GetComponent<FighterClass>().move = ScriptableObject.CreateInstance<CrateLaunchScript>();
        character.GetComponent<FighterClass>().move.target = targets[0].GetComponent<FighterClass>();
        character.GetComponent<FighterClass>().move.parent = character;
        character.GetComponent<FighterClass>().move.Activate();
    }


}


public class CrateLaunchScript : CutSceneClass
{
    private int cutscenePhase = 0;
    public FighterClass source;
    private CombatExecutor combatData;
    JumpToLocation cutscene;

    float xOffset;
    float yOffset;
    float zOffset;
    Vector2 EndPos;
    int[,] gridHeight;
    GameObject[,] characterGrid;

    public override bool Activate()
    {
        combatData = GameDataTracker.combatExecutor;
        xOffset = combatData.xOffset;
        yOffset = combatData.yOffset;
        zOffset = combatData.zOffset;
        gridHeight = combatData.gridHeight;
        characterGrid = combatData.characterGrid;
        EndPos = target.pos;
        source = parent.GetComponent<FighterClass>();

        combatData.characterGrid[(int)source.pos.x, (int)source.pos.y] = null;
        
        JumpToLocation jumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
        jumpTo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset + target.CharacterHeight * 0.1f, EndPos.x * yOffset);
        jumpTo.parent = parent;
        jumpTo.heightOverHighestCharacter = 2.5f;
        jumpTo.speed = source.JumpSpeed;
        cutscene = jumpTo;
        return true;
    }

    public override bool Update()
    {
        if (cutscenePhase == 0)
        {
            cutscene.Activate();
            cutscenePhase++;
        }
        if (cutscenePhase == 1)
        {
            bool done = cutscene.Update();
            if (done)
            {
                cutscene = null;
                cutscenePhase++;
            }
        }
        if (cutscenePhase == 2)
        {
            if (characterGrid[(int)EndPos.x, (int)EndPos.y] is null)
            {
                source.Stun(1);
                source.LightWeight(1);
                parent.transform.position = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset, EndPos.x * yOffset);
            } else
            {
                if (target.pos == EndPos)
                {
                    target.postBufferAttackEffect(source.Power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                }

                List<Vector2> possibleLocations = combatData.FindNearestTileNoCharacter(EndPos, 3);
                int locationIndex = Random.Range(0, possibleLocations.Count);
                EndPos = possibleLocations[locationIndex];

                JumpToLocation jumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
                jumpTo.parent = parent;
                jumpTo.speed = source.JumpSpeed * 1.5f;
                jumpTo.heightOverHighestCharacter = 1;
                jumpTo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset, EndPos.x * yOffset);
                jumpTo.Activate();
                cutscene = jumpTo;
            }
            characterGrid[(int)EndPos.x, (int)EndPos.y] = parent;
            source.pos = EndPos;
            cutscenePhase++;
        }
        if (cutscenePhase == 3)
        {
            if (!(cutscene is null))
            {
                bool done = cutscene.Update();
                if (done)
                {
                    cutscene = null;
                    cutscenePhase++;
                    return true;
                }
            } else
            {
                return true;
            }
        }

        //jumpToTwo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset + 0, EndPos.x * yOffset);
        return false;
    }
}
