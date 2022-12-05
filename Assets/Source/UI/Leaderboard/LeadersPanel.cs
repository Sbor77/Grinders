using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;

public class LeadersPanel : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    //[SerializeField] private PlayerViewer _player;
    [SerializeField] private GameObject _content;
    [SerializeField] private PlayerEntry _template;

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

        Leaderboard.GetEntries("PlaytestBoard", (result) =>
        {
            Debug.Log($"My rank = {result.userRank}");

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
                Debug.Log(name + " " + entry.score);
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
