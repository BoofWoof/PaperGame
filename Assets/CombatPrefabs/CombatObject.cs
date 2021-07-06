using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatObject : MonoBehaviour
{
    [HideInInspector] public Vector2 pos;
    [HideInInspector] public CutSceneClass move;
    [HideInInspector] public Vector2 prevPos;
    public bool Pushable = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual bool PushObject(int HorChange, int VerChange, float Speed, bool Char, CombatExecutor combatExecutor)
    {
        Vector2 EndPos = new Vector2(pos.x + VerChange, pos.y + HorChange);
        prevPos = pos;

        if (EndPos.x >= 0 && EndPos.x < combatExecutor.rows && EndPos.y >= 0 && EndPos.y < combatExecutor.cols && combatExecutor.characterGrid[(int)EndPos.x, (int)EndPos.y] is null &&
            (combatExecutor.gridHeight[(int)EndPos.x, (int)EndPos.y] - combatExecutor.gridHeight[(int)pos.x, (int)pos.y] <= 0) &&
            combatExecutor.blockGrid[(int)EndPos.x, (int)EndPos.y].GetComponent<BlockTemplate>().Walkable && BattleMapProcesses.isObjectPassable(EndPos, combatExecutor))
        {
            if (Char)
            {
                combatExecutor.characterGrid[(int)pos.x, (int)pos.y] = null;
                combatExecutor.characterGrid[(int)EndPos.x, (int)EndPos.y] = gameObject;
            }
            else
            {
                combatExecutor.objectGrid[(int)pos.x, (int)pos.y] = null;
                combatExecutor.objectGrid[(int)EndPos.x, (int)EndPos.y] = gameObject;
            }

            pos = EndPos;
            if (combatExecutor.gridHeight[(int)pos.x, (int)pos.y] != combatExecutor.gridHeight[(int)EndPos.x, (int)EndPos.y])
            {
                JumpToLocation JumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
                JumpTo.endPosition = new Vector3(EndPos.y * combatExecutor.xOffset, combatExecutor.gridHeight[(int)EndPos.x, (int)EndPos.y] * combatExecutor.zOffset + 0, EndPos.x * combatExecutor.yOffset);
                JumpTo.parent = gameObject;
                JumpTo.heightOverHighestCharacter = 0.5f;
                JumpTo.speed = Speed;
                JumpTo.Activate();
                move = JumpTo;
                pos = EndPos;
                return true;
            }
            else
            {
                MoveToLocation WalkTo = ScriptableObject.CreateInstance<MoveToLocation>();
                WalkTo.endPosition = new Vector3(EndPos.y * combatExecutor.xOffset, combatExecutor.gridHeight[(int)EndPos.x, (int)EndPos.y] * combatExecutor.zOffset + 0, EndPos.x * combatExecutor.yOffset);
                WalkTo.parent = gameObject;
                WalkTo.speed = Speed;
                WalkTo.Activate();
                move = WalkTo;
                pos = EndPos;
                return true;
            }
        }
        return false;
    }
}
