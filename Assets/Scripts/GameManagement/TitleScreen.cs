using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //used to change scenes, mainly for moving to first scene in game
    public void ChangeScene(string sceneName)
    {
        if (sceneName != null)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    //Quits the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
