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
    [SerializeField] private TMP_Text playerStateText;
    [Header("SliderUI")]
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private Slider playerStaminaBar;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private TMP_Text bossNameText;
    void Start()
    {
        playerHpBar.maxValue = player.GetMaxHP();
        playerHpBar.minValue = 0f;
        playerStaminaBar.maxValue = player.getMaxStamina();
        playerStaminaBar.minValue = 0f;
        bossHpBar.maxValue = boss.GetMaxHP();
        bossHpBar.minValue = 0f;
        bossNameText.text = "Boss";
    }

    // Update is called once per frame
    void Update()
    {
        playerHpBar.value = player.getHp();
        playerStaminaBar.value = player.getStamina();
        bossHpBar.value = boss.getHp();

        playerStateText.text = playerP.playerStateMachine.currentState.ToString();
    }
}
