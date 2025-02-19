using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPGauge : MonoBehaviour
{
    [SerializeField] Slider enemyHPSlider;
    [SerializeField] EnemyManager enemyManager; // EnemyManager.csがアタッチされているやつ
    private float maxHP; // 敵の最大HP
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();

        // Sliderの最大値をHPに合わせる
        maxHP = enemyManager.GetMaxHP();
        enemyHPSlider.maxValue = maxHP;
        enemyHPSlider.value = maxHP;
    }

    void Update()
    {
        // Sliderの値を現在HPに合わせる
        enemyHPSlider.value = enemyManager.GetCurrentHP();

        // HPが0になったらSliderも破壊(これ上手くいってないけどなんかしらいる)
        if (enemyHPSlider.value <= 0)
        {
            Destroy(gameObject);
        }
    }
}
