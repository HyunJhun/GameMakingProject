using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [Header("Mat")]
    [SerializeField] private Material skyboxMat;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skyboxMat;
    }
}
