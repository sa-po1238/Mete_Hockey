using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Selecter : MonoBehaviour
{
    [Header("選択対象")]
    [Tooltip("Button, Slider, ToggleなどSelectableを並べる")]
    [SerializeField] private List<Selectable> items = new List<Selectable>();

    [Header("カーソル")]
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Vector2 cursorOffset;

    [Header("Input設定")]
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode selectKey = KeyCode.Space;
    [SerializeField] private float sliderStep = 0.1f;

    private int currentIndex = 0;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        bool isSliderSelected = items[currentIndex] is Slider;

        // ↑↓で項目を移動
        if (Input.GetKeyDown(upKey))
        {
            MoveSelection(-1);
        }
        else if (Input.GetKeyDown(downKey))
        {
            MoveSelection(1);
        }

        // ←→で項目を移動
        if (Input.GetKeyDown(leftKey))
        {
            // Sliderなら値を減少、そうでなければ項目を前に移動
            if (isSliderSelected)
            {
                var slider = (Slider)items[currentIndex];
                slider.value = Mathf.Max(slider.minValue, slider.value - sliderStep);
            }
            else
            {
                MoveSelection(-1);
            }
        }
        else if (Input.GetKeyDown(rightKey))
        {
            if (isSliderSelected)
            {
                var slider = (Slider)items[currentIndex];
                slider.value = Mathf.Min(slider.maxValue, slider.value + sliderStep);
            }
            else
            {
                MoveSelection(1);
            }
        }

        // 決定キーで Button の onClick を呼び出し
        if (Input.GetKeyDown(selectKey) && items[currentIndex] is Button btn)
        {
            btn.onClick.Invoke();
        }
    }

    // カーソルの位置を更新する dir: -1:前, 1:次
    private void MoveSelection(int dir)
    {
        int nextIndex = currentIndex + dir;
        currentIndex = Mathf.Clamp(nextIndex, 0, items.Count - 1);
        UpdateCursor();
    }

    // カーソルの位置を更新する
    private void UpdateCursor()
    {
        AudioManager.instance_AudioManager.PlaySE(0);

        var targetRect = items[currentIndex].GetComponent<RectTransform>();
        cursor.anchoredPosition = targetRect.anchoredPosition + cursorOffset;
        EventSystem.current.SetSelectedGameObject(items[currentIndex].gameObject); // 選択を更新
    }
}
