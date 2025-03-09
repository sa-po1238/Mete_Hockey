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

    void Start()
    {
        StartCoroutine(SpawnWave());
        nextWaveIndex = startWave;
    }
    IEnumerator SpawnWave()
    {
        for (; nextWaveIndex < waves.Count; nextWaveIndex++) // waves.Count に達するまで実行
        {
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
        // 全ウェーブが終了したら処理を終える
        Debug.Log("すべてのウェーブが終了しました。");
        yield break;
    }


}
