using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private List<AudioSource> SESources = new List<AudioSource>(); // SE用のAudioSourceリスト
    [SerializeField] private AudioSource BGMSource;
    [SerializeField] private int maxSE = 10; // 最大同時再生数
    [SerializeField] private Slider SESlider;
    [SerializeField] private Slider BGMSlider;

    void Start()
    {
        AudioSource[] tmp = this.GetComponents<AudioSource>();
        this.BGMSource = tmp[0];

        // SE用のAudioSourceを作成
        for (int i = 0; i < maxSE; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            SESources.Add(newSource);
        }

        CheckOverlap(this.audioData.SE_Data, "SE_Data");
        CheckOverlap(this.audioData.BGM_Data, "BGM_Data");

        //田口がif文を追加してやりました
        if (SESlider != null)
        {
            SetSESlider();
        }
        if (BGMSlider != null)
        {
            SetBGMSlider();
        }


        // SESliderのonValueChangedイベントにリスナーを追加
        if (SESlider != null)
        {
            SESlider.onValueChanged.AddListener(delegate { SetSEVolume(); });

            // PlayerPrefsから保存された音量を読み込み
            if (PlayerPrefs.HasKey("SEVolume"))
            {
                float savedVolume = PlayerPrefs.GetFloat("SEVolume");
                SESlider.value = savedVolume;

                // SEリストの各AudioSourceに音量を適用
                foreach (var source in SESources)
                {
                    source.volume = savedVolume;
                }
            }
        }
        else
        {
            //田口がないないしました
            //Debug.LogError("SESlider が設定されていません。");
        }

        // BGMSliderのonValueChangedイベントにリスナーを追加
        if (BGMSlider != null)
        {
            BGMSlider.onValueChanged.AddListener(delegate { SetBGMVolume(); });

            // PlayerPrefsから保存された音量を読み込み
            if (PlayerPrefs.HasKey("BGMVolume"))
            {
                float savedVolume = PlayerPrefs.GetFloat("BGMVolume");
                BGMSlider.value = savedVolume;
                BGMSource.volume = savedVolume;
            }
        }
        else
        {
            //田口がないないしました
            //Debug.LogError("BGMSlider が設定されていません。");
        }
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
        if (source != null)
        {
            source.clip = audioData.SE_Data[index].clip;
            // SEの音量をSliderから取得
            source.volume = SESlider != null ? SESlider.value : audioData.SE_Data[index].volume;
            source.volume = audioData.SE_Data[index].volume;
            source.Play();
        }
    }

    private AudioSource GetAvailableSESource()
    {
        foreach (AudioSource source in SESources)
        {
            if (source != null && !source.isPlaying) return source;
        }

        // すべてのAudioSourceが使用中なら、一番古いものを使う
        return SESources[0];
    }

    public void StopSE(int id)
    {
        foreach (AudioSource source in SESources)
        {
            if (source.isPlaying && source.clip == audioData.SE_Data[ConvertIdIntoIndex(audioData.SE_Data, id)].clip)
            {
                source.Stop();
            }
        }
    }

    public void StopAllSE()
    {
        foreach (AudioSource source in SESources)
        {
            source.Stop();
        }
    }

    public void SetSESlider()
    {
        // 先頭のSESourceの音量をSliderに適用
        if (SESources.Count > 0)
        {
            SESlider.value = SESources[0].volume;
        }
    }

    public void SetSEVolume()
    {
        if (SESources == null || SESlider == null) return;

        // Sliderの値をSEの音量に反映
        foreach (AudioSource source in SESources)
        {
            if (source != null)
            {
                source.volume = SESlider.value;
            }
        }

        // PlayerPrefsに音量を保存
        PlayerPrefs.SetFloat("SEVolume", SESlider.value);
        PlayerPrefs.Save();
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

    public void SetBGMSlider()
    {
        BGMSlider.value = this.BGMSource.volume;
    }

    public void SetBGMVolume()
    {
        if (BGMSource == null || BGMSlider == null) return;

        // Sliderの値をBGMの音量に反映
        this.BGMSource.volume = BGMSlider.value;

        // PlayerPrefsに音量を保存
        PlayerPrefs.SetFloat("BGMVolume", BGMSlider.value);
        PlayerPrefs.Save();
    }
}
