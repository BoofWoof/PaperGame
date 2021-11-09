using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAttack : moveTemplate
{
    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);
        character.GetComponent<FighterClass>().move = ScriptableObject.CreateInstance<RollAttackScript>();
        character.GetComponent<FighterClass>().move.target = null;
        character.GetComponent<FighterClass>().move.parent = character;
        character.GetComponent<FighterClass>().move.Activate();
    }
}


public class RollAttackScript : CutSceneClass
{
    private int cutscenePhase = 0;
    public FighterClass source;
    private CombatExecutor combatData;
    JumpToLocation cutscene;

    Vector3 blockOffset;
    Vector2Int EndPos;
    int[,] gridHeight;
    GameObject[,] characterGrid;

    public override bool Activate()
    {
        combatData = GameDataTracker.combatExecutor;
        blockOffset = CombatExecutor.blockOffset;
        gridHeight = CombatExecutor.gridHeight;
        characterGrid = CombatExecutor.characterGrid;
        source = parent.GetComponent<FighterClass>();
        EndPos = BattleMapProcesses.findNearestTileFullyFitsObject(source.TileSize, target.pos);

        source.RemoveObjectFromGrid();
        
        JumpToLocation jumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
        jumpTo.endPosition = GridManager.GridToPosition(EndPos, source.TileSize);
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
            source.animator.SetTrigger("CrateAttack");
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
            List<Vector2Int> potentialGridOccupations = source.PotentialGridOccupation(EndPos);
            bool landingEmpty = BattleMapProcesses.isTileEmpty(potentialGridOccupations, source.gameObject);
            if (landingEmpty)
            {
                source.Stun(1);
                source.LightWeight(1);
                parent.transform.position = GridManager.GridToPosition(EndPos, source.TileSize);
            } else
            {
                if (BattleMapProcesses.doesObjectOverlapTargets(potentialGridOccupations, target))
                {
                    target.postBufferAttackEffect(source.Power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                }
            }
            if(!landingEmpty || !CombatExecutor.LevelFloor(EndPos, source.TileSize))
            {
                List<Vector2Int> possibleLocations;
                if (!landingEmpty)
                {
                    possibleLocations = BattleMapProcesses.FindNearestTileNoCharacter(EndPos, 3, source.gameObject);
                    source.animator.SetTrigger("Land");
                } else
                {
                    possibleLocations = BattleMapProcesses.FindNearestTileNoCharacter(EndPos, 1, source.gameObject);
                }
                int locationIndex = Random.Range(0, possibleLocations.Count);
                EndPos = possibleLocations[locationIndex];

                JumpToLocation jumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
                jumpTo.parent = parent;
                jumpTo.speed = source.JumpSpeed * 1.5f;
                jumpTo.heightOverHighestCharacter = 1;
                jumpTo.endPosition = GridManager.GridToPosition(EndPos, source.TileSize);
                jumpTo.Activate();
                cutscene = jumpTo;
            }
            characterGrid[(int)EndPos.x, (int)EndPos.y] = parent;
            source.AddObjectToGrid(EndPos);
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

        //jumpToTwo.endPosition = new Vector3(EndPos.y * blockOffset.x, gridHeight[(int)EndPos.x, (int)EndPos.y] * blockOffset.z + 0, EndPos.x * blockOffset.y);
        return false;
    }
}
