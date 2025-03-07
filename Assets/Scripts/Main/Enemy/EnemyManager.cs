using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyData enemyData; // 敵データ
    private float enemySpeed; //敵のスピード
    private float enemyHP; //敵のHP
    private float enemyDamage; //敵の攻撃力、エネミーショットのダメージ
    private float enemyHPRate; // エネミーショットで回復する倍率
    private int enemyScore; // 敵のスコア
    private float currentEnemyHP; // 敵の現在のHP
    [SerializeField] private float destroyLeftLimit = -10f; // 左側の限界値
    [SerializeField] private float destroyRightLimit = 12f; // 右側の限界値
    [SerializeField] float singleDamage = 10f; //シングルショットのダメージ
    [SerializeField] float chargeDamage = 20f; //チャージショットのダメージ
    [SerializeField] float enemyShotRate = 1.2f; // エネミーショットで加速する倍率
    [SerializeField] float enemyBounceRate = 1.05f; // 敵と当たったときにちょっと加速する
    [SerializeField] private float speedThreshold = 0.01f; // 速度の閾値
    [SerializeField] private int hitThreshold = 10; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    private Rigidbody rb;
    private Collider col;
    private Vector3 pastVelocity; // ５秒前の速度

    private void Awake()
    {
        // EnemyData から値を取得
        enemySpeed = enemyData.enemySpeed;
        enemyHP = enemyData.enemyHP;
        enemyDamage = enemyData.enemyDamage;
        enemyHPRate = enemyData.enemyHPRate;
        enemyScore = enemyData.enemyScore;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        currentEnemyHP = enemyHP;
        StartCoroutine(SavePastVelocity()); // 5フレーム前の速度を保存するコルーチン開始

        if (this.gameObject.tag == "Bean")
        {
            col.isTrigger = true; // IsTriggerのチェックを入れる        
        }
    }

    // 5フレームごとにpastVelocityを更新
    private IEnumerator SavePastVelocity()
    {
        while (true)
        {
            pastVelocity = rb.velocity;
            yield return new WaitForSeconds(0.083f);
        }
    }

    void Update()
    {
        if ((this.gameObject.tag == "Bean") || (this.gameObject.tag == "enemy"))
        {
            transform.position += new Vector3(-enemySpeed * Time.deltaTime, 0, 0);
        }
        // x方向での範囲チェック
        if ((transform.position.x >= destroyRightLimit) || (transform.position.x <= destroyLeftLimit))
        {
            Destroy(gameObject); // 範囲外に出たら敵を破壊
        }
        // HPチェック
        if (currentEnemyHP <= 0)
        {
            Destroy(gameObject);
        }

        // 速度が落ちすぎたエネミーショットを破壊
        if (((rb.velocity.magnitude <= speedThreshold) && (this.gameObject.tag == "EnemyShot")) || ((rb.velocity.magnitude <= speedThreshold) && (this.gameObject.tag == "BeanShot")))
        {
            Destroy(gameObject);
        }

        if (this.gameObject.tag == "BeanShot")
        {
            rb.velocity *= 0.99f;
            Debug.Log(rb.velocity.magnitude);

        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (this.gameObject.tag == "EnemyShot")
        {
            currentHit += 1;
            if (currentHit >= hitThreshold)
            {
                Destroy(gameObject); // 衝突しすぎたエネミーショットを破壊
            }
            else if (other.gameObject.tag == "enemy")
            {
                rb.velocity *= enemyBounceRate; // 衝突時に少し速度を上げる
            }
            else if (other.gameObject.tag == "SingleShot")
            {
                rb.velocity = pastVelocity; // 5フレーム前の速度を適用
            }
        }
        else
        {
            // 衝突した対象の種類に応じてダメージが変化
            if (other.gameObject.tag == "SingleShot")
            {
                currentEnemyHP -= singleDamage;
            }
            else if (other.gameObject.tag == "ChargeShot")
            {
                currentEnemyHP -= chargeDamage;
                // チャージショットで倒れたらエネミーショットに
                if (currentEnemyHP <= 0)
                {
                    rb.isKinematic = false; // 物理演算を受けるようにする

                    // 衝突したチャージショットから5フレーム前の速度をもらう
                    ChargeShot chargeShot = other.gameObject.GetComponent<ChargeShot>();
                    rb.velocity = chargeShot.GetChargeShotVelocity();

                    currentEnemyHP = enemyHP * enemyHPRate; // 元のHPの倍数に回復
                    rb.velocity *= enemyShotRate; // 衝突時に速度を上げる
                    this.gameObject.tag = "EnemyShot";
                }
            }
            else if (other.gameObject.tag == "EnemyShot")
            {
                currentEnemyHP -= enemyDamage;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // エネミーショットと豆がぶつかるときもこっちが呼び出される
        if (this.gameObject.tag == "EnemyShot")
        {
            currentHit += 1;
            if (currentHit >= hitThreshold)
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "enemy")
            {
                rb.velocity *= enemyBounceRate; // 衝突時に少し速度を上げる
            }
        }
        else
        {
            // 衝突した対象の種類に応じてダメージが変化
            if (other.gameObject.tag == "EnemyShot")
            {
                // ランダムな方向に飛び出す
                rb.isKinematic = false; // 物理演算を受けるようにする
                Vector3 randomDirection = Random.insideUnitSphere.normalized; // ランダムな方向を取得
                rb.velocity = randomDirection * 10; //大きさはenemyShotRateにする
                currentEnemyHP = enemyHP * enemyHPRate; // 元のHPの倍数に回復
                this.gameObject.tag = "BeanShot";

                Debug.Log(randomDirection);
            }
            else if (other.gameObject.tag == "BeanShot")
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "SingleShot")
            {
                currentEnemyHP -= singleDamage;
            }
            else if (other.gameObject.tag == "ChargeShot")
            {
                currentEnemyHP -= chargeDamage;
                // チャージショットで倒れたらエネミーショットに
                if (currentEnemyHP <= 0)
                {
                    rb.isKinematic = false; // 物理演算を受けるようにする

                    // 衝突したチャージショットから5フレーム前の速度をもらう
                    ChargeShot chargeShot = other.gameObject.GetComponent<ChargeShot>();
                    rb.velocity = chargeShot.GetChargeShotVelocity();

                    col.isTrigger = false; // IsTriggerのチェックを外す
                    currentEnemyHP = enemyHP * enemyHPRate; // 元のHPの倍数に回復
                    rb.velocity *= enemyShotRate; // 衝突時に速度を上げる

                    this.gameObject.tag = "EnemyShot";
                }
            }
            else if (other.gameObject.tag == "EnemyShot")
            {
                currentEnemyHP -= enemyDamage;
            }
            else if ((other.gameObject.tag == "Turret") || (other.gameObject.tag == "Bunker"))
            {
                Destroy(gameObject);
            }
        }
    }

    // 外から最大HPや現在HPを取るための関数
    public float GetMaxEnemyHP()
    {
        return enemyHP;
    }

    public float GetCurrentEnemyHP()
    {
        return currentEnemyHP;
    }
    public float GetEnemyDamage()
    {
        return enemyDamage;
    }

    // スコア参照のために使うやつ
    private void OnDestroy()
    {
        if ((transform.position.x <= destroyRightLimit) && (transform.position.x >= destroyLeftLimit))
        {
            ScoreManager.instance.AddScore(enemyScore);
        }

    }

}
