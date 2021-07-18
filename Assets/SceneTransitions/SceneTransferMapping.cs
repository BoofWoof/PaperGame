using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransferMapping : MonoBehaviour
{
    public static GameObject[] sceneTransitionMap;

    public GameObject[] sceneTransitionInput;

    void Awake()
    {
        sceneTransitionMap = sceneTransitionInput;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
