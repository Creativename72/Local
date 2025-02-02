using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endingText;
    // Start is called before the first frame update
    void Start()
    {
        bool goodEnding =
            GameStateManager.Instance.GetFlag<bool>("WalterGoodEnding") &&
            GameStateManager.Instance.GetFlag<bool>("AnnieGoodEnding") &&
            GameStateManager.Instance.GetFlag<bool>("TylerGoodEnding");

        endingText.text = goodEnding ? "Good ending." : "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameManager.Instance.ChangeScene("TitleScreen");
            GameManager.Instance.NewGame();
        }
    }
}
