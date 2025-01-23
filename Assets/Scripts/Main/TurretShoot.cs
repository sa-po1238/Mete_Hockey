using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    [SerializeField] private GameObject singleShotPrefab; // 弱弾のPrefab
    [SerializeField] private GameObject chargeShotPrefab; // チャージ弾のPrefab
    [SerializeField] private float singleShootForce = 10f; // 弱弾の速度（力）
    [SerializeField] private float chargeShootForce = 10f; // チャージ弾の速度（力）
    [SerializeField] private Transform firePoint; // 発射位置
    [SerializeField] private float coolTime = 0.1f; // 共通のクールタイム
    private float currentTime = 0f; //直前の発射からの時間 
    [SerializeField] private float thresholdTime = 2.0f; // 長押しの閾値
    private float pushTime = 0f; //スペースキーを押してからの時間 


    void Update()
    {
        currentTime += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            pushTime += Time.deltaTime;
            Debug.Log(pushTime);
        }

        // スペースキーが離されたら発射
        if ((Input.GetKeyUp(KeyCode.Space)) && (currentTime > coolTime))
        {
            if (pushTime < thresholdTime)
            {
                SingleShoot();
            }
            else
            {
                ChargeShoot();
            }
            currentTime = 0f;
            pushTime = 0f;
        }
    }

    private void SingleShoot()
    {
        // 弾のインスタンスを作成
        GameObject shot = Instantiate(singleShotPrefab, firePoint.position, firePoint.rotation);
        // Rigidbodyを取得してタレットの「右方向」に力を加える
        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.right * singleShootForce, ForceMode.Impulse);
        }
    }

    private void ChargeShoot()
    {
        // 弾のインスタンスを作成
        GameObject shot = Instantiate(chargeShotPrefab, firePoint.position, firePoint.rotation);
        // Rigidbodyを取得してタレットの「右方向」に力を加える
        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.right * chargeShootForce, ForceMode.Impulse);
        }
    }
}
