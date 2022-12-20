using UnityEngine;
using UnityEngine.UI;

public class VolumePanel : MonoBehaviour
{
    [SerializeField] private Toggle _muteStateToggle;
    [SerializeField] private Slider _totalVolumeSlider;    
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private BackgroundMusic _backgroundMusic;
        
    private const int MaxVolume = 1;
    private const int MinVolume = 0;

    private void OnEnable()
    {
        _muteStateToggle.onValueChanged.AddListener(OnMuteStateChanged);
        _totalVolumeSlider.onValueChanged.AddListener(OnTotalVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void OnDisable()
    {
        _muteStateToggle.onValueChanged.RemoveListener(OnMuteStateChanged);
        _totalVolumeSlider.onValueChanged.RemoveListener(OnTotalVolumeChanged);
        _musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
    }

    public void Start()
    {
        _muteStateToggle.isOn = DataHandler.Instance.GetSavedMuteValue() == MaxVolume;
        _totalVolumeSlider.value = DataHandler.Instance.GetSavedTotalVolume();
        _musicVolumeSlider.value = DataHandler.Instance.GetSavedMusicVolume();

        OnTotalVolumeChanged(DataHandler.Instance.GetSavedTotalVolume());
        OnMusicVolumeChanged(DataHandler.Instance.GetSavedMusicVolume());

        SetTotalVolume();
    }

    private void OnMuteStateChanged(bool unmute)
    {        
        int muteVolume = unmute ? MaxVolume : MinVolume;        

        _musicVolumeSlider.interactable = unmute;
        _totalVolumeSlider.interactable = unmute;
        DataHandler.Instance.SaveMuteValue(muteVolume);

        SetTotalVolume();
    }

    private void OnMusicVolumeChanged(float value)
    {
        DataHandler.Instance.SaveMusicVolume(value);
        _backgroundMusic.SetVolume(value);
    }

    private void SetTotalVolume()
    {
        AudioListener.volume = _muteStateToggle.isOn ? DataHandler.Instance.GetSavedTotalVolume() : MinVolume;
    }

    private void OnTotalVolumeChanged(float value)
    {
        DataHandler.Instance.SaveTotalVolume(value);
        AudioListener.volume = value;
    }
}