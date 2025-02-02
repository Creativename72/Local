using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class House : MonoBehaviour, IHouse
{
    public string gameFlag;
    public Character thisHouse;
    public string[] sceneNames;
    [SerializeField] private bool isActive;
    public int nextScene;

    // Could update later to have the active and inactive sprites on one spritesheet
    // and switch between them with animation states
    private SpriteRenderer spriteRenderer;
    public Sprite blankIcon;
    public Sprite fullIcon;


    /*  Originally the outline was made with a material that duplicated + offset the sprites in another color, but
        I changed it to toggling an outline gameobject because the map icon sprites don't have the space needed for
        the shader to work properly. Plus they're all circles, so all 
    public Material outlineMat;
    */
    public GameObject outlineGameObject;

    public Material highlightMat;
    public Material defaultMat;

    [SerializeField] RectTransform UILocation; //RectTransform of the MapIconPlaceholder object to correctly place map icons
    [SerializeField] AudioSource audioPlayer;


    // Start is called before the first frame update
    void Start()
    {
        //Place this icon at the location of UILocation
        if (UILocation != null)
        {
            this.gameObject.transform.localPosition = (UILocation.GetComponent<RectTransform>().position);
            Debug.Log(this.gameObject.transform.localPosition);
        }

        //Debug.Log(string.Join(",", MapController.Instance.HouseStates));
        isActive = MapController.Instance.HouseStates[(int) thisHouse];
        nextScene = MapController.Instance.currentStage;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioPlayer = GetComponent<AudioSource>();
        updateSprite();
    }

    public void Click() {
        audioPlayer.Play(0);
        MapController.Instance.HouseStates[(int) thisHouse] = false;
        GameManager.Instance.ChangeScene(sceneNames[nextScene]);
        MapController.Instance.UpdateDay();
    }

    public void setActiveStatus(bool s) {
        isActive = s;
        updateSprite();
    }

    private void updateSprite() {
        if (isActive) {
            if (MapController.Instance.currentStage < 2) {
                spriteRenderer.sprite = blankIcon;
            }
            else
                spriteRenderer.sprite = fullIcon;
        }
        else
            spriteRenderer.sprite = null;
    }

    void OnMouseDown()
    {
        if (isActive) {
            Click();
        }
    }

    void OnMouseEnter() {
        if (isActive) {
            // spriteRenderer.material = outlineMat;
            outlineGameObject.SetActive(true);
            spriteRenderer.color = new Color(170f, 170f, 170f, 1f);
            spriteRenderer.material = highlightMat;
        }
    }

    void OnMouseExit() {
        if (isActive) {
            spriteRenderer.material = defaultMat;
            outlineGameObject.SetActive(false);
            spriteRenderer.color = new Color(255f, 255f, 255f, 1f);
        }
    }
}
