using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SkillBuying _buying;
    [SerializeField] private StatsViewer _viewer;
    [SerializeField] private Button _exitButton;

    private StatsInfo _statsInfo;

    private const string Stage = "Stage";
    private const string Golds = "Golds";
    private const string Kills = "Kills";
    private const string Health = "Health";
    private const string MoveSpeed = "Speed";

    private void Start()
    {
        _buying.ChangedStatsInfo += UpdateStateInfo;
        UpdateStateInfo();
    }

    private void CloseShop()
    {
        SceneManager.LoadScene(_statsInfo.Stage);
    }

    private void UpdateStateInfo()
    {
        _statsInfo = new(GetLoadValue(Stage), GetLoadValue(Golds), GetLoadValue(Kills),
                     GetLoadValue(Health), GetLoadValue(MoveSpeed));
        _viewer.Init(_statsInfo);
        _buying.Init(_statsInfo);
    }

    private int GetLoadValue(string name)
    {
        if (PlayerPrefs.HasKey(name))
            return PlayerPrefs.GetInt(name);

        return 0;
    }
}

public class StatsInfo
{
    public int Stage { get; private set; }
    public int Golds { get; private set; }
    public int Kills { get; private set; }
    public int Health { get; private set; }
    public int MoveSpeed { get; private set; }

    public StatsInfo(int stage, int golds, int kills, int health, int moveSpeed)
    {
        Stage = stage;
        Golds = golds;
        Kills = kills;
        Health = health;
        MoveSpeed = moveSpeed;
    }
}
