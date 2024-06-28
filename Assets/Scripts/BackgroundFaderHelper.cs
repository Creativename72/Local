using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class for letting background manager prefabs fade between backgrounds w/ a SpriteRenderer
public class BackgroundFaderHelper : MonoBehaviour
{
    [SerializeField] bool fadeBlack;

    // Animator on the background fader
    [SerializeField] Animator anim;


    public void Fade() {
        fadeBlack = !fadeBlack;
        if(anim != null)
        {
            anim.SetBool("FadeBlack", fadeBlack);
        }
    }
}
