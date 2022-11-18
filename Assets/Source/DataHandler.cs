using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public static DataHandler Instance { get; private set; }

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
}