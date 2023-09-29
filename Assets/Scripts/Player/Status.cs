using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float stamina;
    [SerializeField] private float damage;
    


    // Start is called before the first frame update
    void Start()
    {
        stamina = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void staminaDown()
    {
        stamina -= 1f;
    }
    public void InvokeCancle(string name)
    {
        CancelInvoke(name);
    }
}
