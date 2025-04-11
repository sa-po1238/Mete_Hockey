using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaveA : MonoBehaviour
{
    [SerializeField] TutorialWaveManager tutorialWaveManager;

    private int nextWaveIndex = 1; // ウェーブAなので固定で1
    [SerializeField] bool[] WaveAFlags; // ウェーブA内のフラグたち


    void Start()
    {
        tutorialWaveManager = FindObjectOfType<TutorialWaveManager>();
        // WaveAFlagsのすべてをfalseに初期化
        for (int i = 0; i < WaveAFlags.Length; i++)
        {
            WaveAFlags[i] = false;
        }
        WaveAFlags[0] = true; // ウェーブAの最初のフラグを立てる
    }

    void Update()
    {
        if (WaveAFlags[0])
        {
            // テキストを送る
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WaveChange(0); // フラグを下ろして次のフラグを立てる
            }
        }
        if (WaveAFlags[1])
        {
            // 上下移動のチュートリアル
            if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S)))
            {
                WaveChange(1);
            }
        }
        if (WaveAFlags[2])
        {
            // テキストを送る
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WaveChange(2);
            }
        }
        if (WaveAFlags[3])
        {
            // 回転のチュートリアル
            if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D)))
            {
                WaveChange(3);
            }
        }
        if (WaveAFlags[4])
        {
            // テキストを送る
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WaveChange(4);
            }
        }
        if (WaveAFlags[5])
        {
            // 次のウェーブをスポーンするためのフラグを立てる
            tutorialWaveManager.SetTutorialFlag(nextWaveIndex);
        }
    }

    // 次のフェーズに移すメソッド
    private void WaveChange(int currentWaveIndex)
    {
        WaveAFlags[currentWaveIndex] = false; // 現在のウェーブのフラグを下ろす
        WaveAFlags[currentWaveIndex + 1] = true; // 次のウェーブのフラグを立てる
        Debug.Log($"WaveAFlags[{currentWaveIndex}]がfalseになり、WaveAFlags[{currentWaveIndex + 1}]がtrueになりました。");
    }
}
