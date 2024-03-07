using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBossCollision : MonoBehaviour
{
    public bool isCollisionWithPlayer { get; set; } 
    void Start()
    {
        isCollisionWithPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) isCollisionWithPlayer = true;
    }
}
