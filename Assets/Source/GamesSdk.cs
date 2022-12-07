using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using System;
using Lean.Localization;

public class GamesSdk : MonoBehaviour
{
    private string _leaderboardName = "LeaderBoard";

    public string LeaderboardName => _leaderboardName;

    public event Action Rewarded;
    public event Action AdVideoOpened;
    public event Action AdVideoClosed;
    public event Action InterstitialAdOpened;
    public event Action InterstitialAdClosed;
    public event Action<bool> ChangedLeaders;

    private const float WaitTime = .25f;

    public static GamesSdk Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif

        // Always wait for it if invoking something immediately in the first scene.
        yield return YandexGamesSdk.Initialize();

        if (YandexGamesSdk.IsInitialized)
            LoadLocalization();

        while (!YandexGamesSdk.IsInitialized)
        {
            yield return new WaitForSeconds(WaitTime);

            if (YandexGamesSdk.IsInitialized)
                LoadLocalization();
        }
    }

    #region language
    public string GetLanguage()
    {
        string lang = "ru";

        if (YandexGamesSdk.IsInitialized)
            lang = YandexGamesSdk.Environment.i18n.lang;

        return lang;
    }

    private void LoadLocalization()
    {
        string lang;
        switch (GetLanguage())
        {
            case "ru":
                lang = "Russian";
                break;
            case "tr":
                lang = "Turkish";
                break;
            case "en":
                lang = "English";
                break;
            default:
                lang = "Russian";
                break;
        }

        LeanLocalization.SetCurrentLanguageAll(lang);
        DataHandler.Instance.SaveLanguage(lang);
    }

    #endregion

    #region Ad
    public void InterstitialAdShow()
    {

#if UNITY_EDITOR
        OnCloseCallback(false);
        return;
#endif

#if UNITY_WEBGL || !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized)
            InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
#endif
    }

    public void VideoAdShow()
    {

#if UNITY_EDITOR
        OnRewardedCallback();
        OnVideoCloseCallback();
        return;
#endif

#if UNITY_WEBGL || !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized)
            VideoAd.Show(OnVideoOpenCallback, OnRewardedCallback, OnVideoCloseCallback, OnVideoErrorCallback);
#endif
    }
    #endregion

    #region LeaderBoard

    public void SetLeaderboardScore(int playerScore)
    {
        if (!YandexGamesSdk.IsInitialized)
            return;

        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetPlayerEntry(_leaderboardName, (result) =>
            {
                if (result == null || playerScore > result.score)
                    Leaderboard.SetScore(_leaderboardName, playerScore);
            });
        }
    }

    //public Leader GetLeaderboardPlayerEntry()
    //{
    //    Leader player = null;

    //    if (!PlayerAccount.IsAuthorized)
    //        return player;

    //    Leaderboard.GetPlayerEntry("PlaytestBoard", (result) =>
    //    {
    //        if (result == null)
    //            Debug.Log("Player is not present in the leaderboard.");
    //        else
    //            player = new Leader(result.rank, result.score, "");
    //    });

    //    return player;
    //}

    #endregion

    #region events
    private void OnVideoOpenCallback()
    {
        AdVideoOpened?.Invoke();
    }

    private void OnVideoCloseCallback()
    {
        AdVideoClosed?.Invoke();
    }

    private void OnOpenCallback()
    {
        InterstitialAdOpened?.Invoke();
    }

    private void OnCloseCallback(bool wasShown)
    {
        InterstitialAdClosed?.Invoke();
    }

    private void OnRewardedCallback()
    {
        Rewarded?.Invoke();
    }

    private void OnVideoErrorCallback(string message)
    {
        Debug.LogError(message);
    }

    //private void OnPlayerEntrySuccessCallback(LeaderboardEntryResponse result)
    //{
    //    if (result == null || _playerScore > result.score)
    //    {
    //        Leaderboard.SetScore(_leaderboardName, _playerScore);
    //    }
    //}
    #endregion
}

//public class Leader
//{
//    public readonly int Ranks;
//    public readonly int Scores;
//    public readonly string Names;

//    public Leader(int ranks, int scores, string names)
//    {
//        Ranks = ranks;
//        Scores = scores;
//        Names = names;
//    }
//}