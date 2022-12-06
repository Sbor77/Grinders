using Agava.WebUtility;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;


public class DataHandler : MonoBehaviour
{
    private string _totalScore = "TotalScore";
    private string _levelKey = "Level";
    private string _totalMoneyKey = "TotalMoney";
    private string _levelMoneyKey = "LevelMoney";
    private string _killsKey = "Kills";
    private string _healthSkillKey = "HealthSkill";    
    private string _speedSkillKey = "SpeedSkill";
    private string _radiusSkillKey = "RadiusSkill";
    private string _masterVolume = "MasterVolume";
    private string _musicVolume = "MusicVolume";
    private string _effectsVolume = "EffectsVolume";
    private string _language = "Language";
    private string _ru = "Russian";
    
    public static DataHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(this);
        }        
    }

    #region Event OnInBackground
   /* private void OnEnable()
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
        //Time.timeScale = inBackground ? 0f : 1f;
    }*/
    #endregion

    public void SaveLanguage(string lang)
    {
        PlayerPrefs.SetString(_language, lang);
        PlayerPrefs.Save();
    }

    public void SaveTotalScore(int score)
    {
        if (score <= 0)
            PlayerPrefs.SetInt(_totalScore, 0);
        else
            PlayerPrefs.SetInt(_totalScore, score);
    }

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

    public void SaveRadiusSkill(int radiusSkill)
    {
        if (radiusSkill <= 0)
            PlayerPrefs.SetInt(_radiusSkillKey, 1);
        else
            PlayerPrefs.SetInt(_radiusSkillKey, radiusSkill);
    }

    public void SaveMasterVolume(float value)
    {
        PlayerPrefs.SetFloat(_masterVolume, value);


      /*  if (value >= maxVolume)
            PlayerPrefs.SetFloat(_masterVolume, maxVolume);
        else
            PlayerPrefs.SetFloat(_masterVolume, minVolume);*/
    }

    public void SaveMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(_musicVolume, value);


        /*if (value >= 0 && value <= 1)
            PlayerPrefs.SetFloat(_musicVolume, value);
        else
            PlayerPrefs.SetFloat(_musicVolume, 1);*/
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

    public string GetSavedLanguage()
    {
        if (PlayerPrefs.HasKey(_language))
            return PlayerPrefs.GetString(_language);

        return _ru;
    }

    public int GetSavedTotalScore()
    {
        return PlayerPrefs.GetInt(_totalScore);
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

    public int GetSavedRadiusSkill()
    {
        return PlayerPrefs.GetInt(_radiusSkillKey);
    }

    public float GetSavedMasterVolume()
    {        
        if (PlayerPrefs.HasKey(_masterVolume))
            return PlayerPrefs.GetFloat(_masterVolume);
        else
            return -40;
    }

    public float GetSavedMusicVolume()
    {
        if (PlayerPrefs.HasKey(_musicVolume))
            return PlayerPrefs.GetFloat(_musicVolume);
        else
            return -40;
    }

    public float GetSavedEffectsVolume()
    {
        if (PlayerPrefs.HasKey(_effectsVolume))
            return PlayerPrefs.GetFloat(_effectsVolume);
        else
            return -40;
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