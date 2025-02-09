using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHPManager : MonoBehaviour
{
    [SerializeField] float playerHP = 100f; //プレイヤーのHP
    private float currentHP; //プレイヤーの現在のHP
    [SerializeField] float enemyDamage = 20f; //敵の攻撃力、エネミーショットのダメージ


    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHP = playerHP;
    }
    private void Update()
    {
        if (currentHP <= 0)
        {
            Debug.Log("GameOver");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 敵に衝突したときのみダメージ
        if (other.gameObject.tag == "enemy")
        {
            currentHP -= enemyDamage;
        }
        Debug.Log(currentHP);

    }
}
