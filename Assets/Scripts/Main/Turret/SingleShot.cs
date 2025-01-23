using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour
{
    [SerializeField] private float destroyRightLimit = 12f; // 右側の限界値
    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    void Update()
    {
        // x方向での範囲チェック
        if ((Mathf.Abs(transform.position.x) > destroyRightLimit) || (Mathf.Abs(transform.position.x) < destroyLeftLimit))
        {
            Destroy(gameObject); // 範囲外に出たら弾丸を破壊
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突したら弾丸を削除
        Destroy(gameObject);
    }
}
