using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private SceneLevelLoader _loaderPanel;

    private int _indexLevel = 1;

    private void OnEnable()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
        _continueButton.onClick.AddListener(ContinueGame);
    }

    private void Start()
    {
        if (DataHandler.Instance.GetSavedLevel() <= 1)
            _continueButton.interactable = false;
    }

    private void OnDisable()
    {
        _newGameButton.onClick.RemoveListener(StartNewGame);
        _continueButton.onClick.RemoveListener(ContinueGame);
    }

    private void StartNewGame()
    {
        DataHandler.Instance.DeleteAllStats();

        int defaultLevel = 1;
        int defaultHealthSkill = 1;
        int defaultSpeedSkill = 1;
        int defaultKills = 0;
        int defaultTotalMoney = 0;
        int defaultLevelMoney = 0;

        DataHandler.Instance.SaveLevel(defaultLevel);
        DataHandler.Instance.SaveHealthSkill(defaultHealthSkill);
        DataHandler.Instance.SaveSpeedSkill(defaultSpeedSkill);
        DataHandler.Instance.SaveKills(defaultKills);
        DataHandler.Instance.SaveTotalMoney(defaultTotalMoney);
        DataHandler.Instance.SaveLevelMoney(defaultLevelMoney);
        DataHandler.Instance.SaveAllStats();

        _loaderPanel.LoadLevel(_indexLevel);
    }

    private void ContinueGame()
    {
        int currentLevel = DataHandler.Instance.GetSavedLevel();
        _loaderPanel.LoadLevel(currentLevel);
    }
}
