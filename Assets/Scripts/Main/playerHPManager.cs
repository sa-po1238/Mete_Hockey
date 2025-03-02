using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] float playerHP = 100f; //プレイヤーのHP
    private float currentPlayerHP; //プレイヤーの現在のHP
    [SerializeField] private PlayerHPManager playerHPManager;
    [SerializeField] private PlayerHPGauge playerHPGauge;
    private void Awake()
    {
        currentPlayerHP = playerHP;
    }
    public void TakeDamage(float damage)
    {
        currentPlayerHP -= damage;
        Debug.Log($"HP:{currentPlayerHP}");

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
