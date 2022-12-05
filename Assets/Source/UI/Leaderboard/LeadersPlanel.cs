using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;

public class LeadersPlanel : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private LeadersViewer _leaderViewers;
    [SerializeField] private PlayerViewer _player;
    [SerializeField] private List<PlayerEntry> _playerEntries;

    private int _maxPlayerViews;
    private int _currentPlayerIndex = 0;
    private List<Leader> _leaderList;

    private void OnEnable()
    {
        _okButton.onClick.AddListener(HideLeaderboard);
    }

    private void Start()
    {
        ShowLeaders();
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(HideLeaderboard);
    }

    public void ShowLeaderboard()
    { 
        gameObject.SetActive(true);
    }

    private void HideLeaderboard()
    {
        gameObject.SetActive(false);
    }

    private void ShowLeaders()
    {
         _leaderList = new List<Leader>();

        if (!PlayerAccount.IsAuthorized)
        {
            PlayerAccount.Authorize();
        }

        Leaderboard.GetEntries("LeaderBoard", (result) =>
        {
            foreach (var entry in result.entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";
                
                //_leaderList.Add(new Leader(entry.rank, entry.score, name));
                print(entry.rank);
                SetLeaders(new Leader(entry.rank, entry.score, name));
            }
        });

        //_leaderViewers.SetLeaders(_leaderList);
    }


    public void SetLeaders(Leader leaders)
    {
        if (_currentPlayerIndex < _maxPlayerViews)
        {
            _currentPlayerIndex++;
            SetPlayerEntry(_currentPlayerIndex, leaders);
        }
    }

    private void SetPlayerEntry(int leaderIndex, Leader currentLeader)
    {
        _playerEntries[leaderIndex].SetPlayer(currentLeader);
    }

}
