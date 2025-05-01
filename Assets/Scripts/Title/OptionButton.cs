using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour
{
    [SerializeField] GameObject optionWindow;
    [SerializeField] GameObject optionButton;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenOption();
        }
        // メニューが開いているときはスペースキーの入力を無視する
        if (isOptionWindowActive && Input.GetKeyDown(KeyCode.Space))
        {
            // スペースキーの入力を無視
            return;
        }
    }

    public void OnClickOptionButton()
    {
        OpenOption();
        // ボタンからフォーカスを外す
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OpenOption()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        if (isOptionWindowActive)
        {
            optionWindow.SetActive(false);
            isOptionWindowActive = false;
        }
        else
        {
            optionWindow.SetActive(true);
            isOptionWindowActive = true;
        }
    }
}
