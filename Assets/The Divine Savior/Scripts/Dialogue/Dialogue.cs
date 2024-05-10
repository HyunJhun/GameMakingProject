using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable] // 직접 만든 클래스에 접근할 수 있도록 해줌
public class Dialog
{
    [TextArea] // 한 줄 말고 여러 줄 사용가능하게 해주는 거래
    public string text;
    public string name;


}
public class Dialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TMP_Text objName;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Dialog[] dialogs;

    [Header("Volume")]
    [SerializeField] private Volume volume;
    int m_countOfDialog = 0;
    bool isTextPrintComplete = false;

    public void OnOffDialogUI(bool state)
    {
        dialogueUI.SetActive(state);
    }

    public IEnumerator ShowTextInOrder()
    {
        //if (objName == null || dialogText == null) yield break;
        m_countOfDialog = 0;
        OnOffDialogUI(true);

        Transform npcTransform = GameObject.FindGameObjectWithTag("NPC").transform;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        CameraManager.cameraManagerInstance.SwitchCameraToSub(npcTransform);
        Debug.Log("됏나?");

        while(m_countOfDialog < dialogs.Length)
        {
            if(dialogText.text.Length != dialogs[m_countOfDialog].text.Length)
            {
                objName.text = dialogs[m_countOfDialog].name;
                for (int count = 0; count < dialogs[m_countOfDialog].text.Length; count++)
                {
                    dialogText.text += dialogs[m_countOfDialog].text[count];
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (!isTextPrintComplete)
                        {
                            dialogText.text = dialogs[m_countOfDialog].text;
                            isTextPrintComplete = true;
                        }
                        else
                        {
                            dialogText.text = "";
                            m_countOfDialog++;
                            isTextPrintComplete = false;
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }      
            else if(dialogText.text.Length == dialogs[m_countOfDialog].text.Length)
            {
                dialogText.text = "";
                isTextPrintComplete = false;
                m_countOfDialog++;
            }
        }
        OnOffDialogUI(false);

        CameraManager.cameraManagerInstance.SwitchCameraToMain(playerTransform);
        volume.isConversationStart = false;

        Debug.Log("대화 완료");
    }
}
