using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBgm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance_AudioManager.PlayBGM(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
