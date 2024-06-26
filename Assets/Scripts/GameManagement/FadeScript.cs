using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//PREFAB THAT THIS SCRIPT IS ATTACHED TO MUST BE A CHILD OF A CANVAS

public class FadeScript : MonoBehaviour
{
    //animator of the black fade background
    [SerializeField] Animator fadeAnim;

    //scene should start as black
    private bool fadeBlack = true;

    //Inspector settings for fading to a scene
    [SerializeField] string nextScene;

    // start the scene by fading from black to the scene
    void Start()
    {
        ToggleFade();
    }

    //toggles the fade state of the animator
    public void ToggleFade()
    {
        fadeBlack = !fadeBlack;
        if(fadeAnim != null)
        {
            fadeAnim.SetBool("FadeBlack", fadeBlack);
        }
    }

    public void FadeToNextScene()
    {
        if(nextScene != null)
        {
            fadeBlack = true;
            fadeAnim.SetBool("FadeBlack", true);
            Invoke("CallNextScene", 1.5f);
        }
    }

    private void CallNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
