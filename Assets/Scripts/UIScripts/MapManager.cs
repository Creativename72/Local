using System;
using System.Collections;
using System.Collections.Generic;
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
        int day = GameFlags.GetFlag<int>("DayNumber");

        bool walterVisited = GameFlags.GetFlag<bool>("WalterVisited");
        bool annieVisited = GameFlags.GetFlag<bool>("AnnieVisited");
        bool tylerVisited = GameFlags.GetFlag<bool>("TylerVisited");
        bool showScout = walterVisited && annieVisited && tylerVisited;

        // enabling highlights
        walterIcon.EnableClick(!walterVisited);
        annieIcon.EnableClick(!annieVisited);
        tylerIcon.EnableClick(!tylerVisited);
        scoutIcon.EnableClick(showScout);

        // enabling icons
        walterIcon.EnableIcon(!showScout);
        annieIcon.EnableIcon(!showScout);
        tylerIcon.EnableIcon(!showScout);
        scoutIcon.EnableIcon(showScout);


        // just go to the scene for default map icons
        walterIcon.OnClick(() =>
        {
            audioPlayer.PlaySFX(clickSFX);
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(walterIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(walterIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(walterIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameFlags"); break;
            }
            GameFlags.SetFlag("WalterVisited", true);
        });

        annieIcon.OnClick(() =>
        {
            audioPlayer.PlaySFX(clickSFX);
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(annieIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(annieIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(annieIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameFlags"); break;
            }
            GameFlags.SetFlag("AnnieVisited", true);
        });

        tylerIcon.OnClick(() =>
        {
            audioPlayer.PlaySFX(clickSFX);
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(tylerIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(tylerIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(tylerIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameFlags"); break;
            }
            GameFlags.SetFlag("TylerVisited", true);
        });

        // change the day counter and unvisit everyone for visiting scout
        scoutIcon.OnClick(() =>
        {
            audioPlayer.PlaySFX(clickSFX);
            switch (day)
            {
                case 1: GameManager.Instance.ChangeScene(scoutIcon.day1Scene); break;
                case 2: GameManager.Instance.ChangeScene(scoutIcon.day2Scene); break;
                case 3: GameManager.Instance.ChangeScene(scoutIcon.day3Scene); break;
                default: Debug.LogWarning("Invalid Day Number in GameFlags"); break;
            }

            GameFlags.SetFlag("DayNumber", Math.Clamp(day + 1, 1, 3));
            GameFlags.SetFlag("WalterVisited", false);
            GameFlags.SetFlag("AnnieVisited", false);
            GameFlags.SetFlag("TylerVisited", false);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
