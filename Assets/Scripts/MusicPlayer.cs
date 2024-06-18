using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //PLACE THE PREFAB OF THE MUSIC PLAYER IN THE FIRST SCENE OF THE GAME
    private static MusicPlayer instance; //singleton instance of the music player

    AudioSource musicPlayer; //persistent musicPlayer

    // Start is called before the first frame update
    void Start()
    {
        //verify that this object is the singleton instance, otherwise delete
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        musicPlayer = GetComponent<AudioSource>();    
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicPlayer != null && !musicPlayer.isPlaying) 
        {
            musicPlayer.clip = clip;
            musicPlayer.Play();
        }else if(musicPlayer.clip != clip)
        {
            StartCoroutine(ChangeMusic(clip, 1.0f, 0f, 1.0f));
            //StopMusic(clip);
        }
    }

    public void StopMusic(AudioClip newClip)
    {
        StartCoroutine(ChangeMusic(newClip, 2.0f, 0f, 1.0f));
    }

    //Coroutine to fade music
    IEnumerator ChangeMusic(AudioClip newClip, float duration, float targetVolume, float resetVolume)
    {
        float currentTime = 0;
        float start = musicPlayer.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicPlayer.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        musicPlayer.Stop();
        musicPlayer.volume = resetVolume;
        musicPlayer.clip = newClip;
        musicPlayer.Play();
        yield break;
    }
}
