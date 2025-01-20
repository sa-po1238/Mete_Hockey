using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float speed = 6;
    private Vector3 pos;
    bool One;
    // Use this for initialization
    void Start()
    {
        One = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (One)
        {
            if (Input.GetKey("w"))
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                transform.position -= transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                var force = (transform.forward + transform.right) * 3.5f;
                GetComponent<Rigidbody>().AddForce(force, ForceMode.VelocityChange);
                One = false;
            }
            Clamp();
        }

        // ★追加
        // プレーヤーの移動できる範囲を制限する命令ブロック
        void Clamp()
        {
            // プレーヤーの位置情報を「pos」という箱の中に入れる。
            pos = transform.position;

            pos.z = Mathf.Clamp(pos.z, -4, 4);

            transform.position = pos;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = rigidbody.velocity * 1.13f;
        }
    }
}
