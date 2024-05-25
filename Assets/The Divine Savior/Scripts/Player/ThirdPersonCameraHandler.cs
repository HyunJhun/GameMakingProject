using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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

    [Header("Boss Cam")]
    [SerializeField]private CinemachineFreeLook thirdPersonCamera;

    private float timeToLerp = 3f;

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

        thirdPersonCamera = GameObject.Find("ThirdPersonCamera").GetComponent<CinemachineFreeLook>();
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

    public IEnumerator SetCameraOrbitToBossFlightPattern()
    {
        
        thirdPersonCamera.m_Orbits[0].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Height, 7f, Time.deltaTime / timeToLerp);
        thirdPersonCamera.m_Orbits[0].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Radius, 5f, Time.deltaTime / timeToLerp);

        thirdPersonCamera.m_Orbits[1].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Height, 3f, Time.deltaTime / timeToLerp);
        thirdPersonCamera.m_Orbits[1].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Radius, 10f, Time.deltaTime / timeToLerp);

        thirdPersonCamera.m_Orbits[2].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Height, 1.5f, Time.deltaTime / timeToLerp);
        thirdPersonCamera.m_Orbits[2].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Radius, 6f, Time.deltaTime / timeToLerp);

        yield return null;
    }
    public IEnumerator SetCameraOrbitToBossGroundPattern()
    {
        thirdPersonCamera.m_Orbits[0].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Height, 6f, Time.deltaTime / timeToLerp);
        thirdPersonCamera.m_Orbits[0].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Radius, 3f, Time.deltaTime / timeToLerp);

        thirdPersonCamera.m_Orbits[1].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Height, 2f, Time.deltaTime / timeToLerp);
        thirdPersonCamera.m_Orbits[1].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Radius, 7f, Time.deltaTime / timeToLerp);

        thirdPersonCamera.m_Orbits[2].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Height, 1f, Time.deltaTime / timeToLerp);
        thirdPersonCamera.m_Orbits[2].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Radius, 5f, Time.deltaTime / timeToLerp);
        yield return null;
    }

    public void InvokeCameraToFlight()
    {
        
    }
}
