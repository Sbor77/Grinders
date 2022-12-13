using UnityEngine;
using Agava.WebUtility;

public class GameFocus : MonoBehaviour
{
    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;

        GamesSdk.Instance.AdVideoOpened += OnPlayAd;
        GamesSdk.Instance.AdVideoClosed += OnStopAd;
        GamesSdk.Instance.InterstitialAdOpened += OnPlayAd;
        GamesSdk.Instance.InterstitialAdClosed += OnStopAd;
#endif
    }

    private void OnDisable()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;

        GamesSdk.Instance.AdVideoOpened -= OnPlayAd;
        GamesSdk.Instance.AdVideoClosed -= OnStopAd;
        GamesSdk.Instance.InterstitialAdOpened -= OnPlayAd;
        GamesSdk.Instance.InterstitialAdClosed -= OnStopAd;
#endif
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        if (inBackground)
            Pause();
        else
            Resume();
    }

    private void OnPlayAd()
    {
        Pause();
    }

    private void OnStopAd()
    {
        Resume();
    }

    private void Pause()
    {
        Time.timeScale = 0;
        AudioListener.volume = 0;        
    }

    private void Resume()
    {
        Time.timeScale = 1;

        int muteValue = DataHandler.Instance.GetSavedMuteValue();        
        AudioListener.volume = muteValue == 0 ? 0 : DataHandler.Instance.GetSavedTotalVolume();        
    }
}