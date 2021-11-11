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
    CutSceneClass cutscene;

    Vector3 blockOffset;
    Vector2Int EndPos;
    int[,] gridHeight;
    GameObject[,] characterGrid;

    public override bool Activate()
    {
        blockOffset = CombatExecutor.blockOffset;
        gridHeight = CombatExecutor.gridHeight;
        characterGrid = CombatExecutor.characterGrid;
        source = parent.GetComponent<FighterClass>();

        //source.RemoveObjectFromGrid();
        Vector3 currentPosition = GridManager.GridToPosition(source.pos, source.TileSize);
        MoveToLocation moveTo = ScriptableObject.CreateInstance<MoveToLocation>();
        if (parent.GetComponent<SpriteFlipper>().targetLeft)
        {
            EndPos = source.pos + new Vector2Int(-1, 0);
            moveTo.endPosition = currentPosition - new Vector3(GridManager.blockOffset.x/2f, 0, 0);
        }
        else
        {
            EndPos = source.pos + new Vector2Int(1, 0);
            moveTo.endPosition = currentPosition + new Vector3(GridManager.blockOffset.x / 2f, 0, 0);
        }
        moveTo.parent = parent;
        moveTo.speed = source.WalkSpeed;
        cutscene = moveTo;
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
            bool rollAllowed = false;
            if (BattleMapProcesses.isThisOnTheGrid(EndPos))
            {
                List<Vector2Int> potentialGridOccupations = source.PotentialGridOccupation(EndPos);
                bool landingEmpty = BattleMapProcesses.isTileEmpty(potentialGridOccupations, source.gameObject);
                if (landingEmpty && BattleMapProcesses.CanIMoveToTile(EndPos, source))
                {
                    rollAllowed = true;
                }
            }
            if (rollAllowed)
            {
                MoveToLocation moveTo = ScriptableObject.CreateInstance<MoveToLocation>();
                moveTo.endPosition = GridManager.GridToPosition(EndPos, source.TileSize);
                moveTo.parent = parent;
                moveTo.speed = source.WalkSpeed;
                cutscene = moveTo;

                source.RemoveObjectFromGrid();
                source.AddObjectToGrid(EndPos);
            } else
            {
                parent.GetComponent<SpriteFlipper>().flip();
                JumpToLocation jumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
                jumpTo.endPosition = GridManager.GridToPosition(source.pos, source.TileSize);
                jumpTo.parent = parent;
                jumpTo.heightOverHighestCharacter = 0.5f;
                jumpTo.speed = source.JumpSpeed;
                cutscene = jumpTo;

                if (BattleMapProcesses.isThisOnTheGrid(EndPos))
                {
                    if(characterGrid[EndPos.x, EndPos.y] != null)
                    {
                        target = characterGrid[EndPos.x, EndPos.y].GetComponent<FighterClass>();
                        if (target.objectID <= 10) target.postBufferAttackEffect(source.Power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                    }
                }
            }
            cutscene.Activate();
            cutscenePhase++;
        }
        if (cutscenePhase == 3)
        {
            if (cutscene.Update())
            {
                cutscene = null;
                cutscenePhase++;
                return true;
            }
        }

        //jumpToTwo.endPosition = new Vector3(EndPos.y * blockOffset.x, gridHeight[(int)EndPos.x, (int)EndPos.y] * blockOffset.z + 0, EndPos.x * blockOffset.y);
        return false;
    }
}
