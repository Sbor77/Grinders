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
    [SerializeField] private TMP_Text _KillsText;

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

    private void OnChangedHealth(float health)
    {
        _healthBarSlider.value = health;
    }

    private void OnChangedPlayerCoins(int value)
    {
        print("Get " + value);
        _goldText.text = value.ToString();
    }

    private void OnChangedPlayerKills(int value)
    {
        _KillsText.text = value.ToString();
    }
}
