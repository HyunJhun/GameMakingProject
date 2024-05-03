using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraHandler : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public float rotationSpeed;
    public Transform combatLookAt;

    private CameraStyle currentCam;
    public GameObject basicCam;
    public GameObject combatCam;
    public enum CameraStyle
    {
        Basic,
        LockOn
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentCam = CameraStyle.Basic;
    }

    private void Update()
    {     
        if (currentCam == CameraStyle.Basic)
        {
            if (basicCam.activeSelf == false && combatCam.activeSelf == true)
            {
                basicCam.SetActive(true);
                combatCam.SetActive(false);
            }
        }
        else if (currentCam == CameraStyle.LockOn)
        {
            if (combatCam.activeSelf == false && basicCam.activeSelf == true)
            {
                combatCam.SetActive(true);
                basicCam.SetActive(false);
            }
        }
        }

    public void CurrentStyleChanger()
    {
        if (currentCam == CameraStyle.Basic)
            currentCam = CameraStyle.LockOn;
        else
            currentCam = CameraStyle.Basic;
    }
    public Vector3 combatLook()
    {
        return combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
    }
}
