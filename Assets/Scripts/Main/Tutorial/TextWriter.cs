using System.Collections;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] GameObject tutorialTextPrefab;
    private TutorialText tutorialText;
    public GameObject tutorialTextObj;

    public bool isTextFinished = false; // テキスト表示中ならfalseを返す

    void Start()
    {
        tutorialTextObj = Instantiate(tutorialTextPrefab);
        tutorialText = tutorialTextObj.GetComponent<TutorialText>();
    }

    public IEnumerator TutorialMessage(string text)
    {
        isTextFinished = false;

        // テキストを描画
        tutorialText.DrawText(text);

        // 表示が終わるまで待機（クリックとかスペースは見ない）
        while (tutorialText.tutorialTextPlaying)
        {
            yield return null;
        }

        isTextFinished = true;
    }


    // テキストオブジェクトを破棄するメソッド
    public void DestroyTextObject()
    {
        if (tutorialTextObj != null)
        {
            Debug.Log("Destroy対象: " + tutorialTextObj);
            Destroy(tutorialTextObj);
        }
    }
}
