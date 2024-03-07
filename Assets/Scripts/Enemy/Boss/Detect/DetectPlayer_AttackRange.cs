using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer_AttackRange : MonoBehaviour
{
    private Status player;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Status>(); // 만약 공격 사거리 안에 들어와 있다면 정보를 가져옴
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                player = null; // 기존 정보를 초기화시켜줌
            }
        }
    }

    public Status getPlayerStatusForDamaged() // 공격 직전에 플레이어가 공격범위를 벗어나게 되도 버그가 발생하지 않도록
    {
        if (player != null) 
            return player.GetComponent<Status>();
        else 
            return null;
    }
}
