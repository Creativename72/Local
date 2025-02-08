using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenuButton;
    [Header("Misc")]
    [SerializeField] private DataManager dataManager;
    [SerializeField] private FadeScript fader;
    [SerializeField] private AudioMixer audioFader;
    public static GameManager Instance { get; private set; }

    private int menuDepth;
    private float gameVolume;
    private float sfxVolume;

    private void Awake()
    {
        if (Instance != null && Instance.gameObject != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuDepth = 0;

        // hardcoded set the volumes
        AudioManager.Instance.SetMusicVolume(0.5f);
        AudioManager.Instance.SetSFXVolume(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        escapeMenu.SetActive(menuDepth == 1 || menuDepth == 2);
        pauseMenu.SetActive(menuDepth == 1);
        optionsMenu.SetActive(menuDepth == 2);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuDepth == 0)
                OpenPause();
            else
                ExitMenu();
        }

        mainMenuButton.SetActive(SceneManager.GetActiveScene().name != "TitleScreen");
    }

    public bool Paused()
    {
        return menuDepth >= 1;
    }

    public float GameVolume()
    {
        return gameVolume;
    }

    public float SFXVolume()
    {
        return sfxVolume;
    }
    public void ExitMenu()
    {
        menuDepth = 0;
    }
    public void OpenPause()
    {
        menuDepth = 1;
    }
    public void OpenOptions()
    {
        // opening options
        menuDepth = 2;
        AudioManager.Instance.UpdateSliders();
    }

    public void MainMenu()
    {       
        CloseMenus();
        ChangeScene("TitleScreen");   
    }

    public void ExitGame()
    {
        // Quit game
        Debug.Log("Saving game...");
        

        Debug.Log("Exiting...");
        Application.Quit();
        
    }

    public void ChangeScene(string name, float delay = 1.5f)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.FadeTo(0, delay, () => AudioManager.Instance.StopMusic());

        fader.FadeBlack(true);
        StartCoroutine(Wait(delay, () =>
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
            fader.FadeBlack(false);
            if (AudioManager.Instance != null)
                AudioManager.Instance.FadeTo(1, delay);
        }));
    }
    public void CloseMenus()
    {
        this.menuDepth = 0;
    }
    public static void NextFrame(Action a)
    {
        Instance.StartCoroutine(Instance.WaitFrame(a));
    }

    public static void WaitRoutine(float seconds, Action a)
    {
        Instance.StartCoroutine(Instance.Wait(seconds, a));
    }
    public IEnumerator Wait(float seconds, Action a)
    {
        yield return new WaitForSeconds(seconds);
        a.Invoke();
    }

    public IEnumerator WaitFrame(Action a)
    {
        yield return new WaitForEndOfFrame();
        a.Invoke();
    }

    // saving loading restoring...
    public void NewGame()
    {
        dataManager.NewGame();
    }
    public void SaveGame()
    {
        dataManager.SaveGame();
    }
    public void LoadGame()
    {
        dataManager.LoadGame();
    }
    /// <summary>
    /// Determines if the game can be loaded from an advanced state
    /// </summary>
    /// <returns>true if we can load the game from advance state, false otherwise</returns>
    public bool IsLoadable()
    {
        GameState gs = dataManager.GetLoadedData();
        return gs.DayNumber > 1 || gs.BerniceIntro;
    }

    /// <summary>
    /// Saves game and moves control to the map
    /// </summary>
    public void MapAndSave()
    {
        ChangeScene("Map");
        SaveGame();
    }
}
