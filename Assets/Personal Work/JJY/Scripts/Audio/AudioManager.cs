using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [Header("Mixer")]
    public AudioMixer mixer;

    [Header("Mixer Groups")]
    public AudioMixerGroup masterGroup;
    public AudioMixerGroup bgmGroup;
    public AudioMixerGroup sfxGroup;

    [Header("AudioSources")]
    public AudioSource titleBGM;
    public int sfxPoolSize = 10;

    private const string MASTER_PARAM = "MasterVolume";
    private const string BGM_PARAM = "BGMVolume";
    private const string SFX_PARAM = "SFXVolume";
    private const string PREF_MASTER = "Audio_Master";
    private const string PREF_BGM = "Audio_BGM";
    private const string PREF_SFX = "Audio_SFX";
    private const float MAX_SLIDER_VALUE = 5f;

    private AudioSource bgmSource;
    private List<AudioSource> sfxPool = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureBgmSource();
        CreateSfxPool();

        float masterSliderValue = PlayerPrefs.GetFloat(PREF_MASTER, MAX_SLIDER_VALUE);
        float bgmSliderValue = PlayerPrefs.GetFloat(PREF_BGM, MAX_SLIDER_VALUE);
        float sfxSliderValue = PlayerPrefs.GetFloat(PREF_SFX, MAX_SLIDER_VALUE);
        float masterLinear = masterSliderValue / MAX_SLIDER_VALUE;
        float bgmLinear = bgmSliderValue / MAX_SLIDER_VALUE;
        float sfxLinear = sfxSliderValue / MAX_SLIDER_VALUE;

        ApplyVolume(MASTER_PARAM, masterLinear);
        ApplyVolume(BGM_PARAM, bgmLinear);
        ApplyVolume(SFX_PARAM, sfxLinear);
    }

    #region Setup
    private void EnsureBgmSource()
    {
        if (bgmSource == null)
        {
            if (titleBGM != null)
            {
                bgmSource = Instantiate(titleBGM, transform);
            }
            else
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.playOnAwake = false;
                bgmSource.loop = true;
            }
        }
        if (bgmGroup != null) bgmSource.outputAudioMixerGroup = bgmGroup;
    }

    private void CreateSfxPool()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            var go = new GameObject("SFX_Source_" + i);
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            if (sfxGroup != null) src.outputAudioMixerGroup = sfxGroup;
            sfxPool.Add(src);
        }
    }
    #endregion

    #region Volume Helpers
    public void SetMasterVolume(float value)
    {
        ApplyVolume(MASTER_PARAM, value);
    }

    public void SetBGMVolume(float value)
    {
        ApplyVolume(BGM_PARAM, value);
    }

    public void SetSFXVolume(float value)
    {
        ApplyVolume(SFX_PARAM, value);
    }

    private void ApplyVolume(string param, float linear0to1)
    {
        if (mixer == null) return;

        float clamped = Mathf.Clamp01(linear0to1);
        float dB;

        if (clamped <= 0f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(clamped) * 20f;
        }

        mixer.SetFloat(param, dB);
    }

    public float GetVolume(string param)
    {
        if (mixer == null) return 1f;

        if (mixer.GetFloat(param, out float dB))
        {
            float linear = Mathf.Pow(10f, dB / 20f);
            return Mathf.Clamp01(linear);
        }

        return 1f;
    }
    #endregion

    #region BGM Control
    public void PlayBGM(AudioClip clip, bool loop = true, float fadeTime = 0f)
    {
        if (clip == null) return;
        EnsureBgmSource();
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        StopAllCoroutines();
        bgmSource.loop = loop;
        bgmSource.clip = clip;
        bgmSource.Play();

        if (fadeTime > 0f)
            StartCoroutine(FadeVolumeCoroutine(bgmSource, 0f, 1f, fadeTime));
    }

    public void StopBGM(float fadeTime = 0f)
    {
        if (bgmSource == null) return;
        if (fadeTime > 0f)
            StartCoroutine(FadeOutAndStop(bgmSource, fadeTime));
        else
            bgmSource.Stop();
    }
    #endregion

    #region SFX Playback
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        var src = GetAvailableSfxSource();
        if (src == null) return;
        src.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    private AudioSource GetAvailableSfxSource()
    {
        foreach (var s in sfxPool)
        {
            if (!s.isPlaying) return s;
        }

        var go = new GameObject("SFX_Source_extra");
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        if (sfxGroup != null) src.outputAudioMixerGroup = sfxGroup;
        sfxPool.Add(src);
        return src;
    }
    #endregion

    #region Coroutines
    private IEnumerator FadeVolumeCoroutine(AudioSource source, float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;
            source.volume = Mathf.Lerp(from, to, t);
            yield return null;
        }
        source.volume = to;
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration)
    {
        float start = source.volume;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(start, 0f, timer / duration);
            yield return null;
        }
        source.Stop();
        source.volume = start;
    }
    #endregion

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
