//using WebUtility;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private string _levelKey = "Level";
    private string _totalMoneyKey = "TotalMoney";
    private string _levelMoneyKey = "LevelMoney";
    private string _killsKey = "Kills";
    private string _healthSkillKey = "HealthSkill";    
    private string _speedSkillKey = "SpeedSkill";
    private string _masterVolume = "MasterVolume";
    private string _musicVolume = "MusicVolume";
    private string _effectsVolume = "EffectsVolume";

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

    #region Event OnInBackground
    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        AudioListener.pause = inBackground;
        AudioListener.volume = inBackground ? 0f : 1f;
        Time.timeScale = inBackground ? 0f : 1f;
    }
    #endregion

    public void SaveLevel (int level)
    {
        if (level <= 0)
            PlayerPrefs.SetInt(_levelKey, 1);
        else        
            PlayerPrefs.SetInt(_levelKey, level);
    }

    public void SaveTotalMoney(int totalMoney)
    {
        if (totalMoney >= 0)
            PlayerPrefs.SetInt(_totalMoneyKey, totalMoney);
    }

    public void SaveLevelMoney (int levelMoney)
    {
        if (levelMoney >= 0)
            PlayerPrefs.SetInt(_levelMoneyKey, levelMoney);
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

    public void SaveMasterVolume(float value)
    {
        if (value >= 0 && value <= 1)
            PlayerPrefs.SetFloat(_masterVolume, value);
        else
            PlayerPrefs.SetFloat(_masterVolume, 1);
    }

    public void SaveMusicVolume(float value)
    {
        if (value >= 0 && value <= 1)
            PlayerPrefs.SetFloat(_musicVolume, value);
        else
            PlayerPrefs.SetFloat(_musicVolume, 1);
    }

    public void SaveEffectsVolume(float value)
    {
        if (value >= 0 && value <= 1)
            PlayerPrefs.SetFloat(_effectsVolume, value);
        else
            PlayerPrefs.SetFloat(_effectsVolume, 1);
    }

    public void SaveAllStats()
    {
        PlayerPrefs.Save();
    }

    public void DeleteAllStats()
    {
        PlayerPrefs.DeleteAll();
    }

    public int GetSavedTotalMoney()
    {
        return PlayerPrefs.GetInt(_totalMoneyKey);
    }

    public int GetSavedLevelMoney()
    {
        return PlayerPrefs.GetInt(_levelMoneyKey);
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

    public float GetSavedMasterVolume()
    {
        if (PlayerPrefs.HasKey(_masterVolume))
            return PlayerPrefs.GetFloat(_masterVolume);
        else
            return 1;
    }

    public float GetSavedMusicVolume()
    {
        if (PlayerPrefs.HasKey(_musicVolume))
            return PlayerPrefs.GetFloat(_musicVolume);
        else
            return 1;
    }

    public float GetSavedEffectsVolume()
    {
        if (PlayerPrefs.HasKey(_effectsVolume))
            return PlayerPrefs.GetFloat(_effectsVolume);
        else
            return 1;
    }


    #region Import WebGL for check mobile platform

    [DllImport("__Internal")]
    private static extern bool IsMobilePlatform();

    public bool IsMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        return IsMobilePlatform();
#endif
        return false;
    }
    
    #endregion
}