using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance_AudioManager;

    private void Awake()
    {
        if (instance_AudioManager == null)
        {
            instance_AudioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private AudioData audioData;

    private List<AudioSource> SE_Sources = new List<AudioSource>(); // SE用のAudioSourceリスト
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private int maxSE = 10; // 最大同時再生数

    void Start()
    {
        AudioSource[] tmp = this.GetComponents<AudioSource>();
        this.BGMSource = tmp[0];

        // SE用のAudioSourceを作成
        for (int i = 0; i < maxSE; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            SE_Sources.Add(newSource);
        }

        CheckOverlap(this.audioData.SE_Data, "SE_Data");
        CheckOverlap(this.audioData.BGM_Data, "BGM_Data");
    }

    private void CheckOverlap(List<Datum> data, string variable_name)
    {
        List<int> vs = new List<int>();
        for (int i = 0; i < data.Count; i++)
        {
            if (vs.Contains(data[i].id))
            {
                Debug.LogError(string.Format("{0} のID {1} が重複しています。", variable_name, data[i].id));
            }
            else
            {
                vs.Add(data[i].id);
            }
        }
    }

    public int ConvertIdIntoIndex(List<Datum> data, int id)
    {
        for (int index = 0; index < data.Count; index++)
        {
            if (id == data[index].id)
            {
                return index;
            }
        }

        Debug.LogError(string.Format("指定されたid {0} のデータは存在しません。", id));
        return -1;
    }

    public void PlaySE(int id)
    {
        int index = ConvertIdIntoIndex(audioData.SE_Data, id);
        if (index == -1) return;

        // 空いているAudioSourceを探す
        AudioSource source = GetAvailableSESource();
        source.clip = audioData.SE_Data[index].clip;
        source.volume = audioData.SE_Data[index].volume;
        source.Play();
        Debug.Log("SE再生");
    }

    private AudioSource GetAvailableSESource()
    {
        foreach (AudioSource source in SE_Sources)
        {
            if (!source.isPlaying) return source;
        }

        // すべてのAudioSourceが使用中なら、一番古いものを使う
        return SE_Sources[0];
    }

    public void StopSE(int id)
    {
        foreach (AudioSource source in SE_Sources)
        {
            if (source.isPlaying && source.clip == audioData.SE_Data[ConvertIdIntoIndex(audioData.SE_Data, id)].clip)
            {
                source.Stop();
            }
        }
    }

    public void StopAllSE()
    {
        foreach (AudioSource source in SE_Sources)
        {
            source.Stop();
        }
    }

    public void PlayBGM(int id)
    {
        int index = ConvertIdIntoIndex(audioData.BGM_Data, id);
        if (index == -1) return;

        BGMSource.clip = audioData.BGM_Data[index].clip;
        BGMSource.volume = audioData.BGM_Data[index].volume;
        BGMSource.Play();
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }
}
