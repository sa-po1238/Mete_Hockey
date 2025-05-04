using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance_AudioManager;
    [SerializeField] private AudioData audioData;
    [SerializeField] private int maxSE = 10;    // 最大同時再生数
    [SerializeField] private AudioMixerGroup SEAudioMixerGroup; // SE用 AudioMixerGroup

    private List<AudioSource> SESources = new List<AudioSource>();  // SE用 AudioSource リスト
    [SerializeField] private AudioSource BGMSource; // BGM用 AudioSource

    [SerializeField] private Slider SESlider;
    [SerializeField] private Slider BGMSlider;

    void Awake()
    {
        if (instance_AudioManager == null)
        {
            instance_AudioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        var tmp = this.GetComponents<AudioSource>();
        BGMSource = tmp[0];

        // SE用のAudioSourceを作成
        for (int i = 0; i < maxSE; i++)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.outputAudioMixerGroup = SEAudioMixerGroup; // AudioMixerGroupを設定
            SESources.Add(src);
        }

        // IDの重複チェック
        CheckOverlap(this.audioData.SE_Data, "SE_Data");
        CheckOverlap(this.audioData.BGM_Data, "BGM_Data");

        if (SESlider != null) InitSESlider();
        if (BGMSlider != null) InitBGMSlider();
    }
    
    // Menuを開いたときにスライダーを探してイベント登録
    public void RegisterSESlider(Slider slider)
    {
        SESlider = slider;
        InitSESlider();
    }

    public void RegisterBGMSlider(Slider slider)
    {
        BGMSlider = slider;
        InitBGMSlider();
    }

    private void InitSESlider()
    {
        SESlider.onValueChanged.RemoveAllListeners();   // 以前登録されてたリスナーを消す
        SESlider.onValueChanged.AddListener(_ => SetSEVolume());    // スライダーの値が変更されたときに呼ぶリスナーを追加

        float saved = PlayerPrefs.HasKey("SEVolume") ? PlayerPrefs.GetFloat("SEVolume") : (SESources != null ? SESources[0].volume : 1f); // PlayerPrefsから音量を取得、なければ先頭のSEの音量、なければ1
        SESlider.value = saved; // スライダーの表示値としてセット
        // SEの全AudioSourceに適用
        foreach (var src in SESources)
        {
            src.volume = saved;
        }
    }

    private void InitBGMSlider()
    {
        BGMSlider.onValueChanged.RemoveAllListeners();
        BGMSlider.onValueChanged.AddListener(_ => SetBGMVolume());

        float saved = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : (BGMSource != null ? BGMSource.volume : 1f); // PlayerPrefsから音量を取得、なければBGMの音量、なければ1
        BGMSlider.value = saved;
        BGMSource.volume = saved;
    }

    private void CheckOverlap(List<Datum> data, string variable_name)
    {
        var ids = new HashSet<int>();
        foreach (var datum in data)
        {
            if (!ids.Add(datum.id))
            {
                Debug.LogError($"{variable_name} のID {datum.id} が重複しています。");
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
        int index = ConvertIdIntoIndex(audioData.SE_Data, id);  // SEのIDからインデックスを取得
        if (index < 0) return;  // インデックスが無効な場合は終了

        // 空いているAudioSourceを探す
        var src = GetAvailableSESource();
        src.clip   = audioData.SE_Data[index].clip;   // AudioSourceにクリップをセット
        src.volume = (SESlider != null) ? SESlider.value : audioData.SE_Data[index].volume;   // 音量をセット
        src.Play();
    }

    private AudioSource GetAvailableSESource()
    {
        foreach (var src in SESources)
        {
            if (!src.isPlaying) return src;
        }

        // すべてのAudioSourceが使用中なら、一番古いものを使う
        return SESources[0];
    }

    public void StopSE(int id)
    {
        int index = ConvertIdIntoIndex(audioData.SE_Data, id);
        if (index < 0) return;
        var clip = audioData.SE_Data[index].clip;   // 停止するSEのクリップを取得
        
        foreach (var src in SESources)
        {
            if (src.isPlaying && src.clip == clip)  // クリップが一致するAudioSourceを探す
            {
                src.Stop();
            }
        }
    }
    public void StopAllSE()
    {
        foreach (var src in SESources)
        {
            src.Stop();
        }
    }

    public void SetSEVolume()
    {
        if (SESlider == null) return;
        float vol = SESlider.value;   // スライダーの値を取得

        // Sliderの値をSEのAudioSourceに反映
        foreach (var src in SESources)
        {
            src.volume = vol;
        }

        // PlayerPrefsに音量を保存
        PlayerPrefs.SetFloat("SEVolume", vol);
        PlayerPrefs.Save();
    }

    public void PlayBGM(int id)
    {
        int index = ConvertIdIntoIndex(audioData.BGM_Data, id);
        if (index < 0) return;

        BGMSource.clip = audioData.BGM_Data[index].clip;
        BGMSource.volume = audioData.BGM_Data[index].volume;
        BGMSource.Play();
    }

    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void SetBGMVolume()
    {   
        if (BGMSlider == null) return;
        float vol = BGMSlider.value;

        BGMSource.volume = vol;

        // PlayerPrefsに音量を保存
        PlayerPrefs.SetFloat("BGMVolume", vol);
        PlayerPrefs.Save();
    }
}
