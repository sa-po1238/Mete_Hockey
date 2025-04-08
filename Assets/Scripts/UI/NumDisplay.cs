using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumDisplay : MonoBehaviour
{
    public int spriteNumber;
    public GameObject textDisplay;

    void Start()
    {
        if (textDisplay == null)
        {
            Debug.LogError("textDisplay がセットされていません");
            return;
        }

        TextMeshProUGUI textComponent = textDisplay.GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("textDisplay に TextMeshProUGUI コンポーネントがアタッチされていません");
            return;
        }

        spriteNumber = 12345;
        SetSpriteNumber(spriteNumber, textComponent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpriteNumber(int spriteNumber, TextMeshProUGUI textComponent)
    {
        string spriteText = spriteNumber.ToString();
        textComponent.text = "";
        for (int i = 0; i < spriteText.Length; i++)
        {
            int spriteIndex = int.Parse(spriteText[i].ToString());
            textComponent.text += "<sprite=" + spriteIndex + ">";
        }
    }
}
