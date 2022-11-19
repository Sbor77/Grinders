using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; }

    public string LevelString => "Level";
    public string MoneyString => "Money";
    public string KillsString => "Kills";
    public string HealthSkillString => "HealthSkill";
    public string SpeedSkillString => "SpeedSkill";

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
            PlayerPrefs.SetInt(LevelString, 1);
        else        
            PlayerPrefs.SetInt(LevelString, level);
    }

    public void SaveMoney (int money)
    {
        if (money >= 0)
            PlayerPrefs.SetInt(MoneyString, money);
    }

    public void SaveKills(int kills)
    {
        if (kills >= 0)
            PlayerPrefs.SetInt(KillsString, kills);
    }

    public void SaveHealthSkill(int healthSkill)
    {
        if (healthSkill <= 0)
            PlayerPrefs.SetInt(HealthSkillString, 1);
        else
            PlayerPrefs.SetInt(HealthSkillString, healthSkill);
    }

    public void SaveSpeedSkill(int speedSkill)
    {
        if (speedSkill <= 0)
            PlayerPrefs.SetInt(SpeedSkillString, 1);
        else
            PlayerPrefs.SetInt(SpeedSkillString, speedSkill);
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

    public int GetSavedHealthSkill()
    {
        return PlayerPrefs.GetInt(HealthSkillString);
    }

    public int GetSavedSpeedSkill()
    {
        return PlayerPrefs.GetInt(SpeedSkillString);
    }
}