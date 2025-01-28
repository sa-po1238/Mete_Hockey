using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Rigidbody rb;     // Rigidbodyの参照
    [SerializeField] float slowRate; //減速する倍率
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        // 減速
        rb.velocity *= slowRate;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity *= 1.13f; // 衝突時に速度を1.13倍に
        }
    }
}
