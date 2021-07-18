using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    public GameObject transitionCanvas;

    public float transitionTime = 0.75f;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transitionCanvas.GetComponent<Animator>().SetTrigger("Start");
        GameDataTracker.transitioning = true;

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
