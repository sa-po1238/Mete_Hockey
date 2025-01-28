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
    }

    private void OnCollisionEnter(Collision other)
    {
        // 衝突した対象の種類に応じてダメージが変化
        if (other.gameObject.tag == "SingleShot")
        {
            currentHP -= singleDamage;
        }
        else if (other.gameObject.tag == "ChargeShot")
        {
            currentHP -= chargeDamage;
        }
        /*
        else if (other.gameObject.tag == "EnemyShot")
        {
            currentHP -= enemyDamage;
        }
        */

        Debug.Log(currentHP);
        // 衝突しても速度を0に保つ
        rb.velocity = Vector3.zero;
    }
}
