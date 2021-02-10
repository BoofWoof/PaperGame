using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneContainer : MonoBehaviour
{
    public GameObject PlayerPosition1;
    public GameObject PlayerPosition2;
    [HideInInspector] public GameObject[] PlayerPositions;
    public GameObject[] EnemyPositions;
    public Vector3 sceneDefaultOffset = new Vector3(0, 2.3f, -6f);
    // Start is called before the first frame update

    private void Awake()
    {
        PlayerPositions = new GameObject[2];
        PlayerPositions[0] = PlayerPosition1;
        PlayerPositions[1] = PlayerPosition2;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
