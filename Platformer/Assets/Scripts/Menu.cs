using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Load the first level
    public void StartFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    // Load the main menu
    public void LoadMainMenu()
    {
        FindObjectOfType<GameSession>().ResetGameSession();
        //SceneManager.LoadScene(0);
    }

}
