using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> waves; // ウェーブ一覧
    [SerializeField] int startWave; // 最初のウェーブ(テストプレイ用) 通常プレイは0
    private int nextWaveIndex = 0; // 次のウェーブ番号
    private GameObject currentWave; // 現在のウェーブ
    [SerializeField] private float waveInterval = 15f; // ウェーブ間の間隔
    [SerializeField] private bool loopLastWave = true; // 最後のウェーブをループさせるかどうか
    [SerializeField] private MonoBehaviour notificationTarget; // 通知先スクリプト（任意のクラス）
    private System.Action notifySecondLastWave; // 通知用デリゲート

    void Start()
    {
        // 通知先が設定されていればキャストしてイベントを取得
        if (notificationTarget != null)
        {
            notifySecondLastWave = (System.Action)System.Delegate.CreateDelegate(
                typeof(System.Action),
                notificationTarget,
                "OnSecondLastWave"
            );
        }
        StartCoroutine(SpawnWave());
        nextWaveIndex = startWave;
    }
    IEnumerator SpawnWave()
    {
        for (; nextWaveIndex < waves.Count; nextWaveIndex++) // waves.Count に達するまで実行
        {
            // 最後から2番目のウェーブを生成する直前に通知→BossStart.csボスを出現させる
            if (nextWaveIndex == waves.Count - 2 && notifySecondLastWave != null)
            {
                notifySecondLastWave.Invoke();
            }


            // ウェーブ作成
            currentWave = Instantiate(waves[nextWaveIndex]);

            // 最初のウェーブなら5秒待機
            if (nextWaveIndex == 0)
            {
                yield return new WaitForSeconds(5f);
            }
            else
            {
                yield return new WaitForSeconds(waveInterval);
            }
        }
        Debug.Log("すべてのウェーブが終了しました。");

        // 最後のウェーブをY軸ランダムにループ生成
        if (loopLastWave && waves.Count > 0)
        {
            Debug.Log("最後のウェーブを繰り返し発射します。");
            GameObject lastWave = waves[waves.Count - 1];

            while (true)
            {
                float randomY = Random.Range(-4f, 1f);
                Vector3 spawnPosition = new Vector3(0f, randomY, 0f); // 必要ならX/Zも調整可能
                Instantiate(lastWave, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(waveInterval - 10f);
            }
        }
    }


}
