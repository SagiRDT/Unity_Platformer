using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndStageCheckpoint : MonoBehaviour
{
    // Config
    [SerializeField] float LevelLoadDelay = 1.5f;
    [SerializeField] AudioClip checkpointSFX = null;
    //[SerializeField] float LevelExitSlowMoFactor = 0.5f;

    // Cached component references
    Animator myAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextStage());
    }


    private IEnumerator LoadNextStage()
    {
        //Time.timeScale = LevelExitSlowMoFactor;
        myAnimator.SetTrigger("Pressed");
        AudioSource.PlayClipAtPoint(checkpointSFX, Camera.main.transform.position);

        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        //Time.timeScale = 1f;

        // Destroy the stage scene persist obj before loading the next level
        FindObjectOfType<ScenePersist>().DestroyScenePersist();

        // load the next scene
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
