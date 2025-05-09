using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStageSelect : MonoBehaviour
{
    public void LoadStageSelectScene()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelect");
    }
}
