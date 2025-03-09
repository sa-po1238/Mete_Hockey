using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] float playerHP = 100f; //プレイヤーのHP
    private float currentPlayerHP; //プレイヤーの現在のHP
    [SerializeField] private PlayerHPManager playerHPManager;
    [SerializeField] private PlayerHPGauge playerHPGauge;
    [SerializeField] private  GameObject damageArea;
    private Animator damageAreaAnimator;
    private void Awake()
    {
        currentPlayerHP = playerHP;

        if (damageArea != null)
        {
            damageAreaAnimator = damageArea.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("DamageAreaがアタッチされてない");
        }
    }
    public void TakeDamage(float damage)
    {
        currentPlayerHP -= damage;
        Debug.Log($"HP:{currentPlayerHP}");

        // DamageAreaのアニメーションを再生
        damageAreaAnimator.SetTrigger("isDamageAreaDamaged");

        if (currentPlayerHP <= 0)
        {
            Debug.Log("GameOver");
        }
    }

    // 外から最大HPや現在HPを取るための関数
    public float GetMaxPlayerHP()
    {
        return playerHP;
    }

    public float GetCurrentPlayerHP()
    {
        return currentPlayerHP;
    }
}
