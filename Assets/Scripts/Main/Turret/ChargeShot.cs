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
    private List<Vector3> velocityHistory = new List<Vector3>(); // 速度履歴を保存するリスト
    private int historySize = 5; // 保存するフレーム数（5フレーム前）
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // 初期状態でリストを埋める（ゼロベクトルで初期化）
        for (int i = 0; i < historySize; i++)
        {
            velocityHistory.Add(Vector3.zero);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        // 現在の速度をリストに追加する前に、リストが最大サイズを超えていないか確認
        if (velocityHistory.Count >= historySize)
        {
            velocityHistory.RemoveAt(0); // 最も古いデータを削除
        }

        // 現在の速度をリストに追加
        velocityHistory.Add(rb.velocity);

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
        // バンカーと敵に当たったときだけ耐える
        if ((other.gameObject.tag == "Bunker") || (other.gameObject.tag == "enemy"))
        {
            // 衝突しすぎたチャージショットを破壊
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
        // 衝突しすぎたチャージショットを破壊
        currentHit += 1;
        if (currentHit >= hitThreshold)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetChargeShotVelocity()
    {
        return velocityHistory[0];
    }

}
