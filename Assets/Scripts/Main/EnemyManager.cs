using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] float enemySpeed = 10f; //敵のスピード
    [SerializeField] float enemyHP = 20f; //敵のHP
    private float currentHP; // 敵の現在のHP

    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = enemyHP;
    }

    void Start()
    {

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

    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したらダメージ
        currentHP -= 10;
        Debug.Log(currentHP);
        // 衝突しても速度を0に保つ
        rb.velocity = Vector3.zero;
    }
}
