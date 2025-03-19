using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] GameObject explosionPrefab; // 爆発のPrefab
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 爆発処理
    private IEnumerator ExplosionEffect()
    {
        yield return new WaitForSeconds(1f); // 1秒待つ
        Debug.Log("1秒後の処理");
        // 爆発のインスタンスを作成
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void Explosion()
    {
        rb.velocity *= 0.1f;
        this.gameObject.tag = "Explosion";

        Debug.Log("爆発処理");
        // 数秒後に処理を実行
        StartCoroutine(ExplosionEffect());
        rb.isKinematic = true;
    }
}
