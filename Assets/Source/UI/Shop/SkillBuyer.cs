using System;
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

    public void Init()
    {
        _healthPriceText.text = _healthLevelPrices[DataHandler.Instance.GetSavedHealthSkill()].ToString();
        _movePriceText.text = _moveLevelPrices[DataHandler.Instance.GetSavedSpeedSkill()].ToString();
        ButtonIsValid();
    }

    private void ButtonIsValid()
    {
        if (DataHandler.Instance.GetSavedHealthSkill() >= _healthLevelPrices.Length)
            _buyHealthButton.interactable = false;

        if (DataHandler.Instance.GetSavedSpeedSkill() >= _moveLevelPrices.Length)
            _buyMoveButton.interactable = false;
    }

    private void OnHealthBuy()
    {
        int health = DataHandler.Instance.GetSavedHealthSkill();
        int price = _healthLevelPrices[health];

        if (TryBuying(price))
        {
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
            DataHandler.Instance.SaveSpeedSkill(moveSpeed + 1);

            IsStatBought?.Invoke();
        }
    }

    private bool TryBuying(int statPrice)
    {
        int money = DataHandler.Instance.GetSavedMoney();

        if (money >= statPrice)
        {
            DataHandler.Instance.SaveMoney(money - statPrice);
            return true;
        }

        return false;
    }
}