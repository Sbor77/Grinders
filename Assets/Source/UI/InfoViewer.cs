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

    private void OnEnable()
    {
        _player.ChangedHealth += OnChangedHealth;
        _enemySpawner.IsPLayerKillsIncreased += OnChangedPlayerKills;
        _boxSpawner.IsPlayerMoneyIncreased += OnChangedPlayerCoins;
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
        _boxSpawner.IsPlayerMoneyIncreased -= OnChangedPlayerCoins;
        _player.ChangedHealth -= OnChangedHealth;
    }

    public void SetQuestCollected(QuestInfo conditions)
    {
        _questCoinCollected = conditions.NeedCoinCollected;
        _questbigBoxDestroyed = conditions.NeedDestroyBigBox;
        _questEnemyKills = conditions.NeedEnemyKilled;
        SetStartConditionsText();
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
    }

    private void OnChangedPlayerCoins(int value)
    {
        if (_questCoinCollected == 0)
            _goldText.text = value.ToString();
        else
        _goldText.text = $"{value.ToString()}/{_questCoinCollected}";
    }

    private void OnChangedPlayerKills(int value)
    {
        if (_questEnemyKills == 0)
            _KillsText.text = value.ToString();
        else
            _KillsText.text = $"{value.ToString()}/{_questEnemyKills}";
    }

    private void OnDestroyBigBox(int count)
    {
        if (_questbigBoxDestroyed)
            _bigBoxText.text = $"{count.ToString()}/1";
        else
            _bigBoxText.text = count.ToString();
    }
}
