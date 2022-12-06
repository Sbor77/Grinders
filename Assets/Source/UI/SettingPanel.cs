using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Toggle _masterVolumeToggle;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _effectsVolumeSlider;
    [SerializeField] private AudioMixer _audio;

    private const string Master = "MasterVolume";
    private const string Music = "MusicVolume";
    private const string Effects = "EffectsVolume";
    private const int Multiplier = 20;
    private const float MusicVolumeOffset = 20f;
    private const float MaxVolume = 0;
    private const float MinVolume = -80;

    private void OnEnable()
    {
        _masterVolumeToggle.onValueChanged.AddListener(OnMasterVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
    }

    private void OnDisable()
    {
        _masterVolumeToggle.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _effectsVolumeSlider.onValueChanged.RemoveListener(OnEffectsVolumeChanged);
    }

    public void Start()
    {
        //_masterVolumeToggle.isOn = DataHandler.Instance.GetSavedMasterVolume() == MaxVolume ? true : false;
        _musicVolumeSlider.value = DataHandler.Instance.GetSavedMusicVolume();
        _effectsVolumeSlider.value = DataHandler.Instance.GetSavedEffectsVolume();
        
        OnMasterVolumeChanged(true);
        OnMusicVolumeChanged(_musicVolumeSlider.value);
        OnEffectsVolumeChanged(_effectsVolumeSlider.value);

        _musicVolumeSlider.value = 0.5f;
        _effectsVolumeSlider.value = 0.5f;
    }

    private void OnMasterVolumeChanged(bool value)
    {        
        float volume = MinVolume;

        if (value)
        {
            volume = MaxVolume;

            ActivateSlider(_musicVolumeSlider);

            ActivateSlider(_effectsVolumeSlider);
        }
        else
        {
            DeactivateSlider(_musicVolumeSlider);

            DeactivateSlider(_effectsVolumeSlider);
        }

        _audio.SetFloat(Master, volume);        

        DataHandler.Instance.SaveMasterVolume(volume);
    }

    private void OnMusicVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * Multiplier - MusicVolumeOffset;

        _audio.SetFloat(Music, volumeValue);

        DataHandler.Instance.SaveMusicVolume(volumeValue);
    }

    private void OnEffectsVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * Multiplier;

        _audio.SetFloat(Effects, volumeValue);

        DataHandler.Instance.SaveEffectsVolume(value);
    }

    private void DeactivateSlider(Slider slider)
    {
        slider.interactable = false;        
    }

    private void ActivateSlider(Slider slider)
    {
        slider.interactable = true;
    }
}
