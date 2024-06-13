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

    public void changeBackground()
    {
        currentBg++;
        if (currentBg >= bgs.Length)
        {
            currentBg -= bgs.Length;
        }
        s.sprite = bgs[currentBg];
    }
}
