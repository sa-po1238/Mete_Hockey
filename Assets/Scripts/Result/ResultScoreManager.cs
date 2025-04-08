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
    [SerializeField] private int finalScore = 123456;
    [SerializeField] private float countUpInterval = 0.02f;    // スコアカウントアップの間隔
    [SerializeField] private int incrementStep = 100;   // カウントアップの増分

    [Header("ボーナスリスト")]
    [SerializeField] private List<BonusData> bonusList = new List<BonusData>();

    private int displayedScore = 0;

    private void Start()
    {
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
            SpriteFontUtil.SetSpriteNumber(displayedScore, scoreText);
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
            yield return ShowBonusLabel(bonus.label);
            yield return CountUpBonus(bonus.value);
            yield return new WaitForSeconds(0.4f);
        }

        yield return new WaitForSeconds(0.5f);
        ShowRank();
        SaveFinalScore();
    }

    // ボーナスラベル表示
    private IEnumerator ShowBonusLabel(string label)
    {
        bonusLabelText.text = label;
        // SE再生
        yield return new WaitForSeconds(0.8f);
        bonusLabelText.text = "";
    }

    // ボーナスカウント
    private IEnumerator CountUpBonus(int bonus)
    {
        displayedScore += bonus;
        SpriteFontUtil.SetSpriteNumber(displayedScore, scoreText);
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
        if (score >= 150000) return "S";
        if (score >= 100000) return "A";
        if (score >= 50000) return "B";
        return "C";
    }

    // スコアをPlayerPrefsに保存
    private void SaveFinalScore()
    {
        PlayerPrefs.SetInt("LastScore", displayedScore);
        PlayerPrefs.Save();
    }
}
