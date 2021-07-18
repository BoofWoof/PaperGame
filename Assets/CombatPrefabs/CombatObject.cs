using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatObject : GridObject
{
    [HideInInspector] public CutSceneClass move;
    public bool Pushable = false;
    public bool Passable = false;

    public bool CanWalk = true;
    public bool CanFly = false;
    public bool CanSwim = false;
    public int MaxJumpHeight = 0;

    public float LastMoveSpeed = 0;

    public bool objectReady = false;
    private Vector3 finalPosition;
    private float dropHeight = 7f;
    private float startDropTime = 0.5f;
    private float currentDropTime;

    public virtual void Start()
    {
        finalPosition = transform.position;
        currentDropTime = startDropTime;
        transform.position = finalPosition + new Vector3(0, 100f, 0);
    }

    public virtual void Update()
    {
        if (!objectReady)
        {
            if (checkTileIsReady())
            {
                currentDropTime -= Time.deltaTime;
                if(currentDropTime < 0)
                {
                    currentDropTime = 0;
                    objectReady = true;
                }
                transform.position = finalPosition + new Vector3(0, dropHeight * Mathf.Pow(currentDropTime / startDropTime, 2) * Mathf.Abs(Mathf.Cos(0.75f * 2 * Mathf.PI * (1 - currentDropTime / startDropTime))), 0);
            }
        }
    }

    public bool checkTileIsReady()
    {
        if (!CombatExecutor.blockGrid[pos.x, pos.y].GetComponent<BlockTemplate>().tileReady) return false; 
        foreach(Vector2Int extPos in extra_pos)
        {
            if (!CombatExecutor.blockGrid[extPos.x, extPos.y].GetComponent<BlockTemplate>().tileReady) return false;
        }
        return true;
    }


    public bool AttemptPush(List<Vector2Int> pushTargets, int HorChange, int VerChange, float Speed, int pushStrength)
    {
        List<FighterClass> charactersToPush = new List<FighterClass>();
        List<CombatObject> objectsToPush = new List<CombatObject>();
        foreach (Vector2Int pushTarget in pushTargets)
        {
            if (!BattleMapProcesses.isThisOnTheGrid(pushTarget)) return false;
            if (CombatExecutor.characterGrid[pushTarget.x, pushTarget.y] != null)
            {
                FighterClass characterToPush = CombatExecutor.characterGrid[pushTarget.x, pushTarget.y].GetComponent<FighterClass>();
                if (!CombatExecutor.characterGrid[pushTarget.x, pushTarget.y].GetComponent<CombatObject>().PushObjectCheck(HorChange, VerChange, Speed, pushStrength - 1)) return false;
                if(!charactersToPush.Contains(characterToPush)) objectsToPush.Add(characterToPush);
            }
            if (CombatExecutor.objectGrid[pushTarget.x, pushTarget.y] != null)
            {
                if (!CombatExecutor.objectGrid[pushTarget.x, pushTarget.y].GetComponent<CombatObject>().Passable)
                {
                    CombatObject objectToPush = CombatExecutor.objectGrid[pushTarget.x, pushTarget.y].GetComponent<CombatObject>();
                    if (!CombatExecutor.objectGrid[pushTarget.x, pushTarget.y].GetComponent<CombatObject>().PushObjectCheck(HorChange, VerChange, Speed, pushStrength - 1)) return false;
                    if (!objectsToPush.Contains(objectToPush)) objectsToPush.Add(objectToPush);
                }
            }
        }

        foreach(CombatObject characterToPush in charactersToPush)
        {
            characterToPush.MoveCharacterExecute(new Vector2Int(characterToPush.pos.x + HorChange, characterToPush.pos.y + VerChange), Speed, Speed, CombatExecutor.characterGrid);
        }
        foreach (CombatObject objectToPush in objectsToPush)
        {
            objectToPush.MoveCharacterExecute(new Vector2Int(objectToPush.pos.x + HorChange, objectToPush.pos.y + VerChange), Speed, Speed, CombatExecutor.objectGrid);
        }
        return true;
    }

    public virtual bool PushObjectCheck(int HorChange, int VerChange, float Speed, int pushStrength, bool overridePushability = false)
    {
        if (pushStrength < 0) return false;
        if (!Pushable && !overridePushability) return false;

        Vector2Int EndPos = new Vector2Int(pos.x + HorChange, pos.y + VerChange);
        List<Vector2Int> potentialGridOccupations = PotentialGridOccupation(EndPos);
        
        foreach (Vector2Int potentialGridOccupation in potentialGridOccupations)
        {
            if (!BattleMapProcesses.isThisOnTheGrid(EndPos)) return false;
            if (!BattleMapProcesses.CanIMoveToTile(EndPos, this)) return false;
            if (CombatExecutor.gridHeight[pos.x, pos.y] < CombatExecutor.gridHeight[potentialGridOccupation.x, potentialGridOccupation.y]) return false;
        }
        if (BattleMapProcesses.isTileEmpty(potentialGridOccupations, gameObject))
        {
            return true;
        }
        if (AttemptPush(potentialGridOccupations, HorChange, VerChange, Speed, pushStrength))
        {
            return true;
        }
        return false;
    }

    public virtual void MoveCharacterExecute(Vector2Int EndPos, float walkSpeed, float jumpSpeed, GameObject[,] grid)
    {
        if (CombatExecutor.gridHeight[EndPos.x, EndPos.y] != CombatExecutor.gridHeight[pos.x, pos.y])
        {
            JumpToLocation JumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
            JumpTo.endPosition = GridManager.GridToPosition(EndPos, TileSize);
            JumpTo.parent = gameObject;
            JumpTo.heightOverHighestCharacter = 1;
            JumpTo.speed = jumpSpeed;
            LastMoveSpeed = jumpSpeed;
            JumpTo.Activate();
            move = JumpTo;
        }
        else
        {
            MoveToLocation WalkTo = ScriptableObject.CreateInstance<MoveToLocation>();
            WalkTo.endPosition = GridManager.GridToPosition(EndPos, TileSize);
            WalkTo.parent = gameObject;
            WalkTo.speed = walkSpeed;
            LastMoveSpeed = walkSpeed;
            WalkTo.Activate();
            move = WalkTo;
        }
        MoveObject(EndPos);
    }

    public List<Vector2Int> gridCollissionCheck()
    {
        List<Vector2Int> collisionPos = new List<Vector2Int>();

        return collisionPos;
    }
}
