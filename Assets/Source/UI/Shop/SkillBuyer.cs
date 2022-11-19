using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBuyer : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthPriceText;
    [SerializeField] private TMP_Text _movePriceText;
    [SerializeField] private Button _buyMoveButton;
    [SerializeField] private Button _buyHealthButton;
    [SerializeField] private int[] _moveLevelPrices;
    [SerializeField] private int[] _healthLevelPrices;

    //private StatsData _statsInfo;
    private const string Money = "Money";
    private const string Health = "Health";
    private const string MoveSpeed = "Speed";

    public event Action IsStatBought; 

    private void OnEnable()
    {
        _buyHealthButton.onClick.AddListener(OnHealthBuy);
        _buyMoveButton.onClick.AddListener(OnMoveSpeedBuy);
    }

    private void OnDisable()
    {
        _buyHealthButton.onClick.RemoveListener(OnHealthBuy);
        _buyMoveButton.onClick.RemoveListener(OnMoveSpeedBuy);
    }

    /*public void Init(StatsInfo info)
    {
        _statsInfo = info;

        _healthPriceText.text = _healthLevelPrices[info.Health].ToString();

        _movePriceText.text = _moveLevelPrices[info.MoveSpeed].ToString();
    }*/

    public void Init()
    {
        _healthPriceText.text = _healthLevelPrices[DataHandler.Instance.Health].ToString();
        _movePriceText.text = _moveLevelPrices[DataHandler.Instance.MoveSpeed].ToString();
    }

    /*private void OnHealthBuy()
    {
        int price = _healthLevelPrices[_statsInfo.Health];

        if (IsMoneyEnough(price))
        {
            BuyStat(_statsInfo.Money - price, Health, _statsInfo.Health + 1);

            IsStatBought?.Invoke();
        }
    }

    private void OnMoveSpeedBuy()
    {
        int price = _moveLevelPrices[_statsInfo.MoveSpeed];

        if (IsMoneyEnough(price))
        {
            BuyStat(_statsInfo.Money - price, MoveSpeed, _statsInfo.MoveSpeed + 1);

            IsStatBought?.Invoke();
        }
    }*/

    private void OnHealthBuy()
    {
        int price = _healthLevelPrices[DataHandler.Instance.Health];

        if (IsMoneyEnough(price))
        {
            BuyStat(DataHandler.Instance.Money - price, DataHandler.Instance.HealthString, DataHandler.Instance.Health + 1);

            IsStatBought?.Invoke();
        }
    }

    private void OnMoveSpeedBuy()
    {
        int price = _moveLevelPrices[DataHandler.Instance.MoveSpeed];

        if (IsMoneyEnough(price))
        {
            BuyStat(DataHandler.Instance.Money - price, DataHandler.Instance.MoveSpeedString, DataHandler.Instance.MoveSpeed + 1);

            IsStatBought?.Invoke();
        }
    }

    private void BuyStat(int newGoldsValue, string boughtStatName, int newStatValue)
    {
        //SetValue(Money, newGoldsValue);
        DataHandler.Instance.SaveStat(DataHandler.Instance.MoneyString, newGoldsValue);
        DataHandler.Instance.SaveStat(boughtStatName, newStatValue);
        //SetValue(boughtStatName, newStatValue);

        //PlayerPrefs.Save();
    }

    //private void SetValue(string name, int value)
    //{
    //    PlayerPrefs.SetInt(name, value);
    //}

    /*private bool IsMoneyEnough(int statPrice)
    {
        if (_statsInfo.Money >= statPrice)
        {
            return true;
        }

        return false;
    }*/

    private bool IsMoneyEnough(int statPrice)
    {
        if (DataHandler.Instance.Money >= statPrice)
            return true;

        return false;
    }
}