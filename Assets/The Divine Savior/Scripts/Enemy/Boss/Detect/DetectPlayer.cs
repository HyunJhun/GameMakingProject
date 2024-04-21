using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public bool isDetectPlayer { get; set; }
    [SerializeField] private GameObject bossHpUI;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (bossHpUI != null)
            {
                if (!bossHpUI.activeSelf) bossHpUI.SetActive(true);
            }
            isDetectPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDetectPlayer)
            {
                isDetectPlayer = false;
            }
        }
    }

    
}
