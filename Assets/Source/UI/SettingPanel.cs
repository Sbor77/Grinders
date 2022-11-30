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
    [SerializeField] private Button _closeButton;
    [SerializeField] private AudioMixer _audio;

    private const string Volume = "MasterVolume";
    private const int multiplier = 20;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseClick);
        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseClick);
        _masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _effectsVolumeSlider.onValueChanged.RemoveListener(OnEffectsVolumeChanged);
    }

    public void Init()
    {
        _masterVolumeSlider.value = DataHandler.Instance.GetSavedMasterVolume();
        OnMasterVolumeChanged(_masterVolumeSlider.value);
    }

    private void OnCloseClick()
    {
        DataHandler.Instance.SaveAllStats();
        gameObject.SetActive(false);
    }

    private void OnMasterVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        _audio.SetFloat(Volume, volumeValue);
        DataHandler.Instance.SaveMasterVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        //_audio.SetFloat(Volume, volumeValue);
        DataHandler.Instance.SaveMusicVolume(value);
    }

    private void OnEffectsVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        //_audio.SetFloat(Volume, volumeValue);
        DataHandler.Instance.SaveEffectsVolume(value);
    }
}
