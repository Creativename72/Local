using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHandler : MonoBehaviour
{
    public SpriteRenderer s;
    [SerializeField]
    public Sprite[] bgs;
    public int currentBg;
    public bool b;
    
    [SerializeField] BackgroundFaderHelper bgFader;

    private void Start()
    {
        currentBg = 0;
        s.sprite = bgs[currentBg];
    }

    private void Update()
    {
        if (b)
        {
            b = false;
            changeBackground();
        }
    }

    public void changeBackground(bool doFade = true)
    {
        StartCoroutine(backgroundChange(doFade));
    }

    private IEnumerator backgroundChange(bool doFade) {
        if (doFade)
        {
            bgFader.Fade();

            yield return new WaitForSeconds(0.75f);
        }
        currentBg++;
        if (currentBg >= bgs.Length)
        {
            currentBg = bgs.Length - 1;
        }
        s.sprite = bgs[currentBg];
        if (doFade)
        {
            bgFader.Fade();
        }
    }
}
