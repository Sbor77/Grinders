using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBuying : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthPriceText;
    [SerializeField] private TMP_Text _movePriceText;
    [SerializeField] private Button _buyMoveButton;
    [SerializeField] private Button _buyHealthButton;
    [SerializeField] private int[] _moveLevelPrices;
    [SerializeField] private int[] _healthLevelPrices;

    private StatsInfo _statsInfo;

    private const string Golds = "Golds";
    private const string Health = "Health";
    private const string MoveSpeed = "Speed";

    public event Action ChangedStatsInfo; 

    private void OnEnable()
    {
        _buyHealthButton.onClick.AddListener(OnBuyHealth);
        _buyMoveButton.onClick.AddListener(OnBuyMoveSpeed);
    }

    private void OnDisable()
    {
        _buyHealthButton.onClick.RemoveListener(OnBuyHealth);
        _buyMoveButton.onClick.RemoveListener(OnBuyMoveSpeed);
    }

    public void Init(StatsInfo info)
    {
        _statsInfo = info;
        _healthPriceText.text = _healthLevelPrices[info.Health].ToString();
        _movePriceText.text = _moveLevelPrices[info.MoveSpeed].ToString();
    }

    private void OnBuyHealth()
    {
        int price = _healthLevelPrices[_statsInfo.Health];

        if (TryBuying(price))
        {
            UpdateStats(_statsInfo.Golds - price, Health, _statsInfo.Health + 1);
            ChangedStatsInfo?.Invoke();
        }
    }

    private void OnBuyMoveSpeed()
    {
        int price = _moveLevelPrices[_statsInfo.MoveSpeed];

        if (TryBuying(price))
        {
            UpdateStats(_statsInfo.Golds - price, MoveSpeed, _statsInfo.MoveSpeed + 1);
            ChangedStatsInfo?.Invoke();
        }
    }

    private void UpdateStats(int newGoldsValue, string nameChangedStat, int newStatValue)
    {
        SetValue(Golds, newGoldsValue);
        SetValue(nameChangedStat, newStatValue);
        PlayerPrefs.Save();
    }

    private void SetValue(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }

    private bool TryBuying(int price)
    {
        if (_statsInfo.Golds >= price)
        {
            return true;
        }

        return false;
    }
}
