using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoomer : MonoBehaviour
{
    [SerializeField] private List<Transform> zoomLocations;
    [SerializeField] private Animator animator;
    public void ZoomTo(int location)
    {
        if (location < 0 || location >= zoomLocations.Count)
        {
            animator.SetTrigger("reset");
            animator.ResetTrigger("reset");
        }

        animator.SetInteger("location", location);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
