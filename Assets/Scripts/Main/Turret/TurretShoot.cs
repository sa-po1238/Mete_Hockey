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
    [SerializeField] ParticleSystem chargeParticle1; // チャージのパーティクル
    [SerializeField] ParticleSystem chargeParticle2; // チャージのパーティクル2
    [SerializeField] ParticleSystem chargeParticle3; // チャージのパーティクル3（チャージ完了時にまとうエフェクト）

    private ParticleSystem newParticle1;
    private ParticleSystem newParticle2;
    private ParticleSystem newParticle3;


    void Update()
    {
        currentTime += Time.deltaTime;
        //スペースキーが押されているときはチャージ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance_AudioManager.PlaySE(1);

            newParticle1 = Instantiate(chargeParticle1);
            newParticle1.transform.position = this.transform.position;
            newParticle1.Play();
            newParticle2 = Instantiate(chargeParticle2);
            newParticle2.transform.position = this.transform.position;
            newParticle2.Play();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            pushTime += Time.deltaTime;
            if (pushTime > thresholdTime)
            {
                Destroy(newParticle1);
                Destroy(newParticle2);
                /*
                newParticle3 = Instantiate(chargeParticle3);
                newParticle3.transform.position = this.transform.position;
                newParticle3.Play();
                */
            }
        }
        else
        {
            // スペースキーが離されたらChargeShoot
            if ((Input.GetKeyUp(KeyCode.Space)) && (currentTime > coolTime))
            {
                if (pushTime > thresholdTime)
                {
                    AudioManager.instance_AudioManager.PlaySE(2);
                    ChargeShoot();
                }
                currentTime = 0f;
                pushTime = 0f;
            }
            // 一定の間隔で勝手にSingleShootする
            if (currentTime > coolTime)
            {
                AudioManager.instance_AudioManager.PlaySE(0);
                SingleShoot();
                currentTime = 0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            AudioManager.instance_AudioManager.StopSE(1);
            Destroy(newParticle1);
            Destroy(newParticle2);
            //Destroy(newParticle3);
        }
    }

    // 弱ショット
    private void SingleShoot()
    {
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
        GameObject shot = Instantiate(chargeShotPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(firePoint.right * chargeShootForce, ForceMode.Impulse);
        }
    }

}
