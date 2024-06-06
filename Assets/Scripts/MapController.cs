using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public int currentDay;
    public bool[] HouseStates; // HouseStates order: Annie, Scout, Tyler, Walter
    public string currentScene;
    public static MapController Instance { get; private set; }
    
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        currentDay = 0;
        currentScene = "map";
        HouseStates = new bool[4] {
            false, true, false, false
        };
        UpdateDay();
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(n, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(currentScene);
        currentScene = n;
    }

    public void UpdateDay() {
        if (!HouseStates[0] && !HouseStates[1] && 
            !HouseStates[2] && !HouseStates[3]) {
            currentDay++;
            
            // Setting it up as each day being 
            switch(currentDay) {
                case 0:
                    HouseStates[0] = false;
                    HouseStates[1] = true;
                    HouseStates[2] = false;
                    HouseStates[3] = false;
                    break;
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
