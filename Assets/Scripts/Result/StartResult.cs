using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartResult : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance_AudioManager.PlayBGM(0);
        //AudioManager.instance_AudioManager.PlaySE(13);
    }
}
