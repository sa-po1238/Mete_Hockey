using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI talkText;
    public bool tutorialTextPlaying = false;
    [SerializeField] float textSpeed = 0.1f;

    // テキストを生成する関数
    public void DrawText(string text)
    {
        StartCoroutine("CoDrawText", text);
    }

    // テキストがヌルヌル出てくるためのコルーチン
    IEnumerator CoDrawText(string text)
    {
        tutorialTextPlaying = true;
        float time = 0;
        while (true)
        {
            yield return 0;
            time += Time.deltaTime;

            int len = Mathf.FloorToInt(time / textSpeed);
            if (len > text.Length) break;
            talkText.text = text.Substring(0, len);
        }
        talkText.text = text;
        yield return 0;
        tutorialTextPlaying = false;
    }
}
