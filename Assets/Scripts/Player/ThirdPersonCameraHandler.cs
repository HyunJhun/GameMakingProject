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
        // 회전 방향 설정
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // 유저 회전 설정
        if (currentCam == CameraStyle.Basic)
        {
            if (basicCam.activeSelf == false && combatCam.activeSelf == true)
            {
                basicCam.SetActive(true);
                combatCam.SetActive(false);
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * v + orientation.right * h;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentCam == CameraStyle.LockOn)
        {
            if (combatCam.activeSelf == false && basicCam.activeSelf == true)
            {
                combatCam.SetActive(true);
                basicCam.SetActive(false);
            }
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
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
