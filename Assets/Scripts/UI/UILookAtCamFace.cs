using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtCamFace : MonoBehaviour
{
    private Camera mainCam;


    private void Start()
    {
        mainCam = GameObject.Find("Player Camera").GetComponent<Camera>();
    }
    void Update()
    {
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
    }
}
