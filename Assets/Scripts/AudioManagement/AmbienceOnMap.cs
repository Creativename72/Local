using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbienceOnMap : MonoBehaviour
{
    private Dictionary<string, AudioClip> ambienceDic = new Dictionary<string, AudioClip>();
    private AudioSource ambiencePlayer;
    [SerializeField] string lastLoadedScene;
    [SerializeField] string[] sceneNames;
    [SerializeField] AudioClip[] ambiences;


    // Start is called before the first frame update
    void Start()
    {
        ambiencePlayer = GetComponent<AudioSource>();

        for (int i = 0; i < sceneNames.Length; i++) {
            if (i < ambiences.Length) {
                if (!ambienceDic.ContainsKey(sceneNames[i])) {
                    ambienceDic.Add(sceneNames[i], ambiences[i]);
                }
            }
        }

        lastLoadedScene = "TitleScreen";
    }

    public void MapAmbienceChange(string name) {
        if (name == "Map") {
            if (ambienceDic.ContainsKey(lastLoadedScene)) {
                ambiencePlayer.clip = ambienceDic[lastLoadedScene];
                ambiencePlayer.Play(0);
            }
        }
        else {
            lastLoadedScene = name;
            ambiencePlayer.Stop();
        }
    }
}
