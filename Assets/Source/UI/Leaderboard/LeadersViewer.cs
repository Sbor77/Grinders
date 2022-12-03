using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadersViewer : MonoBehaviour
{
    [SerializeField] private List<PlayerEntry> _playerEntries;

    private int _maxPlayerViews;

    public void SetLeaders(List<Leader> leaders)
    {
        int leaderCount = leaders.Count;

        if (leaderCount > _maxPlayerViews)
            leaderCount = _maxPlayerViews;

        for (int i = 0; i < leaderCount; i++)
            SetPlayerEntry(i, leaders[i]);
    }

    private void SetPlayerEntry(int leaderIndex, Leader currentLeader)
    {
        _playerEntries[leaderIndex].SetPlayer(currentLeader);
    }
}
