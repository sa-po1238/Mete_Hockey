using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour
{
    [SerializeField] GameObject optionWindow;
    [SerializeField] GameObject optionButton;
    //[SerializeField] Sprite optionButtonSprite;
    //[SerializeField] Sprite backButtonSprite;

    private bool isOptionWindowActive = false;

    public bool IsOptionWindowActive
    {
        get { return isOptionWindowActive; }
    }

    void Start()
    {

    }

    void Update()
    {
        // メニューが開いているときはスペースキーの入力を無視する
        if (isOptionWindowActive && Input.GetKeyDown(KeyCode.Space))
        {
            // スペースキーの入力を無視
            return;
        }
    }

    public void OnClickOptionButton()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        if (isOptionWindowActive)
        {
            //GetComponent<Image>().sprite = optionButtonSprite;
            optionWindow.SetActive(false);
            isOptionWindowActive = false;
        }
        else
        {
            optionWindow.SetActive(true);
            //GetComponent<Image>().sprite = backButtonSprite;
            isOptionWindowActive = true;
        }

        // ボタンからフォーカスを外す
        EventSystem.current.SetSelectedGameObject(null);
    }
}
