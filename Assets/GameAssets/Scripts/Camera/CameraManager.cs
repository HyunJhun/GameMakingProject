using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    static public CameraManager cameraManagerInstance;

    public GameObject player, npc;

    CinemachineBlendListCamera blendList;

    public GameObject mainCamObj;
    public GameObject dialogCamObj;
    public GameObject caveCamObj;
    public CinemachineVirtualCameraBase mainCam;
    public CinemachineVirtualCameraBase dialogCam;
    public CinemachineVirtualCameraBase caveCam;

    public bool isDialogRunning { get; set; } = false;

    // Start is called before the first frame update
    private void Awake()
    {
        #region declare Singledtone
        if (cameraManagerInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            cameraManagerInstance = this;
        }
        #endregion
    }

    void Start()
    {
        blendList = this.GetComponent<CinemachineBlendListCamera>();

        blendList.m_Loop = false;

        mainCam = mainCamObj.GetComponent<CinemachineVirtualCameraBase>();
        dialogCam = dialogCamObj.GetComponent<CinemachineVirtualCameraBase>();
        caveCam = caveCamObj.GetComponent<CinemachineVirtualCameraBase>();

        blendList.m_Instructions = new CinemachineBlendListCamera.Instruction[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCameraToTarget(Transform lookAtTransform,CinemachineVirtualCameraBase targetCam)
    {
        isDialogRunning = true;

        targetCam.GetComponent<CinemachineVirtualCamera>().Priority = 1;

        mainCamObj.transform.SetParent(this.transform);
        targetCam.transform.SetParent(this.transform);

        blendList.m_Instructions[0].m_VirtualCamera = mainCam;
        blendList.m_Instructions[1].m_VirtualCamera = targetCam;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.Cut;
        blendList.m_Instructions[1].m_Blend.m_Time = 1.0f;

        mainCam.LookAt = lookAtTransform.transform;
        targetCam.LookAt = lookAtTransform.transform;
    }
    public void SwitchCameraToMain(Transform lookAtTransform, CinemachineVirtualCameraBase fromCam)
    {
        

        fromCam.transform.SetParent(this.transform);
        mainCamObj.transform.SetParent(this.transform);

        blendList.m_Instructions[0].m_VirtualCamera = fromCam;
        blendList.m_Instructions[1].m_VirtualCamera = mainCam;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.HardOut;
        blendList.m_Instructions[1].m_Blend.m_Time = 2.0f;

        isDialogRunning = false;

        mainCam.LookAt = player.transform;
        fromCam.LookAt = lookAtTransform.transform;

        fromCam.GetComponent<CinemachineVirtualCamera>().Priority = 0;
    }
}
