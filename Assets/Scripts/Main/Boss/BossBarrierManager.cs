using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrierManager : MonoBehaviour
{
    private float currentBarrierHP; // 現在のバリアHP
    [SerializeField] private float barrierHP = 100f; // バリアのHP
    [SerializeField] private float singleDamage = 5f; // シングルショットのダメージ
    [SerializeField] private float chargeDamage = 30f; // チャージショットのダメージ
    [SerializeField] GameObject barrierEffect; // バリアのエフェクト

    public delegate void BarrierDestroyedHandler(BossBarrierManager barrier);
    public static event BarrierDestroyedHandler OnBarrierDestroyed; // 全体共有のイベント

    private bool isBarrierBroken = false;
    [SerializeField] CoreActivator coreActivator; // コアのアクティブ化を管理するスクリプト
    [SerializeField] float currentCoolTime = 0f;
    private float coolTime = 10f; // バリアのクールタイム




    private void Awake()
    {
        currentBarrierHP = barrierHP; // バリアのHPを初期化
        barrierEffect.SetActive(true); // バリアのエフェクトを表示
    }

    private void Update()
    {
        Debug.Log(coreActivator.isCoreActive);
        // コアがアクティブになったら10秒数える
        if (coreActivator.isCoreActive)
        {
            currentCoolTime += Time.deltaTime; // クールタイムを加算
        }
        else
        {
            // ここでやらないとcurrentCoolTimeが最初に10に達せたひとつのバリアしか回復しない
            if (currentCoolTime > 0f)
            {
                currentBarrierHP = barrierHP; // バリアのHPをリセット
                isBarrierBroken = false; // バリアが壊れていない状態に戻す
                barrierEffect.SetActive(true); // バリアのエフェクトを表示     
                currentCoolTime = 0f; // コアがアクティブでない場合、クールタイムをリセット       
            }
        }
        if (currentCoolTime >= coolTime)
        {
            coreActivator.ResetCoreActivation(); // コアのアクティブ化をリセット
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("SingleShot"))
        {
            currentBarrierHP -= singleDamage; // バリアのHPを減少
        }
        else if (other.gameObject.CompareTag("ChargeShot"))
        {
            currentBarrierHP -= chargeDamage; // バリアのHPを減少
        }
        else if (other.gameObject.CompareTag("EnemyShot"))
        {
            currentBarrierHP = 0; // バリアを一気に破壊
        }
        CheckBarrierHP(); // バリアのHPをチェック
    }

    private void CheckBarrierHP()
    {
        if (currentBarrierHP <= 0 && !isBarrierBroken)
        {
            isBarrierBroken = true;
            Debug.Log("Barrier Destroyed"); // バリアが破壊されたときの処理
            // バリアのエフェクトを非表示に
            barrierEffect.SetActive(false);

            OnBarrierDestroyed?.Invoke(this); // コア管理側に通知
        }
    }
}
