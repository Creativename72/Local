using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BerniceHoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image toHighlight;
    public Color defaultColor;
    public Color highlightColor;
    public int optionNumber;
    public BerniceDialogueController parent;


    public void OnPointerEnter(PointerEventData eventData)
    {
        toHighlight.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toHighlight.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        parent.currentScene.chooseOption(optionNumber);
    }
}
