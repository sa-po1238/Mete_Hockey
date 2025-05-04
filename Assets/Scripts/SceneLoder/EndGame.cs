using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void EndGameScene()
    {
# if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタでの停止
# else
        Application.Quit(); // ビルド版での終了
# endif
    }
}
