using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoViewer : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private Player _player;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private BoxSpawner _boxSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private TMP_Text _bigBoxText;
    [SerializeField] private TMP_Text _KillsText;

    private int _questCoinCollected;
    private int _questEnemyKills;
    private bool _questbigBoxDestroyed;
    private float _maxHealth;
    private float _currentHealth;
    private QuestInfo _missonConditions;    

    public event Action IsCurrentConditionsChanged;

    public QuestInfo MissionConditions => _missonConditions;

    public float CurrentHealth { get; private set; }
    public int CurrentKills { get; private set; }
    public int CurrentCoins { get; private set; }
    public bool IsBigboxDestroyed { get; private set; }


    private void OnEnable()
    {
        _player.ChangedHealth += OnChangedHealth;
        _player.ChangedCoin += OnChangedPlayerCoins;
        _enemySpawner.IsPLayerKillsIncreased += OnChangedPlayerKills;        
    }

    void Start()
    {
        _maxHealth = _player.MaxHealth;
        _currentHealth = _maxHealth; // load in PlayerPrefs
        _healthBarSlider.maxValue = _maxHealth;
        _healthBarSlider.value = _currentHealth;        
    }

    private void OnDisable()
    {
        _enemySpawner.IsPLayerKillsIncreased -= OnChangedPlayerKills;        
        _player.ChangedHealth -= OnChangedHealth;
        _player.ChangedCoin -= OnChangedPlayerCoins;
    }

    public void SetQuestCollected(QuestInfo conditions)
    {
        _questCoinCollected = conditions.NeedCoinCollected;
        _questbigBoxDestroyed = conditions.NeedDestroyBigBox;
        _questEnemyKills = conditions.NeedEnemyKilled;
        SetStartConditionsText();

        _missonConditions = conditions;
    }

    private void SetStartConditionsText()
    {
        OnChangedPlayerCoins(0);
        OnChangedPlayerKills(0);
        OnDestroyBigBox(0);
    }

    private void OnChangedHealth(float health)
    {
        _healthBarSlider.value = health;

        CurrentHealth = health;
        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnChangedPlayerCoins(int value)
    {
        if (_questCoinCollected == 0)
            _goldText.text = value.ToString();
        else
        _goldText.text = $"{value.ToString()}/{_questCoinCollected}";

        CurrentCoins = value;
        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnChangedPlayerKills(int value)
    {
        if (_questEnemyKills == 0)
            _KillsText.text = value.ToString();
        else
            _KillsText.text = $"{value.ToString()}/{_questEnemyKills}";

        CurrentKills = value;
        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnDestroyBigBox(int count)
    {
        if (_questbigBoxDestroyed)
            _bigBoxText.text = $"{count.ToString()}/1";
        else
            _bigBoxText.text = count.ToString();

        IsBigboxDestroyed = count == 1;
        IsCurrentConditionsChanged?.Invoke();
    }
}
