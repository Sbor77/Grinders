using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;
using TMPro;

public class LeadersPanel : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private GameObject _content;
    [SerializeField] private PlayerEntry _template;
    [SerializeField] private TextMeshProUGUI _playerScore;
    [SerializeField] private TextMeshProUGUI _playerPlace;

    private const int MaxViews = 5;

    private void OnEnable()
    {
        ShowLeaders();
        _okButton.onClick.AddListener(HideLeaderboard);
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(HideLeaderboard);
        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowLeaders()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (!PlayerAccount.IsAuthorized)
        {
            PlayerAccount.Authorize();
        }

        Leaderboard.GetEntries(GamesSdk.Instance.LeaderboardName, (result) =>
        {
            foreach (var entry in result.entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                int score = entry.score;
                int place = entry.rank;

                if (place > MaxViews)
                    break;

                AddLeader(name, score, place);
            }
        });

        Leaderboard.GetPlayerEntry(GamesSdk.Instance.LeaderboardName, (result) =>
        {
            if (result == null)
            {
                Debug.Log("Player is not present in the leaderboard.");
                _playerPlace.text = "0";
                _playerScore.text = "0";
            }
            else
            {
                _playerPlace.text = result.rank.ToString();
                _playerScore.text = result.score.ToString();
            }
        });

    }

    private void HideLeaderboard()
    {
        gameObject.SetActive(false);
    }

    private void AddLeader(string name, int score, int place)
    {
        var view = Instantiate(_template, _content.transform);
        view.Render(name, score.ToString(), place.ToString());
    }
}
