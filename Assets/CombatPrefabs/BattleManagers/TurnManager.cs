using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : ScriptableObject
{
    public enum turnPhases{
        ClipTurnStart,
        ClipTurn,
        ClipTurnEnd,
        PartnerTurnStart,
        PartnerTurn,
        PartnerTurnEnd,
        EnemyTurnStart,
        EnemyTurn,
        EnemyTurnEnd,
        GameOver,
        RoundOver,
        PuzzleTurn,
        TurnTiePlayerStart,
        TurnTiePlayerWait,
        TurnTieEnemyStart,
        TurnTieEnemyWait,
    }

    public int turnCount = 0;

    public List<turnPhases> turnQueue = new List<turnPhases>();
    public int goalType;

    public turnPhases Puzzle(bool allGoal)
    {
        turnQueue.Add(turnPhases.PuzzleTurn);
        if (allGoal) goalType = 1;
        else goalType = 2;
        return turnQueue[0];
    }

    public turnPhases TurnTiePuzzle(bool allGoal)
    {
        turnQueue.Add(turnPhases.TurnTiePlayerStart);
        turnQueue.Add(turnPhases.TurnTiePlayerWait);
        turnQueue.Add(turnPhases.TurnTieEnemyStart);
        turnQueue.Add(turnPhases.TurnTieEnemyWait);
        if (allGoal) goalType = 1;
        else goalType = 2;
        return turnQueue[0];
    }

    public turnPhases GoodGuysFirst()
    {
        turnQueue.Add(turnPhases.ClipTurnStart);
        turnQueue.Add(turnPhases.ClipTurn);
        turnQueue.Add(turnPhases.ClipTurnEnd);
        turnQueue.Add(turnPhases.PartnerTurnStart);
        turnQueue.Add(turnPhases.PartnerTurn);
        turnQueue.Add(turnPhases.PartnerTurnEnd);
        turnQueue.Add(turnPhases.EnemyTurnStart);
        turnQueue.Add(turnPhases.EnemyTurn);
        turnQueue.Add(turnPhases.EnemyTurnEnd);
        turnQueue.Add(turnPhases.RoundOver);
        goalType = 0;
        return turnQueue[0];
    }

    private bool requiredEnemiesDead()
    {
        foreach (GameObject enemy in GameDataTracker.combatExecutor.EnemyList)
        {
            if (enemy.GetComponent<FighterClass>().MustBeat) return false;
        }
        return true;
    }

    public turnPhases NextTurn()
    {
        if (GameDataTracker.combatExecutor.Clip.GetComponent<FighterClass>().Dead &&
            GameDataTracker.combatExecutor.Partner.GetComponent<FighterClass>().Dead)
        {
            GameDataTracker.GameOver();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            return turnPhases.GameOver;
        }
        if (requiredEnemiesDead() && goalType == 0)
        {
            SceneManager.LoadScene(GameDataTracker.previousArea);
            return turnPhases.GameOver;
        }
        if (goalType == 1)
        {
            foreach (GoalBlock goalBlock in CombatExecutor.goalBlockList)
            {
                if (goalBlock.active == true)
                {
                    SceneManager.LoadScene(GameDataTracker.previousArea);
                    return turnPhases.GameOver;
                }
            }
        }
        if (goalType == 2)
        {
            bool allActive = true;
            foreach (GoalBlock goalBlock in CombatExecutor.goalBlockList)
            {
                if (goalBlock.active == false)
                {
                    allActive = false;
                    break;
                }
            }
            if (allActive)
            {
                SceneManager.LoadScene(GameDataTracker.previousArea);
                return turnPhases.GameOver;
            }
        }

        turnQueue.Add(turnQueue[0]);
        turnQueue.RemoveAt(0);
        if (turnQueue[0] == turnPhases.RoundOver)
        {
            turnCount++;
            turnQueue.Add(turnQueue[0]);
            turnQueue.RemoveAt(0);
        }
        Debug.Log(turnQueue[0]);
        return turnQueue[0];
    }

}
