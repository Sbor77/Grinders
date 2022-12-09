using UnityEngine;
using Agava.WebUtility;
using UnityEngine.Audio;

public class GamePause : MonoBehaviour
{
    [SerializeField] private AudioMixer _audio;

    private const float MaxVolume = 0f;
    private const float MinVolume = -80f;

    private void Start()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;

        //if (GamesSdk.Instance != null)
        //{
        GamesSdk.Instance.AdVideoOpened += OnPlayAd;
        GamesSdk.Instance.AdVideoClosed += OnStopAd;
        GamesSdk.Instance.InterstitialAdOpened += OnPlayAd;
        GamesSdk.Instance.InterstitialAdClosed += OnStopAd;
        //}
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        //if (GamesSdk.Instance != null)
        //{
        GamesSdk.Instance.AdVideoOpened -= OnPlayAd;
        GamesSdk.Instance.AdVideoClosed -= OnStopAd;
        GamesSdk.Instance.InterstitialAdOpened -= OnPlayAd;
        GamesSdk.Instance.InterstitialAdClosed -= OnStopAd;
        //}
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        // Use both pause and volume muting methods at the same time.
        // They're both broken in Web, but work perfect together. Trust me on this.
        //AudioListener.pause = inBackground;
        //AudioListener.volume = inBackground ? 0f : 1f;

        /*Time.timeScale = inBackground ? 0f : 1f;
        float volume = inBackground ? MinVolume : MaxVolume;
        _audio.SetFloat(Master, volume);*/

        if (inBackground)
            Activate();
        else
            Resume();

        Debug.Log("Sound is paused in OnInBackgroundChange event");
    }

    private void OnPlayAd()
    {
        Activate();
    }

    private void OnStopAd()
    {
        Resume();
    }
    private void Activate()
    {
        Time.timeScale = 0;

        _audio.SetFloat(DataHandler.Instance.MasterVolume, MinVolume);
    }

    private void Resume()
    {
        Time.timeScale = 1;

        _audio.SetFloat(DataHandler.Instance.MasterVolume, DataHandler.Instance.GetSavedTotalVolume());
    }
}