using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    // Variables
    int startingSceneIndex;

    // making the ScenePersist a singleton on awake
    private void Awake()
    {
        int numberOfScenePersist = FindObjectsOfType<ScenePersist>().Length;

        if (numberOfScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
/*
    // Update is called once per frame
    void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if ( currentSceneIndex != startingSceneIndex )
        {
            Destroy(gameObject);
        }
    }
*/
    public void DestroyScenePersist()
    {
        Destroy(gameObject);
    }
}
