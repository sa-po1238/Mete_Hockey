using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameOverResult : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayAudioSequence());
    }

    private IEnumerator PlayAudioSequence()
    {
        AudioManager.instance_AudioManager.PlaySE(13);

        yield return new WaitForSeconds(2f);

        AudioManager.instance_AudioManager.PlayBGM(0);
    }
}
