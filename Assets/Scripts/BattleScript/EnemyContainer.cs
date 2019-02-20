using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public GameObject EnemyType1 = null;
    public GameObject EnemyType2 = null;
    public GameObject EnemyType3 = null;
    public GameObject EnemyType4 = null;
    public GameObject EnemyType5 = null;
    public GameObject EnemyType6 = null;
    [HideInInspector] public List<GameObject> EnemyList = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (EnemyType1 != null)
        {
            EnemyList.Add(EnemyType1);
        }
        if (EnemyType2 != null)
        {
            EnemyList.Add(EnemyType2);
        }
        if (EnemyType3 != null)
        {
            EnemyList.Add(EnemyType3);
        }
        if (EnemyType4 != null)
        {
            EnemyList.Add(EnemyType4);
        }
        if (EnemyType5 != null)
        {
            EnemyList.Add(EnemyType5);
        }
        if (EnemyType6 != null)
        {
            EnemyList.Add(EnemyType6);
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
