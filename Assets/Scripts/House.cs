using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class House : MonoBehaviour, IHouse
{

    public Character thisHouse;
    public string[] sceneNames;
    [SerializeField] private bool isActive;
    public int nextScene;

    // Could update later to have the active and inactive sprites on one spritesheet
    // and switch between them with animation states
    private SpriteRenderer spriteRenderer;
    public Sprite inactiveSprite;
    public Sprite activeSprite;

    public Material outlineMat;
    public Material defaultMat;


    // Start is called before the first frame update
    void Start()
    {
        isActive = MapController.Instance.HouseStates[(int) thisHouse];
        nextScene = MapController.Instance.currentDay;
        spriteRenderer = GetComponent<SpriteRenderer>();
        updateSprite();
    }

    public void Click() {
        Debug.Log((int) thisHouse);
        MapController.Instance.HouseStates[(int) thisHouse] = false;
        MapController.Instance.LoadNextScene(sceneNames[nextScene]);
        MapController.Instance.UpdateDay();
    }

    public void setActiveStatus(bool s) {
        isActive = s;
        updateSprite();
    }

    private void updateSprite() {
        if (isActive)
            spriteRenderer.sprite = activeSprite;
        else
            spriteRenderer.sprite = inactiveSprite;
    }

    void OnMouseDown()
    {
        if (isActive) {
            Click();
        }
    }

    void OnMouseEnter() {
        if (isActive)
            spriteRenderer.material = outlineMat;
    }

    void OnMouseExit() {
        if (isActive)
            spriteRenderer.material = defaultMat;
    }
}
