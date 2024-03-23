using System.Collections.Generic;
using UnityEngine;
public class Status : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float hp;
    [SerializeField] private float maxHp;
    [SerializeField] private float stamina;
    [SerializeField] private float maxStamina;
    [SerializeField] private float mp;
    [SerializeField] private float maxMp;
    [SerializeField] private float damage;

    [Header("Player")]
    [SerializeField] private List<float> attackStamina = new List<float>(); // 0~2 : OneHanded , 3~5 : TwoHanded
    [SerializeField] private List<float> attackDamage = new List<float>(); // 0~2 : OneHanded , 3~5 : TwoHanded
    [SerializeField] private List<float> skillDamage = new List<float>(); // 0 : SwordJudgment
    [SerializeField] private List<float> skillMpUsage = new List<float>(); // 0: SwordJudgment
    [SerializeField] private Player player;

    [Header("Enemy")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Boss boss;



    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();
        boss = GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    // 스태미너 관련
    private void staminaDown_Sprint()
    {
        if (this.stamina > 0)
            this.stamina -= 1f;
        if (this.stamina <= 0)
        {
            this.stamina = 0;
        }
    }
    public void staminaDown_Dodge(float value)
    {
        if (this.stamina > 0)
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
    private void staminaUp()
    {
        if (this.stamina >= maxStamina)
            this.stamina = maxStamina;
        this.stamina += 0.5f;
    }
    public void InvokeCancel(string name)
    {
        CancelInvoke(name);
    }
    // 체력 관련
    public void hpDown(float hpDeclineRate)
    {
        if (this.hp > 0)
            this.hp -= hpDeclineRate;
        if (this.hp <= 0)
            this.hp = 0;
    }

    // 마나 관련

    public void MpDown(float mpDeclineRate)
    {
        if (this.mp <= 0) this.mp = 0;
        else this.mp -= mpDeclineRate; 
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
    public float GetSkillAttackDamage(int idx) { return skillDamage[idx]; }
    public float GetSkillMpUsage(int idx) { return skillMpUsage[idx]; }
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

    public float GetCurrentMp() { return mp; }
    public float GetMaxMp() { return maxMp; }

    public Boss GetBoss()
    {
        if (boss == null) return null;
        return boss;
    }
    public Enemy GetEnemy()
    {
        if (enemy == null) return null;
        return enemy;
    }
    public void SetBossHpToMaxHp()
    {
        hp = maxHp;
    }
    public void SetStaminaToMaxStamina()
    {
        stamina = maxStamina;
    }

    public void StaminaIncrease()
    {
        InvokeRepeating("staminaUp", 1f, 0.1f);
    }
    private void StaminaCheck()
    {
        if (stamina >= maxStamina) SetStaminaToMaxStamina();
    }
}
