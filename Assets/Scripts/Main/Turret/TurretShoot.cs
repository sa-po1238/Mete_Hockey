using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    [Header("弾の設定")]
    [SerializeField] private GameObject singleShotPrefab; // 弱弾のPrefab
    [SerializeField] private GameObject chargeShotPrefab; // チャージ弾のPrefab
    [SerializeField] private float singleShootForce = 10f; // 弱弾の速度（力）
    [SerializeField] private float chargeShootForce = 10f; // チャージ弾の速度（力）
    [SerializeField] private Transform firePoint; // 発射位置

    [Header("クールタイム設定")]
    [SerializeField] private float coolTime = 0.1f; // 共通のクールタイム
    private float currentTime = 0f; //直前の発射からの時間 

    [Header("チャージ設定")]
    [SerializeField] private float thresholdTime = 2.0f; // 長押しの閾値
    private float pushTime = 0f; //スペースキーを押してからの時間 

    [SerializeField] private Animator chargeShotAnimator;
    [SerializeField] private Animator arrowAnimator;


    void Update()
    {
        currentTime += Time.deltaTime;
        //スペースキーが押されているときはチャージ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance_AudioManager.PlaySE(1);

            chargeShotAnimator.SetBool("isCharging", true);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            pushTime += Time.deltaTime;
        }
        else
        {
            // スペースキーが離されたらChargeShoot
            if ((Input.GetKeyUp(KeyCode.Space)) && (currentTime > coolTime))
            {
                chargeShotAnimator.SetBool("isCharging", false);
                arrowAnimator.SetBool("isChargeGuide", false);
                
                if (pushTime > thresholdTime)
                {
                    ChargeShoot();
                }
                currentTime = 0f;
                pushTime = 0f;
            }
            // 一定の間隔で勝手にSingleShootする
            if (currentTime > coolTime)
            {
                SingleShoot();
                currentTime = 0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            chargeShotAnimator.SetBool("isCharging", false);
            AudioManager.instance_AudioManager.StopSE(1);
        }

        if (pushTime > thresholdTime)
        {
            arrowAnimator.SetBool("isChargeGuide", true);
        }
    }

    // 弱ショット
    private void SingleShoot()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        // 弾のインスタンスを作成
        GameObject shot = Instantiate(singleShotPrefab, firePoint.position, firePoint.rotation);
        // Rigidbodyを取得してタレットのローカルX軸方向に力を加える
        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // firePoint.rightをそのまま使ってローカルX軸方向に力を加える
            rb.AddForce(firePoint.right * singleShootForce, ForceMode.Impulse);
        }
    }

    // チャージショット
    private void ChargeShoot()
    {
        AudioManager.instance_AudioManager.PlaySE(2);
        GameObject shot = Instantiate(chargeShotPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.right * chargeShootForce, ForceMode.Impulse);
        }
    }

}