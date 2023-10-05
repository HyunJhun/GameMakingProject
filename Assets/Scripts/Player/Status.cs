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
    // 스태미너 관련
    public void staminaDown()
    {
        stamina -= 1f;
    }
    public void InvokeCancle(string name)
    {
        CancelInvoke(name);
    }
    // 체력 관련
    public void hpDown(float hp)
    {
        if(this.hp > 0)
            this.hp -= hp;
    }

    // 데미지 관련
    public float getDmg()
    {
        return damage;
    }
}
