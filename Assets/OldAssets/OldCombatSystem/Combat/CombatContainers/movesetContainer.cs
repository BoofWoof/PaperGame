using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movesetContainer : MonoBehaviour
{
    public GameObject move1 = null;
    public GameObject move2 = null;
    public GameObject move3 = null;
    public GameObject move4 = null;
    public GameObject move5 = null;
    public GameObject move6 = null;
    public GameObject move7 = null;
    [HideInInspector] public List<GameObject> moveList = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        if (move1 != null)
        {
            moveList.Add(move1);
        }
        if (move2 != null)
        {
            moveList.Add(move2);
        }
        if (move3 != null)
        {
            moveList.Add(move3);
        }
        if (move4 != null)
        {
            moveList.Add(move4);
        }
        if (move5 != null)
        {
            moveList.Add(move5);
        }
        if (move6 != null)
        {
            moveList.Add(move6);
        }
        if (move7 != null)
        {
            moveList.Add(move7);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
