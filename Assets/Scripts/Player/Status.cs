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
    private void staminaDown_Sprint()
    {
        if(this.stamina > 0)
            this.stamina -= 1f;
        if (this.stamina <= 0)
        {
            InvokeCancle("staminaDown_Sprint");
            
            this.stamina = 0;
        }
    }
    public void staminaDown_Dodge(float value)
    {
        if(this.stamina > 0)
            this.stamina -= value;
        if (this.stamina <= 0)
            this.stamina = 0;
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
        if (this.hp <= 0)
            this.hp = 0;
    }

    // 데미지 관련
    public float getDmg()
    {
        return damage;
    }

    public float getStamina()
    {
        return stamina;
    }
}
