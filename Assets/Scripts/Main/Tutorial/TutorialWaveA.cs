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
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[0]))
            {
                waveInitialized[0] = true; // 一度だけ実行されるように
                text = "あなたが操縦するのは左のタレットです";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(0);
            }
        }
        if (WaveAFlags[1])
        {
            if ((!coroutineStarted) && (!waveInitialized[1]))
            {
                waveInitialized[1] = true; // 一度だけ実行されるように
                text = "WSで上下に移動できます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 上下移動のチュートリアル
            else if (((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S))) && textWriter.isTextFinished)
            {
                StartCoroutine(ShowOKAndChangeWave(1));
            }
        }
        if (WaveAFlags[2])
        {
            if ((!coroutineStarted) && (!waveInitialized[2]))
            {
                waveInitialized[2] = true; // 一度だけ実行されるように
                text = "ADでタレットの回転ができます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }
            // 回転のチュートリアル
            else if (((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D))) && textWriter.isTextFinished)
            {
                StartCoroutine(ShowOKAndChangeWave(2));
            }
        }
        if (WaveAFlags[3])
        {
            if (textWriter.isTextFinished)
            {
                textWriter.ShowTriangle(); // triangleを表示する
            }
            if ((!coroutineStarted) && (!waveInitialized[3]))
            {
                waveInitialized[3] = true; // 一度だけ実行されるように
                text = "基本操作完了 弾は自動で発射されます";
                StartCoroutine(StartTutorialAndSetFlag(text));
            }

            // 表示が完了していたらスペースキーを受け付ける
            else if (Input.GetKeyDown(KeyCode.Space) && textWriter.isTextFinished)
            {
                textWriter.HideTriangle(); // triangleを非表示にする
                WaveChange(3);
            }
        }
        if (WaveAFlags[4])
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

    // OKを表示してから次のウェーブに移るためのコルーチン
    private IEnumerator ShowOKAndChangeWave(int currentWaveIndex)
    {
        AudioManager.instance_AudioManager.PlaySE(15); // SEを鳴らす
        coroutineStarted = true;
        yield return StartCoroutine(textWriter.TutorialMessage("OK"));
        yield return new WaitForSeconds(1f); // 必要に応じて時間調整
        WaveChange(currentWaveIndex);
        coroutineStarted = false;
    }
}
