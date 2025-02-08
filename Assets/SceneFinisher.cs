using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFinisher : MonoBehaviour
{
    public void finish()
    {
        GameManager.Instance.ChangeScene("Map");

    }
}
