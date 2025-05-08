using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHPManager : MonoBehaviour
{
    [SerializeField] CoreActivator coreActivator; // コアのアクティブ化を管理するスクリプト
    [SerializeField] private float currentBossHP; // 現在のボスHP
    [SerializeField] private float bossHP = 1000f; // ボスのHP
    private float singleDamage = 5f; // シングルショットのダメージ
    private float chargeDamage = 30f; // チャージショットのダメージ
    private float enemyDamage = 100f; // エネミーショットのダメージ

    void Awake()
    {
        currentBossHP = bossHP; // ボスのHPを初期化
    }

    private void OnCollisionEnter(Collision other)
    {
        if (coreActivator.isCoreActive)
        {
            if (other.gameObject.CompareTag("SingleShot"))
            {
                currentBossHP -= singleDamage;
            }
            else if (other.gameObject.CompareTag("ChargeShot"))
            {
                currentBossHP -= chargeDamage;
            }
            else if (other.gameObject.CompareTag("EnemyShot"))
            {
                currentBossHP -= enemyDamage;
            }
        }
        CheckBossHP(); // ボスのHPをチェック
    }
    private void CheckBossHP()
    {
        if (currentBossHP <= 0)
        {
            Debug.Log("ボスが倒されました");
        }
    }
    public float GetCurrentBossHP()
    {
        return currentBossHP; // 現在のボスHPを取得
    }
    public float GetMaxBossHP()
    {
        return bossHP; // ボスの最大HPを取得
    }

}
