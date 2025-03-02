using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHPGauge : MonoBehaviour
{
    [SerializeField] Slider playerHPSlider;
    [SerializeField] PlayerHPManager playerHPManager;
    private float maxHP; // 最大HP
    void Start()
    {
        //playerHPManager = GetComponentInParent<PlayerHPManager>();

        // Sliderの最大値をHPに合わせる
        maxHP = playerHPManager.GetMaxPlayerHP();
        playerHPSlider.maxValue = maxHP;
        playerHPSlider.value = maxHP;
    }

    void Update()
    {
        // Sliderの値を現在HPに合わせる
        playerHPSlider.value = playerHPManager.GetCurrentPlayerHP();

    }
}
