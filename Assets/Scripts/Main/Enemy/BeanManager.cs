using UnityEngine;

public class BeanManager : MonoBehaviour
{
    [SerializeField] EnemyData enemyData; // 敵データ
    private float enemyHP; //敵のHP
    private float enemyDamage; //敵の攻撃力、エネミーショットのダメージ
    private float enemyHPRate; // エネミーショットで回復する倍率
    private float currentEnemyHP; // 敵の現在のHP

    [SerializeField] float singleDamage = 5f; //シングルショットのダメージ
    [SerializeField] float chargeDamage = 30f; //チャージショットのダメージ
    [SerializeField] float enemyShotRate = 1.2f; // エネミーショットで加速する倍率
    [SerializeField] float enemyBounceRate = 1.05f; // 敵と当たったときにちょっと加速する
    [SerializeField] private int hitThreshold = 20; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    private Rigidbody rb;
    private Collider col;
    [SerializeField] float explosionDamage = 50f; // 爆発のダメージ

    private void Awake()
    {
        // EnemyData から値を取得
        enemyHP = enemyData.enemyHP;
        enemyDamage = enemyData.enemyDamage;
        enemyHPRate = enemyData.enemyHPRate;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        col.isTrigger = true; // IsTriggerのチェックを入れる        
    }

    private void Start()
    {
        currentEnemyHP = GetComponent<EnemyManager>().GetCurrentEnemyHP();
    }
    private void FixedUpdate()
    {
        // BeanShotになったら減衰
        if (this.gameObject.tag == "BeanShot")
        {
            rb.velocity *= 0.92f;
        }
    }


    private void OnTriggerEnter(Collider other)
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
            //Rotation.zを30か-30にする
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30, 30));
        }
        else if (other.gameObject.tag == "BeanShot")
        {
            if (this.gameObject.tag == "bean")
            {
                Destroy(gameObject);
            }
            else
            {
                currentEnemyHP -= enemyDamage; // 豆のエネミーショットと同じダメージ
                CheckWhichAnimation(currentEnemyHP);
            }
        }
        else if (other.gameObject.tag == "SingleShot")
        {
            currentEnemyHP -= singleDamage;
            CheckWhichAnimation(currentEnemyHP);
        }
        else if (other.gameObject.tag == "ChargeShot")
        {
            currentEnemyHP -= chargeDamage;
            // チャージショットで倒れたらエネミーショットに
            if (currentEnemyHP <= 0)
            {
                Debug.Log("チャージショットでmame ga 倒れた");
                rb.isKinematic = false; // 物理演算を受けるようにする

                // 衝突したチャージショットから5フレーム前の速度をもらう
                ChargeShot chargeShot = other.gameObject.GetComponent<ChargeShot>();
                rb.velocity = chargeShot.GetChargeShotVelocity();

                Debug.Log(rb.velocity);

                col.isTrigger = false; // IsTriggerのチェックを外す
                currentEnemyHP = enemyHP * enemyHPRate; // 元のHPの倍数に回復
                rb.velocity *= enemyShotRate; // 衝突時に速度を上げる

                Debug.Log(currentEnemyHP);

                this.gameObject.tag = "EnemyShot";
                // 弾化のアニメーション
                GetComponent<EnemyAnimation>().DieForStrong();
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
        else if ((other.gameObject.tag == "Turret") || (other.gameObject.tag == "Bunker_a") || (other.gameObject.tag == "Bunker_b"))
        {
            Destroy(gameObject);
        }

        // EnemyManagerに現在のHPを渡す
        GetComponent<EnemyManager>().UpdateEnemyHP(currentEnemyHP);
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

}
