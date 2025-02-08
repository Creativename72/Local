using System;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapIcon walterIcon;
    [SerializeField] private MapIcon annieIcon;
    [SerializeField] private MapIcon tylerIcon;
    [SerializeField] private MapIcon scoutIcon;
    [SerializeField] private PlayDialogueSFX audioPlayer;
    [SerializeField] private AudioClip clickSFX;

    // Start is called before the first frame update
    void Start()
    {
        int day = GameStateManager.Instance.GetFlag<int>("DayNumber");

        bool walterVisited = GameStateManager.Instance.GetFlag<bool>("WalterVisited");
        bool annieVisited = GameStateManager.Instance.GetFlag<bool>("AnnieVisited");
        bool tylerVisited = GameStateManager.Instance.GetFlag<bool>("TylerVisited");
        bool showScout = walterVisited && annieVisited && tylerVisited;
        bool visited = day != 1;

        // enabling highlights
        walterIcon.SetVisited(visited);
        annieIcon.SetVisited(visited);
        tylerIcon.SetVisited(visited);
        scoutIcon.SetVisited(true);

        // enabling icons
        walterIcon.EnableIcon(!walterVisited);
        annieIcon.EnableIcon(!annieVisited);
        tylerIcon.EnableIcon(!tylerVisited);
        scoutIcon.EnableIcon(showScout);


        // just go to the scene for default map icons
        walterIcon.OnClick(() =>
        {
            audioPlayer.PlayButtonSFX();
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(walterIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(walterIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(walterIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameStateManager"); break;
            }
            GameStateManager.Instance.SetFlag("WalterVisited", true);
        });

        annieIcon.OnClick(() =>
        {
            audioPlayer.PlayButtonSFX();
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(annieIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(annieIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(annieIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameStateManager"); break;
            }
            GameStateManager.Instance.SetFlag("AnnieVisited", true);
        });

        tylerIcon.OnClick(() =>
        {
            audioPlayer.PlayButtonSFX();
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(tylerIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(tylerIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(tylerIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameStateManager"); break;
            }
            GameStateManager.Instance.SetFlag("TylerVisited", true);
        });

        // change the day counter and unvisit everyone for visiting scout
        scoutIcon.OnClick(() =>
        {
            audioPlayer.PlayButtonSFX();
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(scoutIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(scoutIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(scoutIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameStateManager"); break;
            }

            GameStateManager.Instance.SetFlag("DayNumber", Math.Clamp(day + 1, 1, 3));
            GameStateManager.Instance.SetFlag("WalterVisited", false);
            GameStateManager.Instance.SetFlag("AnnieVisited", false);
            GameStateManager.Instance.SetFlag("TylerVisited", false);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
