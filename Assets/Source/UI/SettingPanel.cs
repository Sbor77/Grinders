//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Toggle _masterVolumeToggle;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private AudioSource _backgroundMusicSource;

    //private const string Master = "MasterVolume";
    //private const string Music = "MusicVolume";
    //private const string Effects = "EffectsVolume";
    //private const int Multiplier = 20;
    //private const float MusicVolumeOffset = 20f;
    //private const float MaxVolume = 1;
    //private const float MinVolume = -80;

    private void OnEnable()
    {
        _masterVolumeToggle.onValueChanged.AddListener(OnMasterToggleChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    private void OnDisable()
    {
        _masterVolumeToggle.onValueChanged.RemoveListener(OnMasterToggleChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        DataHandler.Instance.SaveAllStats();
    }

    public void Start()
    {
        _masterVolumeToggle.isOn = DataHandler.Instance.GetSavedMuteVolume();
        _musicVolumeSlider.value = DataHandler.Instance.GetSavedMusicVolume();
        _masterVolumeSlider.value = DataHandler.Instance.GetSavedMasterVolume();

        //OnMasterVolumeChanged(true);
        //OnMusicVolumeChanged(_musicVolumeSlider.value);
        //OnEffectsVolumeChanged(_effectsVolumeSlider.value);

        //_musicVolumeSlider.value = 0.5f;
        //_effectsVolumeSlider.value = 0.5f;
    }

    private void OnMasterToggleChanged(bool value)
    {
        float volume;

        if (value)
        {
            volume = DataHandler.Instance.GetSavedMasterVolume();
            print(volume);
        }
        else
        {
            //DataHandler.Instance.SaveMasterVolume(volume);
            volume = 0;
            print(volume);
        }

        AudioListener.volume = volume;
        DataHandler.Instance.SaveMuteVolume(value);
        //{
        //    //volume = MaxVolume;

        //    ActivateSlider(_musicVolumeSlider);

        //    ActivateSlider(_effectsVolumeSlider);
        //}
        //else
        //{
        //    DeactivateSlider(_musicVolumeSlider);

        //    DeactivateSlider(_effectsVolumeSlider);
        //}

        //_audio.SetFloat(Master, volume);

        //DataHandler.Instance.SaveMasterVolume(value == true ? 1 : 0);
    }

    private void OnMusicVolumeChanged(float value)
    {
        _backgroundMusicSource.volume = value;
        //float volumeValue = Mathf.Log10(value) * Multiplier - MusicVolumeOffset;
        //_audio.SetFloat(Music, volumeValue);
        DataHandler.Instance.SaveMusicVolume(value);
    }

    private void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        //    float volumeValue = Mathf.Log10(value) * Multiplier;
        //    _audio.SetFloat(Effects, volumeValue);
        DataHandler.Instance.SaveMasterVolume(value);
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