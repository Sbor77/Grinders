using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; }

    public string LevelString => "Level";
    public string MoneyString => "Money";
    public string KillsString => "Kills";
    public string HealthString => "Health";
    public string MoveSpeedString => "MoveSpeed";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(this);
        }        
    }

    public void SaveLevel (int level)
    {
        PlayerPrefs.SetInt(LevelString, level);
    }

    public void SaveMoney (int money)
    {
        PlayerPrefs.SetInt(MoneyString, money);
    }

    public void SaveKills(int kills)
    {
        PlayerPrefs.SetInt(KillsString, kills);
    }

    public void SaveHealth(int health)
    {
        PlayerPrefs.SetInt(HealthString, health);
    }

    public void SaveMoveSpeed(int moveSpeed)
    {
        PlayerPrefs.SetInt(MoveSpeedString, moveSpeed);
    }

    public void SaveAllStats()
    {
        PlayerPrefs.Save();
    }

    public int GetSavedMoney()
    {
        return PlayerPrefs.GetInt(MoneyString);
    }

    public int GetSavedKills()
    {
        return PlayerPrefs.GetInt(KillsString);
    }

    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt(LevelString);
    }

    public int GetSavedHealth()
    {
        return PlayerPrefs.GetInt(HealthString);
    }

    public int GetSavedMoveSpeed()
    {
        return PlayerPrefs.GetInt(MoveSpeedString);
    }
}