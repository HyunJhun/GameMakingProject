using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    [Header("Reference")]
    private Enemy enemy;
    private Status enemyStats;
    [SerializeField] private GameObject hpUIObject;
    [SerializeField] private RectTransform hpBar;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image hpImage;
    private float maxHpBarWidth;
    public bool isFadeOut { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyStats = GetComponent<Status>();
        maxHpBarWidth = hpBar.rect.width;
        isFadeOut = false;
        hpUIObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hpUIObject.activeSelf)
        {
            if (checkHpUIAlphaReachToZero()) { hpUIObject.SetActive(false); return; }
            calculateHpByWidth();
            if (!isFadeOut)
            {
                isFadeOut = true;
                Invoke("HpUIFadeOut", 1f);
                Debug.Log($"NAme : {transform.name} and Count");
                return;
            }
        }
    }

    private void calculateHpByWidth()
    {
        float hpByWidth = enemyStats.getHp() * maxHpBarWidth / enemyStats.GetMaxHP();

        // 왜 sizeDelta를 좀 더 알아봐야할거같음.

        hpBar.sizeDelta = new Vector2(hpByWidth, hpBar.rect.height);

    }
    public void HpUIFadeOut()
    {
        backgroundImage.CrossFadeAlpha(0,1f,false); // 실질적인 값이 변하는게 아니야 !! 
        hpImage.CrossFadeAlpha(0,1f,false);
    }
    public void ResetHpUIAlphaValue()
    {
        hpImage.color += new Color(0f, 0f, 0f, 1f);
        backgroundImage.color += new Color(0f, 0f, 0f, 1f);

    }
    private bool checkHpUIAlphaReachToZero()
    {
        return hpImage.color.a < 0.3f && backgroundImage.color.a < 0.3f;
    }
    public GameObject GetHpUIObject() { return hpUIObject; }

}
