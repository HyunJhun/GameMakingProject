using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Status : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float damage;

    [Header("Player")]
    [SerializeField] private List<float> attackStamina; // 0~2 : OneHanded , 3~5 : TwoHanded
    [SerializeField] private List<float> attackDamage; // 0~2 : OneHanded , 3~5 : TwoHanded

    // Start is called before the first frame update
    void Start()
    {
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
            this.stamina = 0;
        }
    }
    public void staminaDown_Dodge(float value)
    {
        if(this.stamina > 0)
            this.stamina -= value;
        if (this.stamina <= 0)
        {
            this.stamina = 0;
        }
    }
    public void staminaDown(float value)
    {
        if (this.stamina > 0)
            this.stamina -= value;
        if (this.stamina <= 0)
        {
            this.stamina = 0;
        }
    }
    public void InvokeCancel(string name)
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
    public float GetDamag()
    {
        return damage;
    }
    public float GetAttackDamage(int idx)
    {
        return attackDamage[idx];
    }
    public float GetAttackStamina(int idx)
    {
        return attackStamina[idx];
    }
    public float getStamina()
    {
        return stamina;
    }
    public float getMaxStamina() { return maxStamina; }
    public float getHp()
    {
        return hp;
    }

    public float GetMaxHP()
    {
        return maxHp;
    }

    public void SetBossHpToMaxHp()
    {
        hp = maxHp;
    }
}
