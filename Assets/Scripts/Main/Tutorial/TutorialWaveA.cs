using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaveA : MonoBehaviour
{
    [SerializeField] TutorialWaveManager tutorialWaveManager;

    private int nextWaveIndex = 1; // ウェーブAなので固定で1
    [SerializeField] bool[] WaveAFlags; // ウェーブA内のフラグたち
    [SerializeField] GameObject textWriterPrefab;
    private TextWriter textWriter; // テキストを表示させるためのスクリプト
    [SerializeField] bool coroutineStarted = false; // コルーチンが開始されたかどうかのフラグ

    private string text; // 表示するテキスト
    [SerializeField] private bool[] waveInitialized; // ウェーブAの初期化フラグ

    void Start()
    {
        tutorialWaveManager = FindObjectOfType<TutorialWaveManager>();
        GameObject textWriterObj = Instantiate(textWriterPrefab);
        textWriter = textWriterObj.GetComponent<TextWriter>();
        // WaveAFlagsのすべてをfalseに初期化
        for (int i = 0; i < WaveAFlags.Length; i++)
        {
            WaveAFlags[i] = false;
        }
        WaveAFlags[0] = true; // ウェーブAの最初のフラグを立てる

        // 各Waveの初期化フラグをfalseに
        waveInitialized = new bool[WaveAFlags.Length];
    }

    void Update()
    {
        // テキストが表示しきっていないなら何も受け付けない
        if ((coroutineStarted)) return;

        if (WaveAFlags[0])
        {
            if ((!coroutineStarted) && (!waveInitialized[0]))
            {
                waveInitialized[0] = true; // 一度だけ実行されるように
                text = "zyougeidou no tyutorial desu";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                WaveChange(0);
            }
        }
        if (WaveAFlags[1])
        {
            if ((!coroutineStarted) && (!waveInitialized[1]))
            {
                waveInitialized[1] = true; // 一度だけ実行されるように
                text = "WS osite miro";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 上下移動のチュートリアル
            else if (((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S))) && textWriter.isTextFinished)
            {
                WaveChange(1);
            }
        }
        if (WaveAFlags[2])
        {

            if ((!coroutineStarted) && (!waveInitialized[2]))
            {
                waveInitialized[2] = true; // 一度だけ実行されるように
                text = "tyutorialA no tyutorial desu";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                WaveChange(2);
            }
        }
        if (WaveAFlags[3])
        {
            if ((!coroutineStarted) && (!waveInitialized[3]))
            {
                waveInitialized[3] = true; // 一度だけ実行されるように
                text = "AD osite miro";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 回転のチュートリアル
            else if (((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D))) && textWriter.isTextFinished)
            {
                WaveChange(3);
            }
        }
        if (WaveAFlags[4])
        {
            if ((!coroutineStarted) && (!waveInitialized[4]))
            {
                waveInitialized[4] = true; // 一度だけ実行されるように
                text = "tyutorialA owari desu";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
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

    // メッセージが表示しきるまでフラグを立てないための中間コルーチン
    private IEnumerator StartTutorialAndSetFlag(string message)
    {
        coroutineStarted = true;
        yield return StartCoroutine(textWriter.TutorialMessage(message));
        coroutineStarted = false;
    }
}
