using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static string gameMode = "Mobile";
    public GameObject Playerinit = null;
    public static GameObject Player;

    public void Start()
    {
        Player = Playerinit;
    }
}


