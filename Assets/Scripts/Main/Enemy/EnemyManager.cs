using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyData enemyData; // 敵データ
    private float enemySpeed => enemyData.enemySpeed; // 敵のスピード
    private float enemyHP => enemyData.enemyHP; // 敵のHP
    private float enemyDamage => enemyData.enemyDamage; // 敵の攻撃力、エネミーショットのダメージ
    private float enemyHPRate => enemyData.enemyHPRate; // エネミーショットで回復する倍率
    private int enemyScore => enemyData.enemyScore; // 敵のスコア
    private float currentEnemyHP; // 敵の現在のHP


    private float destroyLeftLimit = -12f; // 左側の限界値
    private float destroyRightLimit = 24f; // 右側の限界値
    
    [SerializeField] float singleDamage = 5f; //シングルショットのダメージ
    [SerializeField] float chargeDamage = 30f; //チャージショットのダメージ
    [SerializeField] float enemyShotRate = 1.2f; // エネミーショットで加速する倍率
    [SerializeField] float enemyBounceRate = 1.05f; // 敵と当たったときにちょっと加速する
    [SerializeField] private float speedThreshold = 0.1f; // 速度の閾値
    [SerializeField] private int hitThreshold = 20; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    private Rigidbody rb;
    private Vector3 pastVelocity; // ５秒前の速度
    [SerializeField] float explosionDamage = 50f; // 爆発のダメージ
    [SerializeField] GameObject BankerEffectPrefab; // バンカーのエフェクト

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        EnemyMove(); // 敵の移動処理
        CheckDestroyCondition(); // 範囲外に出たら敵を破壊
        UpdateRotationForShots(); // エネミーショットの向きを更新
    }

    private void EnemyMove()
    {
        // 敵の移動処理
        if(CompareTag("Bean") || CompareTag("enemy") || CompareTag("Bomb"))
        {
            transform.position += new Vector3(-enemySpeed * Time.deltaTime, 0, 0);
        }
    }

    private void CheckDestroyCondition()
    {
        // x方向での範囲チェック
        if ((transform.position.x <= destroyLeftLimit || transform.position.x >= destroyRightLimit))
        {
            Destroy(gameObject); // 範囲外に出たら敵を破壊
        }

        // 速度が落ちすぎたエネミーショットを破壊
        if (rb.velocity.magnitude <= speedThreshold && (CompareTag("EnemyShot") || CompareTag("BeanShot")))
        {
            Destroy(gameObject);
        } 
    }

    private void UpdateRotationForShots()
    {
        if (!CompareTag("EnemyShot")) return;
        // エネミーショットの向きを速度に合わせて変更する
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

    private void OnCollisionEnter(Collision other)
    {
        string otherTag = other.gameObject.tag; // 衝突したオブジェクトのタグを取得

        if (CompareTag("EnemyShot"))
        {
            HandleEnemyShotCollision(other, otherTag);
        }
        else
        {
            HandleEnemyCollision(other, otherTag);
        }

    }
    
    private void HandleEnemyShotCollision(Collision other, string otherTag)
    {
        currentHit++;

        if (currentHit >= hitThreshold)
        {
            Destroy(gameObject); // 衝突しすぎたエネミーショットを破壊
            return;
        }

        Vector3 hitPos = Vector3.zero;

        switch (otherTag)
        {
            case "enemy":
                rb.velocity *= enemyBounceRate; // 衝突時に少し速度を上げる
                break;
            case "SingleShot":
                rb.velocity = pastVelocity; // 5フレーム前の速度を適用
                break;
            case "Bunker_a":
                // バンカーのエフェクトを生成
                foreach (ContactPoint point in other.contacts)
                {
                    hitPos = point.point;
                }
                Destroy(Instantiate(BankerEffectPrefab, hitPos, transform.rotation), 0.28f);
                break;
            case "Bunker_b":
                // バンカーのエフェクトを生成
                foreach (ContactPoint point in other.contacts)
                {
                    hitPos = point.point;
                }
                Destroy(Instantiate(BankerEffectPrefab, hitPos, Quaternion.Euler(0, 0, 180)), 0.28f);
                break;
        }
    }

    private void HandleEnemyCollision(Collision other, string otherTag)
    {
        switch (otherTag)
        {
            case "SingleShot":
                ApplyDamage(singleDamage);
                break;
            case "ChargeShot":
                ApplyChargeShotDamage(other);
                break;
            case "EnemyShot":
                ApplyDamage(enemyDamage);
                break;
            case "Explosion":
                ApplyDamage(explosionDamage);
                break;
        }
    }

    private void ApplyChargeShotDamage(Collision other)
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
            if (CompareTag("Bomb"))
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

    private void ApplyDamage(float damage)
    {
        currentEnemyHP -= damage; // ダメージを適用
        CheckWhichAnimation(currentEnemyHP); // アニメーションをチェック
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
        else
        {
            GetComponent<EnemyAnimation>().TakeWeakDamage();
        }

    }

    // エネミーショットで豆に当たったとき・BeanShotが他の何かに当たったとき
    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.tag == "EnemyShot")
        {
            currentHit++;
            if (currentHit >= hitThreshold)
            {
                Destroy(gameObject);
                return;
            }
            else if (other.gameObject.tag == "enemy")
            {
                rb.velocity *= enemyBounceRate; // 衝突時に少し速度を上げる
            }
        }
        if (other.gameObject.tag == "BeanShot")
        {

            ApplyDamage(enemyDamage);
        }
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

    public float GetMaxEnemyHP() => enemyHP; // 敵の最大HPを取得
    public float GetCurrentEnemyHP() => currentEnemyHP; // 敵の現在のHPを取得
    public float GetEnemyDamage() => enemyDamage; // 敵の攻撃力を取得
}
