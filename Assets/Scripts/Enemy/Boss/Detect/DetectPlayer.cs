using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public bool isDetectPlayer { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어가 몬스터의 공격 범위 안에 들어왓습니다.");
            isDetectPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isDetectPlayer)
            {
                Debug.Log("플레이어가 몬스터의 공격 범위를 벗어났습니다.");
                isDetectPlayer = false;
            }
        }
    }

    
}
