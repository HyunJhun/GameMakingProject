using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    [Header("Reference")]
    private Enemy enemy;
    private Status enemyStats;
    [SerializeField] private RectTransform hpBar;

    private float maxHpBarWidth;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyStats = GetComponent<Status>();
        maxHpBarWidth = hpBar.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        calculateHpByWidth();
    }

    private void calculateHpByWidth()
    {
        float hpByWidth = enemyStats.getHp() * maxHpBarWidth / enemyStats.GetMaxHP();

        // 왜 sizeDelta를 좀 더 알아봐야할거같음.

        hpBar.sizeDelta = new Vector2(hpByWidth, hpBar.rect.height);

    }


}
