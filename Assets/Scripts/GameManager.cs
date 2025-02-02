using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject menuArea;
    [SerializeField] private AudioMixer audioFader;
    [SerializeField] private FadeScript fader;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
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
    }

    // Update is called once per frame
    void Update()
    {
        escapeMenu.SetActive(menuDepth == 1 || menuDepth == 2);
        pauseMenu.SetActive(menuDepth == 1);
        optionsMenu.SetActive(menuDepth == 2);
        gameVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuDepth == 0)
                menuDepth++;
            else
                menuDepth--;
        }
        if (Input.GetButtonDown("Fire1") && !menuArea.GetComponent<Collider2D>().OverlapPoint(Input.mousePosition))
        {
            menuDepth = 0;
            // Debug.Log("CLICKED OFF PAUSE MENU");
        }
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

    public void Options()
    {
        // opening options
        menuDepth++;
        Debug.Log("OPTIONS");
    }

    public void MainMenu()
    {
        Debug.Log("Saving game...");
       
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

    public void ChangeScene(string name)
    {
        ChangeScene(name, 1.5f);
    }
    public void ChangeScene(string name, float delay = 1.5f)
    {
        if (MusicPlayer.Instance != null)
            MusicPlayer.Instance.StopMusic(null);

        fader.FadeBlack(true);
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "MusicVolume", delay, 0f));
        StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", delay, 0f));
        StartCoroutine(Wait(delay, () =>
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
            fader.FadeBlack(false);
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "MusicVolume", delay, 1f));
            StartCoroutine(FadeMixerGroup.StartFade(audioFader, "AmbienceVolume", delay, 1f));
        }));
    }
    public void CloseMenus()
    {
        this.menuDepth = 0;
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
