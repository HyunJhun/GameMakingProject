using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Status")]
    [SerializeField]private List<Status> obj_StatusList;
    [SerializeField] private Status player;
    [SerializeField] private Status boss;
    [SerializeField] private Player playerP;
    [Header("SliderUI")]
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private Slider playerStaminaBar;
    [SerializeField] private Slider playerMpBar;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private TMP_Text bossNameText;
    void Start()
    {
        playerHpBar.maxValue = player.GetMaxHP();
        playerHpBar.minValue = 0f;
        playerStaminaBar.maxValue = player.getMaxStamina();
        playerStaminaBar.minValue = 0f;
        playerMpBar.maxValue = player.GetMaxMp();
        playerMpBar.minValue = 0f;
        if (boss != null)
        {
            bossHpBar.maxValue = boss.GetMaxHP();
            bossHpBar.minValue = 0f;
            bossNameText.text = "Boss";
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHpBar.value = player.getHp();
        playerStaminaBar.value = player.getStamina();
        playerMpBar.value = player.GetCurrentMp();
        if(boss != null)
            bossHpBar.value = boss.getHp();

    }
}
