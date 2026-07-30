using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TeleportManager : MonoBehaviour
{
    [Header("Teleport Portals")]
    [SerializeField] private GameObject MainToDungeonPortal;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && this.name == MainToDungeonPortal.name)
        {
            Status playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();

            DataManager.dataManagerInstance.SetHudValues(playerStatus.getHp(), playerStatus.GetCurrentMp(), playerStatus.getStamina());

            SoundManager.soundManagerInstacne.initializeSFX();
            SceneManager.LoadScene("Boss Scene");
        }
    }
}
