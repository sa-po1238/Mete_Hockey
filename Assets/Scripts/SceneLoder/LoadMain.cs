using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainScene()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
        AudioManager.instance_AudioManager.StopBGM();
        AudioManager.instance_AudioManager.PlayBGM(1);
        SceneManager.LoadScene("Main");
    }
}
