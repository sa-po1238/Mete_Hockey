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

    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値
    [SerializeField] private float destroyRightLimit = 24f; // 右側の限界値
    [SerializeField] float singleDamage = 5f; //シングルショットのダメージ
    [SerializeField] float chargeDamage = 30f; //チャージショットのダメージ
    [SerializeField] float enemyShotRate = 1.2f; // エネミーショットで加速する倍率
    [SerializeField] float enemyBounceRate = 1.05f; // 敵と当たったときにちょっと加速する
    [SerializeField] private float speedThreshold = 0.1f; // 速度の閾値
    [SerializeField] private int hitThreshold = 20; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    private Rigidbody rb;
    private Collider col;
    private Vector3 pastVelocity; // ５秒前の速度
    [SerializeField] float explosionDamage = 50f; // 爆発のダメージ
    [SerializeField] GameObject BankerEffectPrefab; // バンカーのエフェクト

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
        if ((this.gameObject.tag == "Bean") || (this.gameObject.tag == "enemy") || (this.gameObject.tag == "Bomb"))
        {
            transform.position += new Vector3(-enemySpeed * Time.deltaTime, 0, 0);
        }
        // x方向での範囲チェック
        if ((transform.position.x >= destroyRightLimit) || (transform.position.x <= destroyLeftLimit))
        {
            Destroy(gameObject); // 範囲外に出たら敵を破壊
        }

        // 速度が落ちすぎたエネミーショットを破壊
        if (((rb.velocity.magnitude <= speedThreshold) && (this.gameObject.tag == "EnemyShot")) || ((rb.velocity.magnitude <= speedThreshold) && (this.gameObject.tag == "BeanShot")))
        {
            Destroy(gameObject);
        }

        if (this.gameObject.tag == "EnemyShot") // BeanもBombも上手くいかなかったのでないない
        {
            // 向きも速度に合わせて変更する
            float angle = Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg; // XY平面の角度を計算
            float lastAngle = angle - 90;
            if ((-45 <= lastAngle) && (lastAngle <= 45))
            {
                lastAngle = angle - 90;
            }
            else
            {
                lastAngle = -(angle - 90);
            }
            transform.rotation = Quaternion.Euler(0, 0, lastAngle); // Z軸のみ回転
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
            else if (other.gameObject.tag == "Bunker_a")
            {
                // バンカーのエフェクトを生成
                Vector3 hitPos = Vector3.zero;
                foreach (ContactPoint point in other.contacts)
                {
                    hitPos = point.point;
                }
                Destroy(Instantiate(BankerEffectPrefab, hitPos, transform.rotation), 0.28f);
            }
            else if (other.gameObject.tag == "Bunker_b")
            {
                // バンカーのエフェクトを生成
                Vector3 hitPos = Vector3.zero;
                foreach (ContactPoint point in other.contacts)
                {
                    hitPos = point.point;
                }
                Destroy(Instantiate(BankerEffectPrefab, hitPos, Quaternion.Euler(0, 0, 180)), 0.28f);
            }
        }
        else
        {
            // 衝突した対象の種類に応じてダメージが変化
            if (other.gameObject.tag == "SingleShot")
            {
                currentEnemyHP -= singleDamage;
                CheckWhichAnimation(currentEnemyHP);
            }
            else if (other.gameObject.tag == "ChargeShot")
            {
                currentEnemyHP -= chargeDamage;

                // チャージショットで倒れたらエネミーショットに
                if ((currentEnemyHP <= 0))
                {
                    rb.isKinematic = false; // 物理演算を受けるようにする

                    // 衝突したチャージショットから5フレーム前の速度をもらう
                    ChargeShot chargeShot = other.gameObject.GetComponent<ChargeShot>();
                    rb.velocity = chargeShot.GetChargeShotVelocity();

                    currentEnemyHP = enemyHP * enemyHPRate; // 元のHPの倍数に回復
                    // Bombなら爆発処理（タイミング調整のためこっちに書いてる）
                    if (this.gameObject.tag == "Bomb")
                    {
                        GetComponent<BombManager>().Explosion();
                    }
                    else
                    {
                        rb.velocity *= enemyShotRate; // 衝突時に速度を上げる
                        this.gameObject.tag = "EnemyShot";

                        // 弾化のアニメーション
                        GetComponent<EnemyAnimation>().DieForStrong();
                    }
                }
                else
                {
                    GetComponent<EnemyAnimation>().TakeWeakDamage();
                }
            }
            else if (other.gameObject.tag == "EnemyShot")
            {
                currentEnemyHP -= enemyDamage;
                CheckWhichAnimation(currentEnemyHP);
            }
            else if (other.gameObject.tag == "Explosion")
            {
                currentEnemyHP -= explosionDamage;
                CheckWhichAnimation(currentEnemyHP);
            }

        }

    }


    // どのアニメーションを再生するかを決める関数
    private void CheckWhichAnimation(float currentEnemyHP)
    {
        // 死んでたら死亡アニメーション
        if (currentEnemyHP <= 0)
        {
            GetComponent<EnemyAnimation>().DieForWeak();
        }

        // 死んでなかったらダメージを受けるアニメーション
        GetComponent<EnemyAnimation>().TakeWeakDamage();
    }


    // エネミーショットで豆に当たったとき・BeanShotが他の何かに当たったとき
    private void OnTriggerEnter(Collider other)
    {
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
        if (other.gameObject.tag == "BeanShot")
        {

            currentEnemyHP -= enemyDamage; // 豆のエネミーショットと同じダメージ
            CheckWhichAnimation(currentEnemyHP);

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

    // 敵のHPを更新
    public void UpdateEnemyHP(float hp)
    {
        currentEnemyHP = hp;
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
