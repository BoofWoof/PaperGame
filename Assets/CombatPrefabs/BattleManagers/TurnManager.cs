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
        GameOver
    }

    public int turnCount = 0;

    public List<turnPhases> turnQueue = new List<turnPhases>();

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
        return turnQueue[0];
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
        if (GameDataTracker.combatExecutor.EnemyList.Count == 0)
        {
            SceneManager.LoadScene(GameDataTracker.previousArea);
            return turnPhases.GameOver;
        }

        turnCount++;
        turnQueue.Add(turnQueue[0]);
        turnQueue.RemoveAt(0);
        return turnQueue[0];
    }

}
