using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> waves; // ウェーブ一覧
    private int nextWaveIndex = 0; // 次のウェーブ番号
    private GameObject currentWave; // 現在のウェーブ
    void Update()
    {
        // ウェーブが無い、もしくは敵が居なくなった場合
        if (currentWave == null || currentWave.transform.childCount == 0)
        {
            // ウェーブ作成
            currentWave = Instantiate(waves[nextWaveIndex]);
            // 次のウェーブ番号に
            nextWaveIndex++;
            // ウェーブ総数以上になったら繰り返すように余りを求める
            nextWaveIndex %= waves.Count;
        }
    }
}
