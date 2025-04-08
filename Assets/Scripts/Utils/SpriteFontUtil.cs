using TMPro;

public static class SpriteFontUtil
{
    /// <summary>
    /// 数値をTextMeshProでスプライト表示する（ゼロ埋めあり）
    /// </summary>
    /// <param name="number">表示したい数値</param>
    /// <param name="textComponent">TextMeshProUGUI</param>
    /// <param name="digitCount">桁数（ゼロ埋め）</param>
    public static void SetSpriteNumber(int number, TextMeshProUGUI textComponent, int digitCount = 6)
    {
        string spriteText = number.ToString($"D{digitCount}");
        textComponent.text = "";
        foreach (char c in spriteText)
        {
            int spriteIndex = int.Parse(c.ToString());
            textComponent.text += $"<sprite={spriteIndex}>";
        }
    }
}
