using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWaveC : MonoBehaviour
{
    [SerializeField] TutorialWaveManager tutorialWaveManager;

    private int nextWaveIndex = 3; // ウェーブCなので固定で3
    [SerializeField] bool[] waveCFlags; // ウェーブC内のフラグたち
    [SerializeField] GameObject textWriterPrefab;
    private TextWriter textWriter; // テキストを表示させるためのスクリプト
    [SerializeField] bool coroutineStarted = false; // コルーチンが開始されたかどうかのフラグ

    private string text; // 表示するテキスト
    [SerializeField] private bool[] waveInitialized; // ウェーブCの初期化フラグ
    private bool isFailed = false; // ウェーブ0が失敗した場合trueになるbool

    void Start()
    {
        tutorialWaveManager = FindObjectOfType<TutorialWaveManager>();
        GameObject textWriterObj = Instantiate(textWriterPrefab);
        textWriter = textWriterObj.GetComponent<TextWriter>();
        waveCFlags = new bool[6]; // ウェーブCのフラグを6つ用意
        // waveCFlagsのすべてをfalseに初期化
        for (int i = 0; i < waveCFlags.Length; i++)
        {
            waveCFlags[i] = false;
        }
        waveCFlags[0] = true; // ウェーブCの最初のフラグを立てる

        // 各Waveの初期化フラグをfalseに
        waveInitialized = new bool[waveCFlags.Length];
    }

    void Update()
    {
        // テキストが表示しきっていないなら何も受け付けない
        if ((coroutineStarted)) return;

        if (waveCFlags[0])
        {
            if ((!coroutineStarted) && (!waveInitialized[0]))
            {
                waveInitialized[0] = true; // 一度だけ実行されるように
                text = "その敵にチャージショットでとどめをさしてください";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 敵を倒すチュートリアル
            // TutorialEnemyC.csで勝手にWave2へ

            // 失敗した場合はもう一度WaveCをやり直す
            if (isFailed)
            {
                // textWriterObjをデストロイする
                textWriter.DestroyTextObject();
                tutorialWaveManager.RestartWaveC();
            }
        }
        if (waveCFlags[1])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[1]))
            {
                waveInitialized[1] = true; // 一度だけ実行されるように
                text = "チャージショットで敵にとどめをさすと";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(1);
            }
        }
        if (waveCFlags[2])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[2]))
            {
                waveInitialized[2] = true; // 一度だけ実行されるように
                text = "他の敵をまきこみながらぶっ飛びます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(2);
            }
        }
        if (waveCFlags[3])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[3]))
            {
                waveInitialized[3] = true; // 一度だけ実行されるように
                text = "上手く利用すれば効率的に敵を倒せます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(3);
            }
        }

        if (waveCFlags[4])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[4]))
            {
                waveInitialized[4] = true; // 一度だけ実行されるように
                text = "以上で研修は終了です";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(4);
            }
        }

        if (waveCFlags[5])
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
        waveCFlags[currentWaveIndex] = false; // 現在のウェーブのフラグを下ろす
        waveCFlags[currentWaveIndex + 1] = true; // 次のウェーブのフラグを立てる
        Debug.Log($"waveCFlags[{currentWaveIndex}]がfalseになり、waveCFlags[{currentWaveIndex + 1}]がtrueになりました。");
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
    public void ChangeFlagC(int index)
    {
        StartCoroutine(ChangeFlagCAfterOK(index));
    }

    private IEnumerator ChangeFlagCAfterOK(int index)
    {
        coroutineStarted = true;
        yield return StartCoroutine(textWriter.TutorialMessage("OK"));
        yield return new WaitForSeconds(1f);
        WaveChange(index);
        coroutineStarted = false;
    }

    // 外部からフラグの状態をチェックするメソッド
    public bool GetFlagC(int index)
    {
        if (index >= 0 && index < waveCFlags.Length)
        {
            return waveCFlags[index];
        }
        return false;
    }
    // Wave0が失敗した場合trueになるbool
    public void IsWave0Failed()
    {
        isFailed = true;
    }

}
