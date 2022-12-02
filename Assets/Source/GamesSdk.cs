using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using System;

public class GamesSdk : MonoBehaviour
{
    private List<Leader> _leaders;
    private string _leaderboardName = "LeaderBoard";
    private int _playerScore = 100;
    private bool _isInitialize;

    public event Action Rewarded;
    public event Action AdVideoOpened;
    public event Action AdVideoClosed;
    public event Action InterstitialAdOpened;
    public event Action InterstitialAdClosed;

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
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        yield break;
#endif

#if YANDEX_GAMES
        yield return YandexGamesSdk.Initialize(OnInitialized);
#endif

        //#if VK_GAMES
        //        yield return Agava.VKGames.VKGamesSdk.Initialize(OnVKSDKInitialize);
        //#endif
    }

    #region language
    public string GetLanguage()
    {
        string lang = "default";

        if (_isInitialize)
            lang = YandexGamesSdk.Environment.i18n.lang;

        return lang;
    }
    #endregion

    #region Ad
    public void InterstitialAdShow()
    {
        if (!_isInitialize)
            return;

#if UNITY_EDITOR
        OnCloseCallback(false);
        return;
#endif

#if YANDEX_GAMES
        InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
#endif

        //#if VK_GAMES
        //        Interstitial.Show();
        //#endif
    }

    public void VideoAdShow()
    {
        if (!_isInitialize)
            return;

#if UNITY_EDITOR
        OnRewardedCallback();
        OnVideoCloseCallback();
        return;
#endif

#if YANDEX_GAMES
        Agava.YandexGames.VideoAd.Show(OnVideoOpenCallback, OnRewardedCallback, OnVideoCloseCallback, OnVideoErrorCallback);
#endif
        
#if VK_GAMES
        Agava.VKGames.VideoAd.Show(OnRewardedCallback);
#endif
    }
    #endregion

    #region LeaderBoard

    public List<Leader> GetYandexLeaderboard()
    {
        if (_isInitialize)
            return null;

        PlayerAccount.RequestPersonalProfileDataPermission();

        if (!PlayerAccount.IsAuthorized)
            PlayerAccount.Authorize();

        Leaderboard.GetEntries(_leaderboardName, (result) =>
        {
            int leadersNumber = result.entries.Length >= _leaders.Count ? _leaders.Count : result.entries.Length;

            for (int i = 0; i < leadersNumber; i++)
            {
                string name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonimus";

                _leaders.Add(new Leader(result.entries[i].rank, result.entries[i].score, name));
            }
        });

        return _leaders;
    }

    public void SetLeaderboardScore()
    {
        if (!_isInitialize)
            return;

        if (YandexGamesSdk.IsInitialized)
        {
            Leaderboard.GetPlayerEntry(_leaderboardName, OnPlayerEntrySuccessCallback);
        }
    }


    #endregion

    #region events
    private void OnInitialized()
    {
        _isInitialize = true;
    }

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

    private void OnPlayerEntrySuccessCallback(LeaderboardEntryResponse result)
    {
        if (result == null || _playerScore > result.score)
        {
            Leaderboard.SetScore(_leaderboardName, _playerScore);
        }
    }
    #endregion
}


public class Leader
{
    public readonly int Ranks;
    public readonly int Scores;
    public readonly string Names;

    public Leader(int ranks, int scores, string names)
    {
        Ranks = ranks;
        Scores = scores;
        Names = names;
    }
}