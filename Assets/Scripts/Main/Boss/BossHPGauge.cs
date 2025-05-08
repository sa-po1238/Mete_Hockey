using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPGauge : MonoBehaviour
{
    [SerializeField] Slider bossHPSlider;
    [SerializeField] BossHPManager bossHPManager; // BossHPManager.csがアタッチされているやつ
    private float maxHP; // 敵の最大HP
    private float currentHP; // 敵の現在HP
    void Start()
    {
        //bossHPManager = GetComponent<BossHPManager>();

        // Sliderの最大値をHPに合わせる
        maxHP = bossHPManager.GetMaxBossHP();
        bossHPSlider.maxValue = maxHP;
        bossHPSlider.value = maxHP;
    }
    void Update()
    {
        // Sliderの値を現在HPに合わせる
        currentHP = bossHPManager.GetCurrentBossHP();

        bossHPSlider.value = currentHP;

    }
}
