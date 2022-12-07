using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillBuyer : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthPriceText;
    [SerializeField] private TMP_Text _movePriceText;
    [SerializeField] private TMP_Text _radiusPriceText;
    [SerializeField] private TMP_Text _rewardedCoinsText;
    [SerializeField] private Button _buyMoveButton;
    [SerializeField] private Button _buyHealthButton;
    [SerializeField] private Button _buyRadiusButton;
    [SerializeField] private Button _buyCoinsButton;
    [SerializeField] private int[] _moveLevelPrices;
    [SerializeField] private int[] _healthLevelPrices;
    [SerializeField] private int[] _radiusLevelPrices;
    [SerializeField] private int _rewardedCoins = 50;
    [SerializeField] private AudioSource _buySFX;    

    public event Action IsStatBought;

    private void OnEnable()
    {
        _buyHealthButton.onClick.AddListener(OnHealthBuy);

        _buyMoveButton.onClick.AddListener(OnMoveSpeedBuy);

        _buyRadiusButton.onClick.AddListener(OnRadiusBuy);

        _buyCoinsButton.onClick.AddListener(OnVideoAdBuyCoin);

        if (GamesSdk.Instance != null)        
            GamesSdk.Instance.Rewarded += OnRewardedCoinsBuy;
        
    }

    private void OnDisable()
    {
        _buyHealthButton.onClick.RemoveListener(OnHealthBuy);

        _buyMoveButton.onClick.RemoveListener(OnMoveSpeedBuy);

        _buyRadiusButton.onClick.RemoveListener(OnRadiusBuy);

        _buyCoinsButton.onClick.RemoveListener(OnVideoAdBuyCoin);

        if (GamesSdk.Instance != null)
            GamesSdk.Instance.Rewarded -= OnRewardedCoinsBuy;
    }

    public void Init()
    {
        _rewardedCoinsText.text = $"+{_rewardedCoins}";

        if (DataHandler.Instance.GetSavedHealthSkill() >= _healthLevelPrices.Length)        
            _healthPriceText.text = "MAX!";
        else
            _healthPriceText.text = _healthLevelPrices[DataHandler.Instance.GetSavedHealthSkill()].ToString();
        
        if (DataHandler.Instance.GetSavedSpeedSkill() >= _moveLevelPrices.Length)
            _movePriceText.text = "MAX!"; 
        else
            _movePriceText.text = _moveLevelPrices[DataHandler.Instance.GetSavedSpeedSkill()].ToString();

        if (DataHandler.Instance.GetSavedRadiusSkill() >= _radiusLevelPrices.Length)
            _radiusPriceText.text = "MAX!";
        else
            _radiusPriceText.text = _radiusLevelPrices[DataHandler.Instance.GetSavedRadiusSkill()].ToString();

        ButtonIsValid();
    }

    private void ButtonIsValid()
    {
        if (DataHandler.Instance.GetSavedHealthSkill() >= _healthLevelPrices.Length)
            _buyHealthButton.interactable = false;

        if (DataHandler.Instance.GetSavedSpeedSkill() >= _moveLevelPrices.Length)
            _buyMoveButton.interactable = false;

        if (DataHandler.Instance.GetSavedRadiusSkill() >= _radiusLevelPrices.Length)
            _buyRadiusButton.interactable = false;
    }

    private void OnHealthBuy()
    {
        int health = DataHandler.Instance.GetSavedHealthSkill();

        int price = _healthLevelPrices[health];

        if (TryBuying(price))
        {
            _buySFX.Play();
            DataHandler.Instance.SaveHealthSkill(health + 1);
            DataHandler.Instance.SaveAllStats();

            IsStatBought?.Invoke();
        }
    }

    private void OnMoveSpeedBuy()
    {
        int moveSpeed = DataHandler.Instance.GetSavedSpeedSkill();

        int price = _moveLevelPrices[moveSpeed];

        if (TryBuying(price))
        {
            _buySFX.Play();
            DataHandler.Instance.SaveSpeedSkill(moveSpeed + 1);

            IsStatBought?.Invoke();
        }
    }

    private void OnRadiusBuy()
    {
        int radius = DataHandler.Instance.GetSavedRadiusSkill();

        int price = _radiusLevelPrices[radius];

        if (TryBuying(price))
        {
            _buySFX.Play();
            DataHandler.Instance.SaveRadiusSkill(radius + 1);

            IsStatBought?.Invoke();
        }
    }

    private void OnVideoAdBuyCoin()
    {
        GamesSdk.Instance.VideoAdShow();
    }

    private void OnRewardedCoinsBuy()
    {
        int playerMoneyWithReward = DataHandler.Instance.GetSavedTotalMoney() + _rewardedCoins;

        DataHandler.Instance.SaveTotalMoney(playerMoneyWithReward);

        IsStatBought?.Invoke();
    }

    private bool TryBuying(int statPrice)
    {
        int money = DataHandler.Instance.GetSavedTotalMoney();

        if (money >= statPrice)
        {
            DataHandler.Instance.SaveTotalMoney(money - statPrice);
            return true;
        }

        return false;
    }
}