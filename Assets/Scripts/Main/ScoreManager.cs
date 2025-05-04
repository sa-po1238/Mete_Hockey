using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // シングルトンインスタンス

    private int currentScore = 0; // 現在のスコア
    private int currentCombo = 0; // 現在のコンボ
    private Coroutine comboResetCoroutine; // コンボリセット用コルーチン

    [SerializeField] float comboCoolTime = 1.0f; //コンボを続かせるためのクールタイム

    [SerializeField] GameObject scoreObject;
    [SerializeField] GameObject comboObject;
    [SerializeField] GameObject comboTextObject;    // ”Combo”の表示オブジェクト

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI comboText;

    // 突貫工事
    [SerializeField] GameObject playerFace;
    private Animator playerFaceAnimator; //プレイヤーの顔のアニメーター

    private void Awake()
    {
        playerFaceAnimator = playerFace.GetComponent<Animator>();
    }

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

        // スコア数, コンボ数を表示
        UpdateScoreDisplay();
        UpdateComboDisplay();
    }

    public void AddScore(int enemyScore)
    {
        // コンボの増加
        currentCombo++;
        // アニメーションとSE
        if (currentCombo == 4) playerFaceAnimator.SetTrigger("isCombo");
        if (currentCombo >= 4)
        {
            AudioManager.instance_AudioManager.PlaySE(12);
        }
        else{
            AudioManager.instance_AudioManager.PlaySE(currentCombo + 8);
        }
        // コンボ数を表示
        UpdateComboDisplay();

        // スコアの増加
        currentScore += enemyScore * currentCombo;
        UpdateScoreDisplay();

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

        UpdateComboDisplay();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    /// <summary>
    /// スコア用スプライトフォントを更新 (6桁ゼロ埋め)
    /// </summary>
    private void UpdateScoreDisplay()
    {
        // 6桁でゼロ埋め
        SpriteFontUtil.SetSpriteNum(currentScore, scoreText, 6);
    }

    /// <summary>
    /// コンボ用スプライトフォントを更新 (0のとき非表示)
    /// </summary>
    private void UpdateComboDisplay()
    {
        if (currentCombo == 0)
        {
            comboText.text = ""; // コンボ数を非表示
            comboTextObject.SetActive(false);
        }
        else
        {
            comboTextObject.SetActive(true);
            // 桁指定なし
            SpriteFontUtil.SetSpriteNum(currentCombo, comboText, 0);
        }
    }
}
