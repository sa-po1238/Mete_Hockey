using System.Collections;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [SerializeField] GameObject tutorialTextPrefab;
    private TutorialText tutorialText;
    public GameObject tutorialTextObj;
    public bool isTextFinished = false; // テキスト表示中ならfalseを返す
    [SerializeField] GameObject trianglePrefab; // triangleのPrefab
    private GameObject triangle; // triangleのインスタンス

    void Start()
    {
        tutorialTextObj = Instantiate(tutorialTextPrefab);
        tutorialText = tutorialTextObj.GetComponent<TutorialText>();
        triangle = Instantiate(trianglePrefab); // triangleのインスタンスを生成
        triangle.SetActive(false); // triangleを非表示にする
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

    // triangleを表示するメソッド
    public void ShowTriangle()
    {
        if (triangle != null)
        {
            triangle.SetActive(true);
        }
    }
    // triangleを非表示にするメソッド
    public void HideTriangle()
    {
        Debug.Log("triangleを非表示にします。");
        if (triangle != null)
        {
            triangle.SetActive(false);
        }
    }
}
