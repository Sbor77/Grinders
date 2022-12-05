using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agava.YandexGames;

public class LeadersPlanel : MonoBehaviour
{
    [SerializeField] private Button _okButton;
    [SerializeField] private LeadersViewer _leaders;
    [SerializeField] private PlayerViewer _player;

    private int _liderCount = 0;

    private void OnEnable()
    {
        _okButton.onClick.AddListener(HideLeaderboard);
        _liderCount = ShowLeaders();
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

    public int ShowLeaders()
    {
        List<Leader> leaders = new List<Leader>();

        if (!PlayerAccount.IsAuthorized)
        {
            PlayerAccount.Authorize();
        }

        Leaderboard.GetEntries("LeaderBoard", (result) =>
        {
            print($"Leader counts = {result.entries.Length}");
            
            foreach (var entry in result.entries)
            {
                string name = entry.player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";
                
                int score = entry.score;
                int place = entry.rank;

                leaders.Add(new Leader(entry.rank, entry.score, name));
                print(name + " " + entry.score);
            }
        });

        print("leaders count = " + leaders.Count);
        _leaders.SetLeaders(leaders);

        return leaders.Count;
    }
}
