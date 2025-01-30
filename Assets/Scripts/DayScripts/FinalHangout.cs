using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalHangout : MonoBehaviour
{
    [SerializeField] private DC dialogueController;
    // all the different text assets for the different endings
    [SerializeField] private TextAsset allBadText;
    [SerializeField] private TextAsset walterGoodText;
    [SerializeField] private TextAsset walterAnnieGoodText;
    [SerializeField] private TextAsset walterTylerGoodText;
    [SerializeField] private TextAsset annieGoodText;
    [SerializeField] private TextAsset annieTylerGoodText;
    [SerializeField] private TextAsset tylerGoodText;
    [SerializeField] private TextAsset allGoodText;
    [SerializeField] private DialogueCharacter scoutCreator;
    [SerializeField] private DialogueCharacter walterCreator;
    [SerializeField] private DialogueCharacter narratorCreator;
    [SerializeField] private AmbienceInScene ambienceInScene;
    [SerializeField] private BackgroundHandler backgroundHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
