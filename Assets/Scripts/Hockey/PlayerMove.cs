using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 6.0f;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        Vector3 force = Vector3.zero;

        // 前後移動
        if (Input.GetKey("w"))
        {
            force += transform.forward * speed;
        }
        if (Input.GetKey("s"))
        {
            force -= transform.forward * speed;
        }

        // 左右移動
        if (Input.GetKey("d"))
        {
            force += transform.right * speed;
        }
        if (Input.GetKey("a"))
        {
            force -= transform.right * speed;
        }
        // 力を加える
        rb.AddForce(force, ForceMode.Force);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // プレイヤーの速度をゼロに保つ
            rb.velocity = Vector3.zero;
        }
    }
}
