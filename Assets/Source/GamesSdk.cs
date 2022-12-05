using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using System;
using Lean.Localization;

public class GamesSdk : MonoBehaviour
{
    [SerializeField] private LeanLocalization _leanLocalization;

    private List<Leader> _leaders;
    private string _leaderboardName = "LeaderBoard";
    private int _playerScore = 0;
    private bool _isInitialize;

    public event Action Rewarded;
    public event Action AdVideoOpened;
    public event Action AdVideoClosed;
    public event Action InterstitialAdOpened;
    public event Action InterstitialAdClosed;

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
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        yield break;
#endif

#if UNITY_WEBGL
        yield return YandexGamesSdk.Initialize();
#endif

        //#if VK_GAMES
        //        yield return Agava.VKGames.VKGamesSdk.Initialize(OnVKSDKInitialize);
        //#endif
        while (!YandexGamesSdk.IsInitialized)
        {
            yield return new WaitForSeconds(WaitTime);

            if (YandexGamesSdk.IsInitialized)
            {
                LoadLocalization();
                Debug.Log("Yandex SDK initialized");
            }
        }
    }

    #region language
    public string GetLanguage()
    {
        string lang = "ru";

        if (_isInitialize)
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

        _leanLocalization.SetCurrentLanguage(lang);
        DataHandler.Instance.SaveLanguage(lang);
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

    public void SetLeaderboardScore(int playerScore)
    {
        if (!_isInitialize)
            return;

        _playerScore = playerScore;

        if (PlayerAccount.IsAuthorized)
        {
            Leaderboard.GetPlayerEntry(_leaderboardName, OnPlayerEntrySuccessCallback);
        }
    }

    public Leader GetLeaderboardPlayerEntry()
    {
        Leader player = null;

        if (!PlayerAccount.IsAuthorized)
            return player;

        Leaderboard.GetPlayerEntry("PlaytestBoard", (result) =>
        {
            if (result == null)
                Debug.Log("Player is not present in the leaderboard.");
            else
                player = new Leader(result.rank, result.score, "");
        });

        return player;
    }

    #endregion

    #region events
    private void OnInitialized()
    {
        _isInitialize = true;
        LoadLocalization();
        Debug.Log("Initialization succeeded");
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