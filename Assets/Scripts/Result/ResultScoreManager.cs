using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BonusData
{
    public string label;    // 表示名
    public int value;       // ボーナススコア
}

public class ResultScoreManager : MonoBehaviour
{
    [Header("スコア表示")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI bonusLabelText;   // ボーナス名表示用
    
    [Header("スコア設定")]
    [SerializeField] private float countUpInterval = 0.02f;    // スコアカウントアップの間隔
    [SerializeField] private int incrementStep = 100;   // カウントアップの増分

    [Header("ボーナスリスト")]
    [SerializeField] private List<BonusData> bonusList = new List<BonusData>();

    private int finalScore;   // 最終スコア
    private int displayedScore; // 表示中のスコア

    private void Start()
    {
        finalScore = PlayerPrefs.GetInt("Score", 9999);
        SpriteFontUtil.SetSpriteNum(0, scoreText, 0);
        rankText.text = "";
        bonusLabelText.text = "";
        StartCoroutine(CountUpScore(finalScore));
    }

    // スコアカウントアップ
    private IEnumerator CountUpScore(int targetScore)
    {
        while (displayedScore < targetScore)
        {
            displayedScore += incrementStep;
            if (displayedScore > targetScore) {
                displayedScore = targetScore;
            }
            SpriteFontUtil.SetSpriteNum(displayedScore, scoreText, 0);
            // SE再生するならここ
            yield return new WaitForSeconds(countUpInterval);
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayBonusSequence());
    }

    // ボーナス表示
    private IEnumerator PlayBonusSequence()
    {
        foreach (BonusData bonus in bonusList)
        {
            yield return ShowBonusLabel(bonus.label, bonus.value);
            yield return CountUpBonus(bonus.value);
            yield return new WaitForSeconds(0.4f);
        }

        yield return new WaitForSeconds(0.5f);
        ShowRank();
        SaveFinalScore();

        AudioManager.instance_AudioManager.PlayBGM(0);
    }

    // ボーナスラベル表示
    private IEnumerator ShowBonusLabel(string label, int value)
    {
        bonusLabelText.text = label + " +" + value;
        // SE再生
        yield return new WaitForSeconds(0.8f);
        bonusLabelText.text = "";
    }

    // ボーナスカウント
    private IEnumerator CountUpBonus(int bonus)
    {
        displayedScore += bonus;
        SpriteFontUtil.SetSpriteNum(displayedScore, scoreText, 0);
        // ド派手SEなど
        yield return new WaitForSeconds(0.3f);
    }

    // ランク表示
    private void ShowRank()
    {
        string rank = GetScoreRank(displayedScore);
        rankText.text = rank;
    }

    // スコアランク取得
    private string GetScoreRank(int score)
    {
        if (score >= 30000) return "S";
        if (score >= 20000) return "A";
        if (score >= 10000) return "B";
        return "C";
    }

    // スコアをPlayerPrefsに保存
    private void SaveFinalScore()
    {
        PlayerPrefs.SetInt("LastScore", displayedScore);
        PlayerPrefs.Save();
    }
}
