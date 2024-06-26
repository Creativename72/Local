using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaytestMenuController : MonoBehaviour
{

    public string[] sceneNames;

    public Button[] sceneButtons;
    public Button prevButton;
    public Button nextButton;
    public GameObject menu;
    // public TextMeshPro pageCounter;

    [SerializeField] private int page;
    [SerializeField] private int lastNameIndex;
    [SerializeField] private int namesLength;
  
    // Start is called before the first frame update
    void Start()
    {
        MapController.Instance.LoadNextScene("BerniceIntro");
        page = 0;
        lastNameIndex = 0;
        namesLength = sceneNames.Length;

        NextPage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("tab")) {
            if (menu.activeInHierarchy == false) {
                menu.SetActive(true);
            }
            else {
                menu.SetActive(false);
            }
        }
    }

    public void NextPage() {
        int newIndex = lastNameIndex;

        for (int i = lastNameIndex; i < lastNameIndex + 5; i++) {
            if (i < sceneNames.Length) {
                if (sceneNames[i] != null) {
                    sceneButtons[i % 5].gameObject.SetActive(true);
                    sceneButtons[i % 5].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(sceneNames[i]);
                    newIndex++;
                }
            }
            else if (page < Mathf.Ceil(namesLength / 5f)) {
                sceneButtons[i % 5].gameObject.SetActive(false);
                newIndex++;
            }
        }

        lastNameIndex = newIndex;
        if (page < Mathf.Ceil(namesLength / 5f)) {
            page++;
        }
        if (page == Mathf.Ceil(namesLength / 5f)) {
            nextButton.gameObject.SetActive(false);
        }
        if (page > 1) {
            prevButton.gameObject.SetActive(true);
        }
    }

    public void PrevPage() {
        int newIndex = lastNameIndex;
        for (int i = lastNameIndex - 10; i < lastNameIndex - 5; i++) {
            sceneButtons[i % 5].gameObject.SetActive(true);
            sceneButtons[i % 5].gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(sceneNames[i]);
            newIndex--;
        }

        lastNameIndex = newIndex;
        if (page > 1) {
            page--;
        }
        if (page < Mathf.Ceil(namesLength / 5f)) {
            nextButton.gameObject.SetActive(true);
        }
        if (page == 1) {
            prevButton.gameObject.SetActive(false);
        }
    }

    public void GoToScene(TextMeshProUGUI buttonText) {
        MapController.Instance.LoadNextScene(buttonText.text);
    }
}
