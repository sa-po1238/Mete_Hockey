using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] PlayerHPManager playerHPManager;
    private Rigidbody rb;
    [SerializeField] float damageRate = 2.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // playerHPManager = GetComponent<PlayerHPManager>();

    }
    private void OnTriggerEnter(Collider other)
    {
        // 敵に衝突したときのみダメージ
        if (other.gameObject.tag == "enemy")
        {
            EnemyManager enemy = other.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                float damage = enemy.GetEnemyDamage(); // 敵ごとのダメージを取得
                playerHPManager.TakeDamage(damage);
                Debug.Log($"これ：{this.gameObject.tag}");
            }

        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // 敵に衝突したときのみダメージ
        if (other.gameObject.tag == "enemy")
        {
            EnemyManager enemy = other.gameObject.GetComponent<EnemyManager>(); // 修正後
            if (enemy != null)
            {
                float damage = enemy.GetEnemyDamage(); // 敵ごとのダメージを取得
                damage *= damageRate;
                playerHPManager.TakeDamage(damage);
                Debug.Log($"これ：{this.gameObject.tag}");
            }

        }
    }

}
