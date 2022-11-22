using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Button _closeButton;
    [SerializeField] private AudioMixer _audio;

    private const string Volume = "MasterVolume";
    private const int multiplier = 20;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnCloseClick);
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnCloseClick);
        _volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
    }

    public void Init()
    {
        _volumeSlider.value = DataHandler.Instance.GetSavedVolume();
        OnVolumeChanged(_volumeSlider.value);
    }

    private void OnCloseClick()
    {
        DataHandler.Instance.SaveAllStats();
        gameObject.SetActive(false);
    }

    private void OnVolumeChanged(float value)
    {
        float volumeValue = Mathf.Log10(value) * multiplier;
        _audio.SetFloat(Volume, volumeValue);
        DataHandler.Instance.SaveVolume(value);
    }
}
