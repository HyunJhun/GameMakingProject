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
    [SerializeField] private TMP_Text moveSpeed;
    [Header("SliderUI")]
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private Slider playerStaminaBar;
    [SerializeField] private Slider playerMpBar;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private TMP_Text bossNameText;

    [Header("Debug")]
    [SerializeField] private Button btn_x1;
    [SerializeField] private Button btn_x0;
    [SerializeField] private Button btn_x05;
    [SerializeField] private Button btn_x025;

    private Player playerComp;


    private void Awake()
    {
        if(btn_x0 == null || btn_x1 == null || btn_x05 == null || btn_x025 == null)
        {
            Debug.LogError("버튼이 할당되지 않았습니다.");
            return;
        }
        btn_x1.onClick.AddListener(() => SetTimeScale(1f));
        btn_x0.onClick.AddListener(() => SetTimeScale(0f));
        btn_x05.onClick.AddListener(() => SetTimeScale(0.5f));
        btn_x025.onClick.AddListener(() => SetTimeScale(0.25f));
    }
    void Start()
    {
        GameObject.FindWithTag("Player").TryGetComponent<Status>(out player);
        GameObject.FindWithTag("Boss").TryGetComponent<Status>(out boss);
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
            bossNameText.text = "The Darkness Ancient Dragon";
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHpBar.value = player.getHp();
        playerStaminaBar.value = player.getStamina();
        playerMpBar.value = player.GetCurrentMp();
        
        if (boss != null)
        {
            if(bossHpBar.isActiveAndEnabled)
                bossHpBar.value = boss.getHp();
        }

    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Debug.Log($"TimeScale 변경: {scale}");
    }
}
