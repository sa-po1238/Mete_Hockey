using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private Selecter selecter;

    private bool isOptionWindowActive = false;

    public bool IsOptionWindowActive
    {
        get { return isOptionWindowActive; }
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
        // ボタンからフォーカスを外す
        EventSystem.current.SetSelectedGameObject(null);
        OpenOption();
    }

    private void OpenOption()
    {
        isOptionWindowActive = !isOptionWindowActive;   // トグル状態を反転
        optionWindow.SetActive(isOptionWindowActive);   // オプションウィンドウを表示/非表示にする

        selecter.enabled = !isOptionWindowActive;

        if (isOptionWindowActive)
        {
            var seSliderObj = optionWindow.transform.Find("SESlider");
            var bgmSliderObj = optionWindow.transform.Find("BGMSlider");

            if (seSliderObj != null)
            {
                AudioManager.instance_AudioManager.RegisterSESlider(seSliderObj.GetComponent<Slider>());
            }

            if (bgmSliderObj != null)
            {
                AudioManager.instance_AudioManager.RegisterBGMSlider(bgmSliderObj.GetComponent<Slider>());
            }
        }

        AudioManager.instance_AudioManager.PlaySE(0);
    }
}
