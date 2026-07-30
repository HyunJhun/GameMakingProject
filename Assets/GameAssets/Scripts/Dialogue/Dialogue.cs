using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [TextArea]
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

    [Header("Text Settings")]
    [SerializeField, Min(0.01f)] private float characterInterval = 0.1f;

    [Header("Volume")]
    [SerializeField] private Volume volume;

    private int currentDialogIndex;
    private bool isTextPrintComplete;

    public void OnOffDialogUI(bool state)
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(state);
        }
    }

    public async UniTask ShowTextInOrder(CancellationToken ct = default)
    {
        if (objName == null ||
            dialogText == null ||
            dialogs == null ||
            dialogs.Length == 0)
        {
            return;
        }

        currentDialogIndex = 0;
        isTextPrintComplete = false;
        objName.text = string.Empty;
        dialogText.text = string.Empty;

        OnOffDialogUI(true);

        GameObject npcObject = GameObject.FindGameObjectWithTag("NPC");
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        Transform npcTransform = npcObject != null ? npcObject.transform : null;
        Transform playerTransform = playerObject != null ? playerObject.transform : null;

        try
        {
            if (CameraManager.cameraManagerInstance != null && npcTransform != null)
            {
                CameraManager.cameraManagerInstance.SwitchCameraToTarget(
                    npcTransform,
                    CameraManager.cameraManagerInstance.dialogCam
                );
            }

            while (currentDialogIndex < dialogs.Length)
            {
                ct.ThrowIfCancellationRequested();

                Dialog currentDialog = dialogs[currentDialogIndex];

                if (currentDialog == null)
                {
                    currentDialogIndex++;
                    continue;
                }

                string currentText = currentDialog.text ?? string.Empty;

                objName.text = currentDialog.name ?? string.Empty;
                dialogText.text = string.Empty;
                isTextPrintComplete = false;

                int characterIndex = 0;
                float elapsedTime = characterInterval;

                // 현재 대사를 한 글자씩 출력한다.
                while (!isTextPrintComplete)
                {
                    ct.ThrowIfCancellationRequested();

                    // 출력 중 E 또는 Space를 누르면 현재 대사를 즉시 전부 표시한다.
                    if (IsNextKeyPressed())
                    {
                        dialogText.text = currentText;
                        isTextPrintComplete = true;

                        // 같은 키 입력이 바로 다음 대사로 넘기는 데 사용되지 않도록 한다.
                        await UniTask.Yield(PlayerLoopTiming.Update, ct);
                        break;
                    }

                    elapsedTime += Time.unscaledDeltaTime;

                    if (elapsedTime >= characterInterval)
                    {
                        elapsedTime = 0f;

                        if (characterIndex < currentText.Length)
                        {
                            dialogText.text += currentText[characterIndex];
                            characterIndex++;
                        }

                        if (characterIndex >= currentText.Length)
                        {
                            isTextPrintComplete = true;
                        }
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }

                // 현재 대사가 모두 출력되면, 다음 입력이 들어올 때까지 기다린다.
                while (!IsNextKeyPressed())
                {
                    ct.ThrowIfCancellationRequested();
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }

                currentDialogIndex++;

                // 다음 대사로 넘긴 키 입력이 같은 프레임에 다시 감지되어
                // 새 대사를 즉시 완성하지 않도록 입력 프레임을 분리한다.
                if (currentDialogIndex < dialogs.Length)
                {
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }
            }
        }
        finally
        {
            dialogText.text = string.Empty;
            objName.text = string.Empty;
            OnOffDialogUI(false);

            if (CameraManager.cameraManagerInstance != null && playerTransform != null)
            {
                CameraManager.cameraManagerInstance.SwitchCameraToMain(
                    playerTransform,
                    CameraManager.cameraManagerInstance.dialogCam
                );
            }

            if (volume != null)
            {
                volume.isConversationStart = false;
            }

            Debug.Log("대화 완료");
        }
    }

    private bool IsNextKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.E) ||
               Input.GetKeyDown(KeyCode.Space);
    }
}
