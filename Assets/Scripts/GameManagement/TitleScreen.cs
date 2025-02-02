using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadable;
    [SerializeField] private GameObject newGame;
    [SerializeField] private GameObject confirm;

    private bool load;
    private bool confirmActive;
    //Quits the game
    public void StartGameButton()
    {
        GameManager.Instance.NewGame();
        GameManager.Instance.ChangeScene("BerniceIntro");
        GameManager.Instance.SaveGame();
    }

    public void ConfirmStartGameButton()
    {
        confirmActive = true;
    }

    public void ExitConfirm()
    {
        confirmActive = false;
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void LoadSaveButton()
    {
        GameManager.Instance.LoadGame();
        GameManager.Instance.ChangeScene("Map");
        GameManager.Instance.SaveGame();
    }

    public void Start()
    {
        // only enable the load game button if there is a save file
        load = GameManager.Instance.IsLoadable();
        confirmActive = false;
    }

    public void Update()
    {
        confirm.SetActive(confirmActive);
        loadable.SetActive(load && !confirmActive);
        newGame.SetActive(!load);
    }
}
