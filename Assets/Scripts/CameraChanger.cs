using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChanger : MonoBehaviour
{
    public GameObject[] cameraObjects;
    private int currentCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeCamera() {
        cameraObjects[currentCam + 1].SetActive(true);
        cameraObjects[currentCam].SetActive(false);
        currentCam++;
    }
}
