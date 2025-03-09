using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] PlayerHPManager playerHPManager;
    private Rigidbody rb;
    [SerializeField] float damageRate = 2.0f;
    [SerializeField] private GameObject damageArea;
    private Animator damageAreaAnimator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // playerHPManager = GetComponent<PlayerHPManager>();
        damageAreaAnimator = damageArea.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 敵に衝突したときのみダメージ
        if ((other.gameObject.tag == "enemy") || (other.gameObject.tag == "Bean") || (other.gameObject.tag == "Bomb"))
        {
            EnemyManager enemy = other.GetComponent<EnemyManager>();
            if (enemy != null)
            {
                float damage = enemy.GetEnemyDamage(); // 敵ごとのダメージを取得
                if (this.gameObject.tag == "Turret")    //タレットがダメージを負う
                {
                    AudioManager.instance_AudioManager.PlaySE(8);
                    damage *= damageRate;
                }
                else if (this.gameObject.tag == "DamageArea")   //プレイヤーがダメージを負う
                {
                    // DamageAreaのSEとアニメーションを再生
                    damageAreaAnimator.SetTrigger("isDamageAreaDamaged");
                    AudioManager.instance_AudioManager.PlaySE(7);
                }
                playerHPManager.TakeDamage(damage);
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // 敵に衝突したときのみダメージ
        if ((other.gameObject.tag == "enemy") || (other.gameObject.tag == "Bean") || (other.gameObject.tag == "Bomb"))
        {
            EnemyManager enemy = other.gameObject.GetComponent<EnemyManager>(); // 修正後
            if (enemy != null)
            {
                float damage = enemy.GetEnemyDamage(); // 敵ごとのダメージを取得
                if (this.gameObject.tag == "Turret")    //タレットがダメージを負う
                {
                    AudioManager.instance_AudioManager.PlaySE(8);
                    damage *= damageRate;
                }
                else if (this.gameObject.tag == "DamageArea")   //プレイヤーがダメージを負う
                {
                    // DamageAreaのSEとアニメーションを再生
                    damageAreaAnimator.SetTrigger("isDamageAreaDamaged");
                    AudioManager.instance_AudioManager.PlaySE(7);
                }
                playerHPManager.TakeDamage(damage);
            }
        }
    }
}
