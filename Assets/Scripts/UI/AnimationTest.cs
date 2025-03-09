using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ダメージを受けたとき
    public void TakeWeakDamage()
    {
        animator.SetTrigger("isDamaged");
    }

    // 弱ショットで死んだとき
    public void DieForWeak()
    {
        animator.SetTrigger("isDamaged");
        // 死亡アニメーションが終わったらDestroyする
        // なんかいい感じにデストロイしてな
        Destroy(gameObject, 0.40f);
    }

    // チャージショットで死んだとき(弾化する時)
    public void DieForStrong()
    {
        animator.SetTrigger("isBullet");
    }
}
