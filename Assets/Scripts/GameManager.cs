using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject menuArea;
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
            Destroy(this);
        } else
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
            Debug.Log("CLICKED OFF PAUSE MENU");
        }
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
        // Change scene to main menu
        Debug.Log("MAIN MENU");
    }

    public void ExitGame()
    {
        // Quit game
        Application.Quit();
        Debug.Log("EXIT GAME");
    }
}
