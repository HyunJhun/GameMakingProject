using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    [SerializeField] private GameObject conversationUI;
    public Dialogue dialogue;
    public bool isPlayerTriggered { get; set; } = false;
    public bool isConversationStart { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        conversationUI = GameObject.Find("Conversation");
        conversationUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerTriggered) isPlayerTriggered = true;
            if (!conversationUI.activeSelf) conversationUI.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isConversationStart)
                {
                    Debug.Log("대화시작");
                    isConversationStart = true;
                    StartCoroutine(dialogue.ShowTextInOrder());
                }

            }
            if (CameraManager.cameraManagerInstance.isDialogRunning) conversationUI.SetActive(false);
            else conversationUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPlayerTriggered) isPlayerTriggered = false;
            if (conversationUI.activeSelf) conversationUI.SetActive(false);
        }
    }
}
