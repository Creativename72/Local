using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteEnabler : MonoBehaviour
{
    private bool hasRun = false;
    public BackgroundHandler b;
    public AnnieMinigameChecker a;

    // Update is called once per frame
    void Update()
    {
        if (!hasRun)
        {
            if (b.currentBg == 2)
            {
                a.enableSprites();
                hasRun = true;
                this.enabled = false;
            }
        }
    }
}
