using Cinemachine;
using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading;
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
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;

    private float timeToLerp = 2f;

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


    public async UniTask SetCameraOrbitToBossFlightPattern(CancellationToken cts = default)
    {
        while (thirdPersonCamera.m_Lens.FieldOfView < 59.5f)
        {
            thirdPersonCamera.m_Orbits[0].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Height, 7f, Time.deltaTime / timeToLerp);
            thirdPersonCamera.m_Orbits[0].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Radius, 5f, Time.deltaTime / timeToLerp);

            thirdPersonCamera.m_Orbits[1].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Height, 3f, Time.deltaTime / timeToLerp);
            thirdPersonCamera.m_Orbits[1].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Radius, 10f, Time.deltaTime / timeToLerp);

            thirdPersonCamera.m_Orbits[2].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Height, 1.5f, Time.deltaTime / timeToLerp);
            thirdPersonCamera.m_Orbits[2].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Radius, 6f, Time.deltaTime / timeToLerp);

            thirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(thirdPersonCamera.m_Lens.FieldOfView, 60f, Time.deltaTime / timeToLerp);


            await UniTask.Yield(PlayerLoopTiming.Update, cts);
        }
    }
    //public IEnumerator SetCameraOrbitToBossFlightPattern()
    //{
    //    while (thirdPersonCamera.m_Lens.FieldOfView < 59.5f)
    //    {
    //        thirdPersonCamera.m_Orbits[0].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Height, 7f, Time.deltaTime / timeToLerp);
    //        thirdPersonCamera.m_Orbits[0].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Radius, 5f, Time.deltaTime / timeToLerp);

    //        thirdPersonCamera.m_Orbits[1].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Height, 3f, Time.deltaTime / timeToLerp);
    //        thirdPersonCamera.m_Orbits[1].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Radius, 10f, Time.deltaTime / timeToLerp);

    //        thirdPersonCamera.m_Orbits[2].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Height, 1.5f, Time.deltaTime / timeToLerp);
    //        thirdPersonCamera.m_Orbits[2].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Radius, 6f, Time.deltaTime / timeToLerp);

    //        thirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(thirdPersonCamera.m_Lens.FieldOfView, 60f, Time.deltaTime / timeToLerp);

    //        yield return null;
    //    }
    //}
    public IEnumerator SetCameraOrbitToBossGroundPattern()
    {
        float timer = 0f;

        while (thirdPersonCamera.m_Lens.FieldOfView > 50.5f)
        {
            thirdPersonCamera.m_Orbits[0].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Height, 6f, Time.deltaTime / timeToLerp);
            thirdPersonCamera.m_Orbits[0].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[0].m_Radius, 3f, Time.deltaTime / timeToLerp);

            thirdPersonCamera.m_Orbits[1].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Height, 2f, Time.deltaTime / timeToLerp);
            thirdPersonCamera.m_Orbits[1].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[1].m_Radius, 7f, Time.deltaTime / timeToLerp);

            thirdPersonCamera.m_Orbits[2].m_Height = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Height, 1f, Time.deltaTime / timeToLerp);
            thirdPersonCamera.m_Orbits[2].m_Radius = Mathf.Lerp(thirdPersonCamera.m_Orbits[2].m_Radius, 5f, Time.deltaTime / timeToLerp);

            thirdPersonCamera.m_Lens.FieldOfView = Mathf.Lerp(thirdPersonCamera.m_Lens.FieldOfView, 50f, Time.deltaTime / timeToLerp);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void InvokeCameraToFlight()
    {
        SetCameraOrbitToBossFlightPattern(this.GetCancellationTokenOnDestroy()).Forget();
    }
    public void InvokeCameraToGround()
    {
        StartCoroutine(SetCameraOrbitToBossGroundPattern());
    }
}
