using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true);
    }

    // ダメージを受けたとき
    public void TakeWeakDamage()
    {
        AudioManager.instance_AudioManager.PlaySE(3);
        animator.SetTrigger("isDamaged");
    }

    // 弱ショットで死んだとき
    public void DieForWeak()
    {
        AudioManager.instance_AudioManager.PlaySE(4);
        animator.SetTrigger("isDamaged");
        // 死亡アニメーションが終わったらDestroyする
        // なんかいい感じにデストロイしてな
        Destroy(gameObject, 0.40f);
    }

    // チャージショットで死んだとき(弾化する時)
    public void DieForStrong()
    {
        AudioManager.instance_AudioManager.PlaySE(5);
        animator.SetTrigger("isBullet");
    }

    // 爆発アニメーション
    public void Explosion()
    {
        AudioManager.instance_AudioManager.PlaySE(6);
    }
}
