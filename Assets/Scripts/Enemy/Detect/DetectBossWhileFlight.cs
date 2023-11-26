using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBossWhileFlight : MonoBehaviour
{
    public bool isTriggered { get; set; } 
    void Start()
    {
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            isTriggered = true;
        }
    }
}
