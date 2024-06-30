using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayDialogueSFX : MonoBehaviour
{
    [SerializeField] AudioSource audioPlayer;


    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    public void PlaySFX(string n) {
        // Debug.Log("Play SFX: "+n);
        audioPlayer.PlayOneShot(Resources.Load<AudioClip>("SFX/"+n), 1f);
    }
}
