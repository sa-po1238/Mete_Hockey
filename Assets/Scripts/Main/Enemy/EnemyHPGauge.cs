using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPGauge : MonoBehaviour
{
    [SerializeField] Slider enemyHPSlider;
    [SerializeField] EnemyManager enemyManager; // EnemyManager.csがアタッチされているやつ
    private float maxHP; // 敵の最大HP
    private float currentHP; // 敵の現在HP
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();

        // Sliderの最大値をHPに合わせる
        maxHP = enemyManager.GetMaxEnemyHP();
        enemyHPSlider.maxValue = maxHP;
        enemyHPSlider.value = maxHP;
    }
    void Update()
    {
        // Sliderの値を現在HPに合わせる
        currentHP = enemyManager.GetCurrentEnemyHP();
        if ((enemyManager.gameObject.tag == "EnemyShot") || (enemyManager.gameObject.tag == "BeanShot") || (enemyManager.gameObject.tag == "Explosion"))
        {
            enemyHPSlider.value = 0;
        }
        else
        {
            enemyHPSlider.value = currentHP;
        }
    }

}
