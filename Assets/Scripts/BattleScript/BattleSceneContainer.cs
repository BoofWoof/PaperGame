using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneContainer : MonoBehaviour
{
    public GameObject PlayerPosition1;
    public GameObject PlayerPosition2;
    public GameObject EnemyPosition1 = null;
    public GameObject EnemyPosition2 = null;
    public GameObject EnemyPosition3 = null;
    public GameObject EnemyPosition4 = null;
    public GameObject EnemyPosition5 = null;
    public GameObject EnemyPosition6 = null;
    [HideInInspector] public List<GameObject> PlayerPositions = new List<GameObject>();
    [HideInInspector] public List<GameObject> EnemyPositions = new List<GameObject>();
    // Start is called before the first frame update

    private void Awake()
    {
        PlayerPositions.Add(PlayerPosition1);
        PlayerPositions.Add(PlayerPosition2);
        if (EnemyPosition1 != null)
        {
            EnemyPositions.Add(EnemyPosition1);
        }
        if (EnemyPosition2 != null)
        {
            EnemyPositions.Add(EnemyPosition2);
        }
        if (EnemyPosition3 != null)
        {
            EnemyPositions.Add(EnemyPosition3);
        }
        if (EnemyPosition4 != null)
        {
            EnemyPositions.Add(EnemyPosition4);
        }
        if (EnemyPosition5 != null)
        {
            EnemyPositions.Add(EnemyPosition5);
        }
        if (EnemyPosition6 != null)
        {
            EnemyPositions.Add(EnemyPosition6);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
