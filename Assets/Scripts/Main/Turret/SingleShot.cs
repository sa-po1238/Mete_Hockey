using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour
{
    [SerializeField] private float destroyRightLimit = 12f; // 右側の限界値
    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値
    [SerializeField] private int hitThreshold = 2; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // x方向での範囲チェック
        if ((Mathf.Abs(transform.position.x) > destroyRightLimit) || (Mathf.Abs(transform.position.x) < destroyLeftLimit))
        {
            Destroy(gameObject); // 範囲外に出たら弾丸を破壊
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // 敵に当たったときだけすぐ破壊
        if (other.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }

        // 衝突しすぎたチャージショットを破壊
        currentHit += 1;
        if (currentHit >= hitThreshold)
        {
            Destroy(gameObject);
        }
    }
}
