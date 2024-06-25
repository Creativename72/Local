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

    private int pointsInteracted;
    private State currentState;
    private bool continueClicked;
    //private bool continueClickedRun = false;

    // public AudioClip sceneMusic; //music for this scene

    public void ContinueClicked()
    {
        dialogueController.end = true;
        continueClicked = true;
        dialogueController.t = tylerDay2TextPostItem;
        dialogueController.runDialogue();
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
        continueClicked = false;
        currentState = State.IntroDialogue;
        dialogueController.t = tylerDay2Text;
        dialogueController.runDialogue();

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
        else if (currentState == State.Minigame && !dialogueController.dialogueRunning && pointsInteracted >= 4)
        {
            continueButton.SetActive(true);
        }
        else if (currentState == State.Minigame && dialogueController.dialogueRunning)
        {
            continueButton.SetActive(false);
        }
        else if (continueClicked)
        {
            Debug.Log("something please");
            dialogueController.end = true;
            currentState = State.PostDialogue;
            dialogueController.t = tylerDay2TextPostItem;
            dialogueController.runDialogue();
            continueButton.SetActive(false);
            //continueClickedRun = true;
            
        }
        else if (currentState == State.PostDialogue && !dialogueController.dialogueRunning)
        {
            //MapController.Instance.LoadNextScene("Map");
            //not working rn, have workaround
        }
    }


    /*
    //Only for scene music
    //Finds MusicPlayer singleton and plays music associated with this scene
    private void PlaySceneMusic()
    {
        if (sceneMusic != null)
        {
            GameObject musicObj = GameObject.Find("MusicPlayer");
            if (musicObj != null)
            {
                MusicPlayer musicPlayer = musicObj.GetComponent<MusicPlayer>();
                if (musicPlayer != null)
                {
                    musicPlayer.PlayMusic(sceneMusic);
                }
            }
        }
    }
    */
}
