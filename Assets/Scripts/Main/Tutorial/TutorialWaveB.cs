using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaveB : MonoBehaviour
{
    [SerializeField] TutorialWaveManager tutorialWaveManager;

    private int nextWaveIndex = 2; // ウェーブBなので固定で2
    [SerializeField] bool[] WaveBFlags; // ウェーブB内のフラグたち
    [SerializeField] GameObject textWriterPrefab;
    private TextWriter textWriter; // テキストを表示させるためのスクリプト
    [SerializeField] bool coroutineStarted = false; // コルーチンが開始されたかどうかのフラグ

    private string text; // 表示するテキスト
    [SerializeField] private bool[] waveInitialized; // ウェーブBの初期化フラグ

    void Start()
    {
        tutorialWaveManager = FindObjectOfType<TutorialWaveManager>();
        GameObject textWriterObj = Instantiate(textWriterPrefab);
        textWriter = textWriterObj.GetComponent<TextWriter>();
        WaveBFlags = new bool[6]; // ウェーブBのフラグを6つ用意
        // WaveBFlagsのすべてをfalseに初期化
        for (int i = 0; i < WaveBFlags.Length; i++)
        {
            WaveBFlags[i] = false;
        }
        WaveBFlags[0] = true; // ウェーブBの最初のフラグを立てる

        // 各Waveの初期化フラグをfalseに
        waveInitialized = new bool[WaveBFlags.Length];
    }

    void Update()
    {
        // テキストが表示しきっていないなら何も受け付けない
        if ((coroutineStarted)) return;

        if (WaveBFlags[0])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[0]))
            {
                waveInitialized[0] = true; // 一度だけ実行されるように
                text = "あなたの任務は敵をすべて倒すことです";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(0);
            }
        }
        if (WaveBFlags[1])
        {
            if ((!coroutineStarted) && (!waveInitialized[1]))
            {
                waveInitialized[1] = true; // 一度だけ実行されるように
                text = "手始めに前方の敵を倒してください";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 敵を倒すチュートリアル
            // TutorialEnemyB.csで勝手にWave2へ
        }
        if (WaveBFlags[2])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[2]))
            {
                waveInitialized[2] = true; // 一度だけ実行されるように
                text = "敵が左端まで進むとダメージを受けてしまいます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(2);
            }
        }
        if (WaveBFlags[3])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[3]))
            {
                waveInitialized[3] = true; // 一度だけ実行されるように
                text = "くれぐれもタレットには当たらないように気を付けて";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(3);
            }
        }

        if (WaveBFlags[4])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[4]))
            {
                waveInitialized[4] = true; // 一度だけ実行されるように
                text = "スペースキー長押しでチャージショットが撃てます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(4);
            }
        }

        if (WaveBFlags[5])
        {
            // 次のウェーブをスポーンするためのフラグを立てる
            tutorialWaveManager.SetTutorialFlag(nextWaveIndex);
            // textWriterObjをデストロイする
            textWriter.DestroyTextObject();
        }
    }

    // 次のフェーズに移すメソッド
    private void WaveChange(int currentWaveIndex)
    {
        WaveBFlags[currentWaveIndex] = false; // 現在のウェーブのフラグを下ろす
        WaveBFlags[currentWaveIndex + 1] = true; // 次のウェーブのフラグを立てる
        Debug.Log($"WaveBFlags[{currentWaveIndex}]がfalseになり、WaveBFlags[{currentWaveIndex + 1}]がtrueになりました。");
    }

    // メッセージが表示しきるまでフラグを立てないための中間コルーチン
    private IEnumerator StartTutorialAndSetFlag(string message)
    {
        coroutineStarted = true;
        yield return StartCoroutine(textWriter.TutorialMessage(message));
        coroutineStarted = false;
    }

    // OKを表示してから次のウェーブに移るためのコルーチン
    private IEnumerator ShowOKAndChangeWave(int currentWaveIndex)
    {
        coroutineStarted = true;
        yield return StartCoroutine(textWriter.TutorialMessage("OK"));
        yield return new WaitForSeconds(1f);
        WaveChange(currentWaveIndex);
        coroutineStarted = false;
    }

    // 外部からフラグをセットするメソッド IEnumeratorを呼ぶと怒られるのでvoidで
    public void ChangeFlagB(int index)
    {
        StartCoroutine(ChangeFlagBAfterOK(index));
    }

    private IEnumerator ChangeFlagBAfterOK(int index)
    {
        coroutineStarted = true;
        yield return StartCoroutine(textWriter.TutorialMessage("OK"));
        yield return new WaitForSeconds(1f);
        WaveChange(index);
        coroutineStarted = false;
    }

    // 外部からフラグの状態をチェックするメソッド
    public bool GetFlagB(int index)
    {
        if (index >= 0 && index < WaveBFlags.Length)
        {
            return WaveBFlags[index];
        }
        return false;
    }

}
