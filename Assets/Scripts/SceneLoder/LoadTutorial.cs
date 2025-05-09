using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTutorial : MonoBehaviour
{
    public void LoadTutorialScene()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        AudioManager.instance_AudioManager.StopBGM();
        AudioManager.instance_AudioManager.PlayBGM(1);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }
}
