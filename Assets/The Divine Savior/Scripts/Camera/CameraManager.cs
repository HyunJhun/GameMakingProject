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
    CinemachineVirtualCameraBase mainCam;
    CinemachineVirtualCameraBase dialogCam;

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

        blendList.m_Instructions = new CinemachineBlendListCamera.Instruction[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCameraToSub(Transform lookAtTransform)
    {
        isDialogRunning = true;

        mainCamObj.transform.SetParent(this.transform);
        dialogCamObj.transform.SetParent(this.transform);


        Debug.Log(blendList.m_Instructions.Length);
        Debug.Log(blendList.m_Instructions[0]);
        blendList.m_Instructions[0].m_VirtualCamera = mainCam;
        blendList.m_Instructions[1].m_VirtualCamera = dialogCam;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.Cut;
        blendList.m_Instructions[1].m_Blend.m_Time = 1.0f;

        //blendList.m_Instructions[0].m_Hold = 1.0f;

        mainCam.LookAt = lookAtTransform.transform;
        dialogCam.LookAt = lookAtTransform.transform;

    }

    public void SwitchCameraToMain(Transform lookAtTransform)
    {
        dialogCamObj.transform.SetParent(this.transform);
        mainCamObj.transform.SetParent(this.transform);

        blendList.m_Instructions[0].m_VirtualCamera = dialogCam;
        blendList.m_Instructions[1].m_VirtualCamera = mainCam;

        blendList.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.HardOut;
        blendList.m_Instructions[1].m_Blend.m_Time = 2.0f;

        blendList.m_Instructions[0].m_Hold = 1.0f;

        isDialogRunning = false;

        mainCam.LookAt = lookAtTransform.transform;
        dialogCam.LookAt = lookAtTransform.transform;

        
    }
}
