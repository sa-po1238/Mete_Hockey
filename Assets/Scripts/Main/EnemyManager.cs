using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] float enemySpeed = 10f; //敵のスピード
    [SerializeField] float enemyHP = 20f; //敵のHP
    private float currentHP; // 敵の現在のHP
    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値
    [SerializeField] float singleDamage = 10f; //シングルショットのダメージ
    [SerializeField] float chargeDamage = 20f; //チャージショットのダメージ
    [SerializeField] float enemyDamage = 20f; //敵の攻撃力、エネミーショットのダメージ
    [SerializeField] float enemyShotRate = 1.2f; // エネミーショットで加速する倍率
    [SerializeField] private float speedThreshold = 0.01f; // 速度の閾値

    [SerializeField] private int hitThreshold = 10; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = enemyHP;
    }

    void Update()
    {
        Vector3 movement = new Vector3(-enemySpeed, 0, 0) * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
        // x方向での範囲チェック
        if (transform.position.x <= destroyLeftLimit)
        {
            Destroy(gameObject); // 範囲外に出たら敵を破壊
        }
        // HPチェック
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }

        // 速度が落ちすぎたエネミーショットを破壊
        if ((rb.velocity.magnitude <= speedThreshold) && (this.gameObject.tag == "EnemyShot"))
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        // 衝突しすぎたエネミーショットを破壊
        if (this.gameObject.tag == "EnemyShot")
        {
            currentHit += 1;
            if (currentHit >= hitThreshold)
            {
                Destroy(gameObject);
            }
        }

        // 衝突した対象の種類に応じてダメージが変化
        if (other.gameObject.tag == "SingleShot")
        {
            currentHP -= singleDamage;
            // 衝突しても速度を0に保つ
            rb.velocity = Vector3.zero;
        }
        else if (other.gameObject.tag == "ChargeShot")
        {
            currentHP -= chargeDamage;
            // チャージショットで倒れたらエネミーショットに
            if (currentHP <= 0)
            {
                currentHP = 9999;
                rb.velocity *= enemyShotRate; // 衝突時に速度を上げる
                this.gameObject.tag = "EnemyShot";
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else if (other.gameObject.tag == "EnemyShot")
        {
            currentHP -= enemyDamage;
        }
        else if (other.gameObject.tag == "Turret")
        {
            Destroy(gameObject);

        }
        Debug.Log(currentHP);
    }
}
