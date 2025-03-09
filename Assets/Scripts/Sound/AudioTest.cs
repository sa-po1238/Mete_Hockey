using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance_AudioManager.PlayBGM(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE()
    {
        AudioManager.instance_AudioManager.PlaySE(0);
    }
}
