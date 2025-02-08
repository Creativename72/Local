using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(audioClip);
    }
}
