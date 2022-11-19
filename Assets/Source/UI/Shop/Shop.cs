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

    //private StatsInfo _statsInfo;

    private const string Level = "SceneLevel";
    //private const string Money = "Money";
    //private const string Kills = "Kills";
    //private const string Health = "Health";
    //private const string MoveSpeed = "Speed";

    private void Start()
    {
        OnStatBought();
    }

    private void OnEnable()
    {
        _buyer.IsStatBought += OnStatBought;
        _exitButton.onClick.AddListener(CloseShop);
    }

    private void OnDisable()
    {
        _buyer.IsStatBought -= OnStatBought;
        _exitButton.onClick.RemoveListener(CloseShop);
    }

    private void CloseShop()
    {
        SceneManager.LoadScene(Level + (DataHandler.Instance.Level + 1));
    }

    private void OnStatBought()
    {
        _viewer.Init();
        _buyer.Init();
    }

    //private int GetSavedValue(string name)
    //{
    //    if (PlayerPrefs.HasKey(name))
    //        return PlayerPrefs.GetInt(name);

    //    return 0;
    //}
}

/*public class StatsData : MonoBehaviour
{
    public int Level { get; private set; }
    public int Money { get; private set; }
    public int Kills { get; private set; }
    public int Health { get; private set; }
    public int MoveSpeed { get; private set; }

    public StatsData()
    {
        Level = DataHandler.Instance.GetSavedStat(DataHandler.Instance.LevelString);
        Money = DataHandler.Instance.GetSavedStat(DataHandler.Instance.MoneyString);
        Kills = DataHandler.Instance.GetSavedStat(DataHandler.Instance.KillsString);
        Health = DataHandler.Instance.GetSavedStat(DataHandler.Instance.HealthString);
        MoveSpeed = DataHandler.Instance.GetSavedStat(DataHandler.Instance.MoveSpeedString);
    }
}

    /*public StatsInfo(int level, int money, int kills, int health, int moveSpeed)
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
    }*//*

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
}*/