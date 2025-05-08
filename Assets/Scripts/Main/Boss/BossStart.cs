using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab; // インスタンス化するボスのプレハブ
    [SerializeField] private Transform spawnPoint;   // ボスを出現させる位置（任意）
    [SerializeField] private float moveDuration = 10f; // 移動にかける時間（秒）

    private bool isStarted = false; // ボスが出現したかどうかのフラグ
    private bool hasSpawned = false; // ボスの生成を一度きりに制限

    public void OnSecondLastWave()
    {
        Debug.Log("最後から2番目のウェーブが始まりました！");
        isStarted = true;
    }

    void Update()
    {
        if (isStarted && !hasSpawned)
        {
            Debug.Log("ボスが出現しました！");
            hasSpawned = true;

            Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
            GameObject bossInstance = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);

            // 移動
            Vector3 targetPosition = new Vector3(0.57f, 0.68f, spawnPosition.z);
            StartCoroutine(MoveBossSmoothly(bossInstance.transform, targetPosition, moveDuration));
        }

    }
    IEnumerator MoveBossSmoothly(Transform bossTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = bossTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            bossTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        bossTransform.position = targetPosition; // 念のためピッタリ合わせる
    }
}
