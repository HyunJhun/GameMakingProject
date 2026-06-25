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
    private float targetAlphaVar = 0f;
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
            
            calculateHpByWidth();
            if (!isFadeOut)
            {
                isFadeOut = true;
                Invoke("FadeOut", 0.5f);
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
    IEnumerator AlphaFadeOut()
    {
        Color backgroundColor = backgroundImage.color;
        Color hpImageColor = hpImage.color;

        if (isFadeOut)
        {
            while (backgroundColor.a > 0.01f && hpImageColor.a > 0.01f)
            {
                backgroundColor.a = Mathf.Lerp(backgroundColor.a, targetAlphaVar, Time.deltaTime);
                hpImageColor.a = Mathf.Lerp(hpImageColor.a, targetAlphaVar, Time.deltaTime);
                backgroundImage.color = backgroundColor;
                hpImage.color = hpImageColor;
                yield return null;
            }
            isFadeOut = false;
            hpUIObject.SetActive(false);
        }
    }
    public void ResetHpUIAlphaValue()
    {
        hpImage.color = new Color(hpImage.color.r, hpImage.color.b, hpImage.color.g, 1f);
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.b, backgroundImage.color.g, 1f);

    }
    private void FadeOut()
    {
        StartCoroutine("AlphaFadeOut");
    }

    
    public GameObject GetHpUIObject() { return hpUIObject; }

}
