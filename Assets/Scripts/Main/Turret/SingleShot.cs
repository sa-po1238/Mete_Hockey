using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour
{
    [SerializeField] private float destroyRightLimit = 12f; // 右側の限界値
    [SerializeField] private float destroyLeftLimit = -12f; // 左側の限界値
    [SerializeField] private int hitThreshold = 2; //　衝突回数の閾値
    private int currentHit = 0; // 現在の衝突回数
    [SerializeField] private float speedThreshold = 0.1f; // 速度の閾値
    private float currentTime = 0f; //発射されてからの時間
    private Rigidbody rb;

    private int frameCount = 0; // 生成後のフレームカウント
    private Renderer rend;
    private Color originalColor;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 最初なんか向きおかしくなっちゃうのの突貫工事
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
            Color transparentColor = originalColor;
            transparentColor.a = 0f; // 透明にする
            rend.material.color = transparentColor;
        }
    }
    void Update()
    {
        currentTime += Time.deltaTime;

        // これも突貫工事
        frameCount++;
        // 4フレーム目から元の色に戻す
        if (frameCount == 5 && rend != null)
        {
            rend.material.color = originalColor;
        }

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
        // 向きも速度に合わせて変更する
        float angle = Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg; // XY平面の角度を計算
        float lastAngle = angle - 90;
        if ((-90 <= lastAngle) && (lastAngle <= 90))
        {
            lastAngle = -(angle - 90);
        }
        else
        {
            lastAngle = angle - 90;
        }
        transform.rotation = Quaternion.Euler(0, 0, lastAngle); // Z軸のみ回転
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bunker")
        {
            // 衝突しすぎたショットを破壊
            currentHit += 1;
            if (currentHit >= hitThreshold)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 豆だけこっち
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

}
