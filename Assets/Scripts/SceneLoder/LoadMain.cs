using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMain : MonoBehaviour
{
    public void LoadMainScene()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        AudioManager.instance_AudioManager.StopBGM();
        AudioManager.instance_AudioManager.PlayBGM(1);
        SceneManager.LoadScene("Main");
    }
}
