using TMPro;

public static class SpriteFontUtil
{
    /// <summary>
    /// 数値をTextMeshProでスプライト表示する（任意でゼロ埋めあり）
    /// </summary>
    /// <param name="num">表示したい数値</param>
    /// <param name="textComponent">TextMeshProUGUI</param>
    /// <param name="digitCount">ゼロ埋め桁数（0なら埋めない）</param>
    public static void SetSpriteNum(int num, TextMeshProUGUI textComponent, int digitCount = 6)
    {
        string spriteText;

        if (digitCount > 0)
        {
            // 例: num = 42 → "000042"
            spriteText = num.ToString($"D{digitCount}");
        }
        else
        {
            // 桁数指定なし: そのまま表示
            spriteText = num.ToString();
        }

        textComponent.text = "";
        foreach (char c in spriteText)
        {
            int spriteIndex = int.Parse(c.ToString());
            textComponent.text += $"<sprite={spriteIndex}>";
        }
    }
}
