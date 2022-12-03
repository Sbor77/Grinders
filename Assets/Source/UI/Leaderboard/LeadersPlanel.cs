using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeadersPlanel : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private LeadersViewer _leaders;
    [SerializeField] private PlayerViewer _player;

    private void OnEnable()
    {
        _okButton.onClick.AddListener(HideLeaderboard);
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(HideLeaderboard);
    }

    public void ShowLeaderboard()
    {
        ViewLeaders();
        gameObject.SetActive(true);
    }

    private void ViewLeaders()
    {
#if YANDEX_GAMES
        List<Leader> leaders = GamesSdk.Instance.GetYandexLeaderboard();
        Leader player = GamesSdk.Instance.GetLeaderboardPlayerEntry();

        _leaders.SetLeaders(leaders);
        _player.SetPlayerStats(player.Ranks, player.Scores);
#endif

#if UNITY_EDITOR
        print("Будет работать только на YandexGames");
        _player.SetPlayerStats(1, DataHandler.Instance.GetSavedTotalScore());
#endif
    }

    private void HideLeaderboard()
    {
        gameObject.SetActive(false);
    }
}
