using System.Runtime.InteropServices;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    private string _totalScore = "TotalScore";
    private string _level = "Level";
    private string _completedLevel = "_completedLevel";
    private string _currentZone = "CurrentZone";
    private string _totalMoney = "TotalMoney";
    private string _levelMoney = "LevelMoney";
    private string _kills = "Kills";
    private string _healthSkill = "HealthSkill";
    private string _speedSkill = "SpeedSkill";
    private string _radiusSkill = "RadiusSkill";
    private string _muteValue = "MuteStateValue";    
    private string _totalVolume = "TotalVol";
    private string _musicVolume = "MusicVol";
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
            PlayerPrefs.SetInt(_level, 0);
        else
            PlayerPrefs.SetInt(_level, level);
    }

    public void SaveCompletedLevel(int completedLevel)
    {
        if (completedLevel <= 0)
            PlayerPrefs.SetInt(_completedLevel, 0);
        else
            PlayerPrefs.SetInt(_completedLevel, completedLevel);
    }

    public void SaveCurrentZoneIndex(int value)
    {
        if (value >= 0)            
            PlayerPrefs.SetInt(_currentZone, value);
    }

    public void SaveTotalMoney(int totalMoney)
    {
        if (totalMoney >= 0)
            PlayerPrefs.SetInt(_totalMoney, totalMoney);
    }

    public void SaveLevelMoney (int levelMoney)
    {
        if (levelMoney >= 0)
            PlayerPrefs.SetInt(_levelMoney, levelMoney);
    }

    public void SaveKills(int kills)
    {
        if (kills > 0)
            PlayerPrefs.SetInt(_kills, kills);
    }

    public void SaveHealthSkill(int healthSkill)
    {
        if (healthSkill <= 0)
            PlayerPrefs.SetInt(_healthSkill, 1);
        else
            PlayerPrefs.SetInt(_healthSkill, healthSkill);
    }

    public void SaveSpeedSkill(int speedSkill)
    {
        if (speedSkill <= 0)
            PlayerPrefs.SetInt(_speedSkill, 1);
        else
            PlayerPrefs.SetInt(_speedSkill, speedSkill);
    }

    public void SaveRadiusSkill(int radiusSkill)
    {
        if (radiusSkill <= 0)
            PlayerPrefs.SetInt(_radiusSkill, 1);
        else
            PlayerPrefs.SetInt(_radiusSkill, radiusSkill);
    }

    public void SaveMuteValue(int value)
    {        
        PlayerPrefs.SetInt(_muteValue, value);        
    }

    public void SaveTotalVolume(float value)
    {
        PlayerPrefs.SetFloat(_totalVolume, value);      
    }

    public void SaveMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(_musicVolume, value);
    }

    public void SaveAllStats()
    {
        PlayerPrefs.Save();
    }

    public void DeleteAllStatsWithExcludes()
    {
        string language = GetSavedLanguage();
        int muteValue = GetSavedMuteValue();
        float totalVolume = GetSavedTotalVolume();
        float musicVolume = GetSavedMusicVolume();

        PlayerPrefs.DeleteAll();

        SaveLanguage(language);
        SaveMuteValue(muteValue);
        SaveTotalVolume(totalVolume);
        SaveMusicVolume(musicVolume);        
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
        if (PlayerPrefs.HasKey(_totalMoney))
            return PlayerPrefs.GetInt(_totalMoney);            
        else
            return 0;
    }

    public int GetSavedLevelMoney()
    {
        if (PlayerPrefs.HasKey(_levelMoney))
            return PlayerPrefs.GetInt(_levelMoney);
        else
            return 0;        
    }

    public int GetSavedKills()
    {
        if (PlayerPrefs.HasKey(_kills))
            return PlayerPrefs.GetInt(_kills);
        else
            return 0;        
    }

    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt(_level);
    }

    public int GetSavedCompletedLevel()
    {
        return PlayerPrefs.GetInt(_completedLevel);
    }

    public int GetSavedCurrentZoneIndex()
    {
        if (PlayerPrefs.HasKey(_currentZone))
            return PlayerPrefs.GetInt(_currentZone);
        else
            return 0;        
    }

    public int GetSavedHealthSkill()
    {
        if (PlayerPrefs.HasKey(_healthSkill))
            return PlayerPrefs.GetInt(_healthSkill);
        else
            return 1;
    }

    public int GetSavedSpeedSkill()
    {
        if (PlayerPrefs.HasKey(_speedSkill))
            return PlayerPrefs.GetInt(_speedSkill);
        else
            return 1;
    }

    public int GetSavedRadiusSkill()
    {
        if (PlayerPrefs.HasKey(_radiusSkill))
            return PlayerPrefs.GetInt(_radiusSkill);
        else
            return 1;
    }

    public int GetSavedMuteValue()
    {
        int unmuteValue = 1;

        if (PlayerPrefs.HasKey(_muteValue))
            return PlayerPrefs.GetInt(_muteValue);
        else
            return unmuteValue;
    }

    public float GetSavedTotalVolume()
    {
        float defaultVolume = 0.5f;

        if (PlayerPrefs.HasKey(_totalVolume))
            return PlayerPrefs.GetFloat(_totalVolume);
        else
            return defaultVolume;
    }

    public float GetSavedMusicVolume()
    {
        float defaultVolume = 0.5f;

        if (PlayerPrefs.HasKey(_musicVolume))
            return PlayerPrefs.GetFloat(_musicVolume);
        else
            return defaultVolume;
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