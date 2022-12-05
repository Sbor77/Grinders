using Lean.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private LeanLocalization _leanLocalization;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private SceneLevelLoader _loaderPanel;
    [SerializeField] private LeadersPlanel _leadersPlanel;

    private int _indexLevel = 1;
    private List<Leader> leaders;
    private Leader player;
    private const int MaxDelay = 100;
    private const float WaitDelay = 0.25f;

    private void OnEnable()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
        _leaderboardButton.onClick.AddListener(ShowLeaderboard);
    }

    private void Start()
    {
        _leanLocalization.SetCurrentLanguage(DataHandler.Instance.GetSavedLanguage());

        if (DataHandler.Instance.GetSavedLevel() <= 1)
            _continueButton.interactable = false;
    }

    private void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(StartNewGame);
        _continueButton.onClick.RemoveListener(ContinueGame);
        _leaderboardButton.onClick.RemoveListener(ShowLeaderboard);
    }

    private void StartNewGame()
    {
        float masterVolume = DataHandler.Instance.GetSavedMasterVolume();
        float musicVolume = DataHandler.Instance.GetSavedMusicVolume();
        float effectsVolume = DataHandler.Instance.GetSavedEffectsVolume();
        
        DataHandler.Instance.DeleteAllStats();

        int defaultLevel = 1;
        int defaultHealthSkill = 1;
        int defaultSpeedSkill = 1;
        int defaultRadiusSkill = 1;
        int defaultKills = 0;
        int defaultTotalMoney = 0;
        int defaultLevelMoney = 0;

        DataHandler.Instance.SaveLevel(defaultLevel);
        DataHandler.Instance.SaveHealthSkill(defaultHealthSkill);
        DataHandler.Instance.SaveSpeedSkill(defaultSpeedSkill);
        DataHandler.Instance.SaveRadiusSkill(defaultRadiusSkill);
        DataHandler.Instance.SaveKills(defaultKills);
        DataHandler.Instance.SaveTotalMoney(defaultTotalMoney);
        DataHandler.Instance.SaveLevelMoney(defaultLevelMoney);
        DataHandler.Instance.SaveMasterVolume(masterVolume);
        DataHandler.Instance.SaveMusicVolume(musicVolume);
        DataHandler.Instance.SaveEffectsVolume(effectsVolume);
        DataHandler.Instance.SaveAllStats();
        
        _loaderPanel.LoadLevel(_indexLevel);
    }

    private void ContinueGame()
    {
        int currentLevel = DataHandler.Instance.GetSavedLevel();
        _loaderPanel.LoadLevel(currentLevel);
    }

    private void ShowLeaderboard()
    {
        _leadersPlanel.ShowLeaderboard();
    }
}
