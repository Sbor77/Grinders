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
        if (PlayerPrefs.HasKey(_totalMoneyKey))
            return PlayerPrefs.GetInt(_totalMoneyKey);            
        else
            return 0;
    }

    public int GetSavedLevelMoney()
    {
        if (PlayerPrefs.HasKey(_levelMoneyKey))
            return PlayerPrefs.GetInt(_levelMoneyKey);
        else
            return 0;        
    }

    public int GetSavedKills()
    {
        if (PlayerPrefs.HasKey(_killsKey))
            return PlayerPrefs.GetInt(_killsKey);
        else
            return 0;        
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
        float defaultVolume = 1;

        if (PlayerPrefs.HasKey(_totalVolume))
            return PlayerPrefs.GetFloat(_totalVolume);
        else
            return defaultVolume;
    }

    public float GetSavedMusicVolume()
    {
        float defaultVolume = 1;

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