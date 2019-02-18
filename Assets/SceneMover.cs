using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider trig)
    {
        if (trig.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
