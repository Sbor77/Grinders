using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private AudioMixer _audio;

    private const string Master = "MasterVolume";
    private const string Music = "MusicVolume";
    private const string Effects = "EffectsVolume";
    private const int multiplier = 20;

    private void OnEnable()
    {
        //_masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        //_musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        //_effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
    }

    private void OnDisable()
    {
        //_masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        //_musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        //_effectsVolumeSlider.onValueChanged.RemoveListener(OnEffectsVolumeChanged);
    }

    public void Start()
    {
        //_masterVolumeSlider.value = DataHandler.Instance.GetSavedMasterVolume();
        //_musicVolumeSlider.value = DataHandler.Instance.GetSavedMusicVolume();
        //_effectsVolumeSlider.value = DataHandler.Instance.GetSavedEffectsVolume();
        //OnMasterVolumeChanged(_masterVolumeSlider.value);
        //OnMusicVolumeChanged(_musicVolumeSlider.value);
        //OnEffectsVolumeChanged(_effectsVolumeSlider.value);
    }

    private void OnMasterVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        _audio.SetFloat(Master, volumeValue);
        DataHandler.Instance.SaveMasterVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        _audio.SetFloat(Music, volumeValue);
        DataHandler.Instance.SaveMusicVolume(value);
    }

    private void OnEffectsVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        _audio.SetFloat(Effects, volumeValue);
        DataHandler.Instance.SaveEffectsVolume(value);
    }
}
