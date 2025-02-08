using UnityEngine;

public class PlayDialogueSFX : MonoBehaviour
{
    [SerializeField] AudioClip buttonClip;

    public void PlaySFX(string n) {
        // Debug.Log("Play SFX: "+n);

        AudioManager.Instance.PlaySFX(n);
    }

    public void PlayButtonSFX() {
        AudioManager.Instance.PlaySFX(buttonClip);
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioManager.Instance.PlaySFX(clip);
    }
}
