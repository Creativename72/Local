using UnityEngine;

/**
 * Does the tyler minigame.
 * Known problems: 
 * -- the text is overlapping on some dialogue boxes.
 */
public class TylerDay2Manager : MonoBehaviour
{
    [SerializeField] private GameObject pointsOfInterest;
    [SerializeField] private GameObject bike;
    [SerializeField] private GameObject grill;
    [SerializeField] private GameObject guitar;
    [SerializeField] private GameObject sculpture;
    [SerializeField] private GameObject graffiti;
    [SerializeField] private GameObject continueButton;

    [SerializeField] private TextAsset tylerDay2Text;
    [SerializeField] private TextAsset tylerDay2TextGraffiti;
    [SerializeField] private TextAsset tylerDay2TextGrill;
    [SerializeField] private TextAsset tylerDay2TextSculpture;
    [SerializeField] private TextAsset tylerDay2TextBike;
    [SerializeField] private TextAsset tylerDay2TextGuitar;
    [SerializeField] private TextAsset tylerDay2TextPostItem;

    [SerializeField] private DialogueController dialogueController;

    public int pointsInteracted;
    private State currentState;
    //private bool continueClickedRun = false;

    // public AudioClip sceneMusic; //music for this scene

    public void ContinueClicked()
    {
        dialogueController.end = true;
        dialogueController.t = tylerDay2TextPostItem;
        dialogueController.runDialogue();
        currentState = State.PostDialogue;
    }
    private enum State
    {
        IntroDialogue,
        Minigame,
        PostDialogue,
    }

    // Start is called before the first frame update
    void Start()
    {
        // PlaySceneMusic(); //plays scene music

        pointsOfInterest.SetActive(false);
        continueButton.SetActive(false);
        pointsInteracted = 0;
        currentState = State.IntroDialogue;
        //dialogueController.t = tylerDay2Text; 
        //dialogueController.runDialogue();

        // set actions to occur for each object
        bike.GetComponent<HighlightableObject>().OnClick(() =>
        {
            if (!dialogueController.dialogueRunning)
            {
                dialogueController.t = tylerDay2TextBike;
                dialogueController.runDialogue();
                bike.SetActive(false);
                pointsInteracted++;
            }
        });
        grill.GetComponent<HighlightableObject>().OnClick(() =>
        {
            if (!dialogueController.dialogueRunning)
            {
                dialogueController.t = tylerDay2TextGrill;
                dialogueController.runDialogue();
                grill.SetActive(false);
                pointsInteracted++;
            }
        });
        guitar.GetComponent<HighlightableObject>().OnClick(() =>
        {
            if (!dialogueController.dialogueRunning)
            {
                dialogueController.t = tylerDay2TextGuitar;
                dialogueController.runDialogue();
                guitar.SetActive(false);
                pointsInteracted++;
            }
        });
        sculpture.GetComponent<HighlightableObject>().OnClick(() =>
        {
            if (!dialogueController.dialogueRunning)
            {
                dialogueController.t = tylerDay2TextSculpture;
                dialogueController.runDialogue();
                sculpture.SetActive(false);
                pointsInteracted++;
            }
        });
        graffiti.GetComponent<HighlightableObject>().OnClick(() =>
        {
            if (!dialogueController.dialogueRunning)
            {
                dialogueController.t = tylerDay2TextGraffiti;
                dialogueController.runDialogue();
                graffiti.SetActive(false);
                pointsInteracted++;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.IntroDialogue && !dialogueController.dialogueRunning)
        {
            currentState = State.Minigame;
            pointsOfInterest.SetActive(true);
        }
        else if (currentState == State.Minigame && !dialogueController.dialogueRunning && pointsInteracted >= 5)
        {
            ContinueClicked();
        }
    }


    /*
    //Only for scene music
    //Finds AudioManager singleton and plays music associated with this scene
    private void PlaySceneMusic()
    {
        if (sceneMusic != null)
        {
            GameObject musicObj = GameObject.Find("AudioManager");
            if (musicObj != null)
            {
                AudioManager musicAudioSource = musicObj.GetComponent<AudioManager>();
                if (musicAudioSource != null)
                {
                    musicAudioSource.PlayMusic(sceneMusic);
                }
            }
        }
    }
    */
}
