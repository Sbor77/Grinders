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

    private int _liderCount = 0;
    private List<Leader> _leaderList;

    private void OnEnable()
    {
        _okButton.onClick.AddListener(HideLeaderboard);
    }

    private void Start()
    {
        _liderCount = ShowLeaders();
        print("leaders count = " + _leaderList.Count);
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

    private int ShowLeaders()
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
                
                _leaderList.Add(new Leader(entry.rank, entry.score, name));
            }
        });

        _leaderViewers.SetLeaders(_leaderList);

        return _leaderList.Count;
    }
}
