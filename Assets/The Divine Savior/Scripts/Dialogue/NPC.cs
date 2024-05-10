using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private Transform npcTransform;
    [SerializeField] private Volume volume;
    [SerializeField] private Transform playerTransform;
    void Start()
    {
        npcTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        volume = GetComponentInChildren<Volume>();
        volume.dialogue = GetComponentInChildren<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if(volume.isPlayerTriggered)
        {
            npcTransform.LookAt(playerTransform);
        }
        
    }
}
