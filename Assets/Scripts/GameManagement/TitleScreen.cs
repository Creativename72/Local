using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public FadeScript fader;


    //used to change scenes, mainly for moving to first scene in game
    public void ChangeScene()
    {
        fader.FadeToNextScene();
            // SceneManager.LoadScene(sceneName);
    }

    //Quits the game
    public void ExitGame()
    {
        Application.Quit();
    }
}
