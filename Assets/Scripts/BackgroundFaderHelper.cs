using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Helper class for letting background manager prefabs fade between backgrounds w/ a SpriteRenderer
public class BackgroundFaderHelper : MonoBehaviour
{
    [SerializeField] bool fadeBlack;

    // Animator on the background fader
    [SerializeField] Animator anim;


    public void FadeBlack(bool value) {
        fadeBlack = value;
        if (anim != null)
        {
            anim.SetBool("FadeBlack", value);
        }
    }

    public void SetVisible()
    {
        anim.SetBool("FadeBlack", false);
        anim.SetTrigger("SetVisible");
        anim.ResetTrigger("SetVisible");
    }
}
