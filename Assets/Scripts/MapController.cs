using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MapController : MonoBehaviour
{
    public int currentStage;
    public bool[] HouseStates; // HouseStates order: Annie, Scout, Tyler, Walter
    public House[] houses;
    public string currentScene;
    public SpriteRenderer s;
    public GameObject canvas;
    public static MapController Instance { get; private set; }

    public SceneChangeEvent m_sceneChangeEvent;
    
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        currentStage = 0;
        currentScene = "-";
        HouseStates = new bool[4] {
            false, false, false, false
        };
        UpdateDay();

        if (m_sceneChangeEvent == null) {
            m_sceneChangeEvent = new SceneChangeEvent();
        }
    }

    public void LoadNextScene(string name) {
        StartCoroutine(LoadNextSceneAsync(name));
    }

    /** 
        Loads the next scene additively, waits until the load is finished, and then
        unloads the previous scene
    
        @param n Name of the next scene to load
    **/
    IEnumerator LoadNextSceneAsync(string n) {
        Camera.main.GetComponent<AudioListener>().enabled = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(n, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (currentScene != "-")
            SceneManager.UnloadSceneAsync(currentScene);
        s.enabled = false;
        canvas.SetActive(false);
        
        currentScene = n;
        m_sceneChangeEvent.Invoke(n);
    }

    public void UpdateDay() {
        if (!HouseStates[0] && !HouseStates[1] && 
            !HouseStates[2] && !HouseStates[3]) {
            currentStage++;
            Debug.Log("current stage:" + currentStage);
            // Setting it up as each day being 
            switch(currentStage) {
                case 1:
                    HouseStates[0] = true;
                    HouseStates[1] = false;
                    HouseStates[2] = true;
                    HouseStates[3] = true;
                    break;
                case 2:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
                case 3:
                    HouseStates[0] = true;
                    HouseStates[1] = false;
                    HouseStates[2] = true;
                    HouseStates[3] = true;
                    break;
                case 4:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
                case 5:
                    HouseStates[0] = true;
                    HouseStates[1] = false;
                    HouseStates[2] = true;
                    HouseStates[3] = true;
                    break;
                case 6:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
        }
        }
    }

}


[System.Serializable]
public class SceneChangeEvent : UnityEvent<string>
{
}
