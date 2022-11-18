using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SkillBuyer _buyer;
    [SerializeField] private StatsViewer _viewer;
    [SerializeField] private Button _exitButton;

    private StatsInfo _statsInfo;

    private const string Level = "Level";
    private const string Money = "Money";
    private const string Kills = "Kills";
    private const string Health = "Health";
    private const string MoveSpeed = "Speed";

    private void Start()
    {
        OnStatBought();
    }

    private void OnEnable()
    {
        _buyer.IsStatBought += OnStatBought;        
    }

    private void OnDisable()
    {
        _buyer.IsStatBought -= OnStatBought;
    }

    private void CloseShop()
    {
        SceneManager.LoadScene(_statsInfo.Level);
    }

    private void OnStatBought()
    {
        _statsInfo = new(GetSavedValue(Level), GetSavedValue(Money), GetSavedValue(Kills),
                     GetSavedValue(Health), GetSavedValue(MoveSpeed));

        _viewer.Init(_statsInfo);

        _buyer.Init(_statsInfo);
    }

    private int GetSavedValue(string name)
    {
        if (PlayerPrefs.HasKey(name))
            return PlayerPrefs.GetInt(name);

        return 0;
    }
}

public class StatsInfo
{
    public int Level { get; private set; }
    public int Money { get; private set; }
    public int Kills { get; private set; }
    public int Health { get; private set; }
    public int MoveSpeed { get; private set; }

    public string LevelString => "Level";
    public string MoneyString => "Money";
    public string KillsString => "Kills";
    public string HealthString => "Health";
    public string MoveSpeedString => "MoveSpeed";

    public StatsInfo(int level, int money, int kills, int health, int moveSpeed)
    {
        Level = level;
        Money = money;
        Kills = kills;
        Health = health;
        MoveSpeed = moveSpeed;
    }

    public StatsInfo() 
    {
        InitStats();
    }

    public void SaveStat(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);

        PlayerPrefs.Save();
    }

    private void InitStats()
    {
        int defaultValue = -1;

        PlayerPrefs.SetInt(LevelString, defaultValue);

        PlayerPrefs.SetInt(MoneyString, defaultValue);

        PlayerPrefs.SetInt(KillsString, defaultValue);

        PlayerPrefs.SetInt(HealthString, defaultValue);

        PlayerPrefs.SetInt(MoveSpeedString, defaultValue);

        PlayerPrefs.Save();
    }



    public bool TryGetCurrentStats()
    {
        bool result = true;

        if (PlayerPrefs.HasKey(LevelString))
            Level = PlayerPrefs.GetInt(LevelString);
        else
            result = false;

        if (PlayerPrefs.HasKey(MoneyString))
            Money = PlayerPrefs.GetInt(MoneyString);
        else
            result = false;

        if (PlayerPrefs.HasKey(KillsString))
            Kills = PlayerPrefs.GetInt(KillsString);
        else
            result = false;

        if (PlayerPrefs.HasKey(HealthString))
            Health = PlayerPrefs.GetInt(HealthString);
        else
            result = false;

        if (PlayerPrefs.HasKey(MoveSpeedString))
            MoveSpeed = PlayerPrefs.GetInt(MoveSpeedString);
        else
            result = false;

        return result;
    }    
}