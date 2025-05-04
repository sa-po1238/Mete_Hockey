using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> waves; // ウェーブ一覧
    [SerializeField] int startWave; // 最初のウェーブ(テストプレイ用) 通常プレイは0
    private int nextWaveIndex = 0; // 次のウェーブ番号
    private GameObject currentWave; // 現在のウェーブ
    [SerializeField] bool[] tutorialFlags; // 各ウェーブごとのチュートリアルフラグ

    void Start()
    {
        nextWaveIndex = startWave;
    }

    void Update()
    {
        /*
        // フラグONテスト：1, 2, 3キーを押したときに対応するフラグをON
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetTutorialFlag(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetTutorialFlag(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetTutorialFlag(2);
        }
        */
        SpawnTutorialWave();

    }

    // 外部からフラグをセットするメソッド
    public void SetTutorialFlag(int index)
    {
        if (index >= 0 && index < tutorialFlags.Length)
        {
            tutorialFlags[index] = true;
            Debug.Log($"tutorialFlags[{index}] を true にしました。");
        }
    }

    void SpawnTutorialWave()
    {
        if (nextWaveIndex < waves.Count && tutorialFlags.Length > nextWaveIndex)
        {
            // tutorialFlagsがTRUEならばnextWaveIndexのウェーブをスポーン
            if (tutorialFlags[nextWaveIndex])
            {
                currentWave = Instantiate(waves[nextWaveIndex]);
                tutorialFlags[nextWaveIndex] = false; // 一度だけ実行するように
                Debug.Log($"Wave {nextWaveIndex} をスポーンしました。");
                nextWaveIndex++;
            }
        }
        else if (nextWaveIndex >= waves.Count)
        {
            //Debug.Log("すべてのウェーブが終了しました。");
        }
    }

    // WaveCをデストロイして作りなおすメソッド
    public void RestartWaveC()
    {
        if (currentWave != null)
        {
            Destroy(currentWave);
            Debug.Log("WaveCをデストロイしました。");
        }
        // WaveCをもう一度開始
        nextWaveIndex = 2; // ウェーブ番号をリセット
        tutorialFlags[2] = true; // ウェーブCのフラグを立てる
    }
}
