using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShot : MonoBehaviour
{
    [SerializeField] private float destroyRightLimit = 12f; // 右側の限界値
    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値
    private Rigidbody rb;
    [SerializeField] private float speedThreshold = 0.01f; // 速度の閾値
    private float currentTime = 0f;
    [SerializeField] private int hitThreshold = 3; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    void Update()
    {
        currentTime += Time.deltaTime;

        // x方向での範囲チェック
        if ((Mathf.Abs(transform.position.x) > destroyRightLimit) || (Mathf.Abs(transform.position.x) < destroyLeftLimit))
        {
            Destroy(gameObject); // 範囲外に出たら弾丸を破壊
        }
        // 速度が落ちすぎたオブジェクトを破壊
        if ((rb.velocity.magnitude <= speedThreshold) && (currentTime > 1f))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // 衝突しすぎたチャージショットを破壊
        currentHit += 1;
        if (currentHit >= hitThreshold)
        {
            Destroy(gameObject);
        }
    }


}
