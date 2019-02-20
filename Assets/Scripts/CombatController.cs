using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public GameObject MenuObject;
    // Start is called before the first frame update
    void Start()
    {
        GameObject CombatMenu = Instantiate<GameObject>(MenuObject, transform.position, Quaternion.identity);
        CombatMenu.GetComponent<BattleMenu>().comControl = gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
