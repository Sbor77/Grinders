using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private string _levelKey = "Level";
    private string _moneyKey = "Money";
    private string _killsKey = "Kills";
    private string _healthSkillKey = "HealthSkill";
    private string _speedSkillKey = "SpeedSkill";

    public static DataHandler Instance { get; private set; }

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
        if (level <= 0)
            PlayerPrefs.SetInt(_levelKey, 1);
        else        
            PlayerPrefs.SetInt(_levelKey, level);
    }

    public void SaveMoney (int money)
    {
        if (money >= 0)
            PlayerPrefs.SetInt(_moneyKey, money);
    }

    public void SaveKills(int kills)
    {
        if (kills > 0)
            PlayerPrefs.SetInt(_killsKey, kills);
    }

    public void SaveHealthSkill(int healthSkill)
    {
        if (healthSkill <= 0)
            PlayerPrefs.SetInt(_healthSkillKey, 1);
        else
            PlayerPrefs.SetInt(_healthSkillKey, healthSkill);
    }

    public void SaveSpeedSkill(int speedSkill)
    {
        if (speedSkill <= 0)
            PlayerPrefs.SetInt(_speedSkillKey, 1);
        else
            PlayerPrefs.SetInt(_speedSkillKey, speedSkill);
    }

    public void SaveAllStats()
    {
        PlayerPrefs.Save();
    }

    public int GetSavedMoney()
    {
        return PlayerPrefs.GetInt(_moneyKey);
    }

    public int GetSavedKills()
    {
        return PlayerPrefs.GetInt(_killsKey);
    }

    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt(_levelKey);
    }

    public int GetSavedHealthSkill()
    {
        return PlayerPrefs.GetInt(_healthSkillKey);
    }

    public int GetSavedSpeedSkill()
    {
        return PlayerPrefs.GetInt(_speedSkillKey);
    }
}