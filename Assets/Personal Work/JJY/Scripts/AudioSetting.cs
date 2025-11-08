using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private TextMeshProUGUI bgmValueText;
    [SerializeField] private TextMeshProUGUI sfxValueText;

    private const float MAX_SLIDER_VALUE = 5f;
    private const string PREF_MASTER = "Audio_Master";
    private const string PREF_BGM = "Audio_BGM";
    private const string PREF_SFX = "Audio_SFX";

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(PREF_MASTER, MAX_SLIDER_VALUE);
        bgmSlider.value = PlayerPrefs.GetFloat(PREF_BGM, MAX_SLIDER_VALUE);
        sfxSlider.value = PlayerPrefs.GetFloat(PREF_SFX, MAX_SLIDER_VALUE);

        UpdateTexts();
        OnMasterVolumeChanged(masterSlider.value);
        OnBGMVolumeChanged(bgmSlider.value);
        OnSFXVolumeChanged(sfxSlider.value);

        masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private float SliderValueToLinearVolume(float sliderValue)
    {
        return Mathf.Clamp01(sliderValue / MAX_SLIDER_VALUE);
    }

    private void OnMasterVolumeChanged(float value)
    {
        float linearVolume = SliderValueToLinearVolume(value);
        PlayerPrefs.SetFloat(PREF_MASTER, value);
        AudioManager.Instance?.SetMasterVolume(linearVolume);
        UpdateTexts();
    }

    private void OnBGMVolumeChanged(float value)
    {
        float linearVolume = SliderValueToLinearVolume(value);
        PlayerPrefs.SetFloat(PREF_BGM, value);
        AudioManager.Instance?.SetBGMVolume(linearVolume);
        UpdateTexts();
    }

    private void OnSFXVolumeChanged(float value)
    {
        float linearVolume = SliderValueToLinearVolume(value);
        PlayerPrefs.SetFloat(PREF_SFX, value);
        AudioManager.Instance?.SetSFXVolume(linearVolume);
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        if (masterValueText != null)
            masterValueText.text = $"{(masterSlider.value / MAX_SLIDER_VALUE * 100f):F0}%";


        if (bgmValueText != null)
            bgmValueText.text = $"{(bgmSlider.value / MAX_SLIDER_VALUE * 100f):F0}%";


        if (sfxValueText != null)
            sfxValueText.text = $"{(sfxSlider.value / MAX_SLIDER_VALUE * 100f):F0}%";
    }
}
