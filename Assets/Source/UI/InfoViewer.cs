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
    [SerializeField] private TMP_Text _KillsText;

    private float _maxHealth;
    private float _currentHealth;

    void Start()
    {
        _maxHealth = _player.MaxHealth;
        _currentHealth = _maxHealth; // load in PlayerPrefs
        _healthBarSlider.maxValue = _maxHealth;
        _healthBarSlider.value = _currentHealth;
        _player.ChangedHealth += OnChangedHealth;
    }

    private void OnChangedHealth(float health)
    {
        _healthBarSlider.value = health;
    }
}
