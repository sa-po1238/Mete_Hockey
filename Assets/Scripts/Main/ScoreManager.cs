using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // シングルトンインスタンス
    private int currentScore = 0; // 現在のスコア
    private int currentCombo = 0; // 現在のコンボ
    private Coroutine comboResetCoroutine; // コンボリセット用コルーチン
    [SerializeField] float comboCoolTime = 1.0f; //コンボを続かせるためのクールタイム
    [SerializeField] GameObject scoreObject;
    private TextMeshProUGUI scoreText;
    [SerializeField] GameObject comboObject;
    private TextMeshProUGUI comboText;
    private void Start()
    {

        // シングルトンの設定
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Destroy(gameObject);
        }


        // オブジェクトからTextコンポーネントを取得
        scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
        comboText = comboObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // UIの表示を変える
        scoreText.text = $"SCORE:{currentScore}";
        comboText.text = $"COMBO:{currentCombo}";
    }

    public void AddScore(int enemyScore)
    {
        Debug.Log("AddScore");
        // コンボとスコアの増加
        currentCombo++;
        int addScore = enemyScore * currentCombo;

        currentScore += addScore;
        Debug.Log("スコア: " + currentScore);

        // 以前のコンボリセット処理をキャンセル
        if (comboResetCoroutine != null)
        {
            StopCoroutine(comboResetCoroutine);
        }

        // 新しいコンボリセット処理を開始
        comboResetCoroutine = StartCoroutine(ResetCombo());
    }

    // IEnumerator:途中で待機できる関数
    private IEnumerator ResetCombo()
    {
        // ResetComboが呼ばれる
        yield return new WaitForSeconds(comboCoolTime);
        // comboCoolTime分だけここで待つ
        // この間にStopCoroutineが起きたらこの処理がここで止まる
        currentCombo = 0;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
