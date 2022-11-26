using System;
using System.Collections;
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
    [SerializeField] private Image _CooldownTimer;

    private Movement _movement;
    private int _questCoinCollected;
    private int _questEnemyKills;
    private bool _questbigBoxDestroyed;
    private float _maxHealth;
    private float _currentHealth;
    private QuestInfo _missonConditions;
    private Coroutine _cooldownCoroutine;

    public event Action IsCurrentConditionsChanged;

    public QuestInfo MissionConditions => _missonConditions;

    public float CurrentHealth { get; private set; }

    public int CurrentKills { get; private set; }

    public int CurrentCoins { get; private set; }

    public bool IsBigboxDestroyed { get; private set; }

    private void Awake()
    {
        _movement = _player.GetComponent<Movement>();
    }

    private void OnEnable()
    {
        _player.ChangedHealth += OnChangedHealth;

        _player.ChangedCoin += OnChangedPlayerCoins;

        _enemySpawner.IsPLayerKillsIncreased += OnChangedPlayerKills;

        _boxSpawner.IsBigBoxCollected += OnDestroyBigBox;

        _movement.StartAttackCooldown += OnStartCooldown;

        _movement.ChangedState += OnStartAttack;
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

        _boxSpawner.IsBigBoxCollected -= OnDestroyBigBox;

        _movement.StartAttackCooldown -= OnStartCooldown;

        _movement.ChangedState -= OnStartAttack;
    }

    public void SetQuestCollected(QuestInfo conditions)
    {
        _questCoinCollected = conditions.NeedCoinCollected;

        _questbigBoxDestroyed = conditions.NeedDestroyBigBox;

        _questEnemyKills = conditions.NeedEnemyKilled;

        SetStartConditionsText();

        _missonConditions = conditions;
    }

    private void OnStartAttack(State state, AttackType type)
    {
        if (state == State.Attack)
            _CooldownTimer.fillAmount = 0;
    }

    private void OnStartCooldown(float delay)
    {
        _cooldownCoroutine = StartCoroutine(Cooldown(delay));

    }

    private IEnumerator Cooldown(float maxValue)
    {
        float value = 0;
        while (value < maxValue)
        {
            value = Mathf.Clamp(value + Time.deltaTime, 0, maxValue);
            _CooldownTimer.fillAmount = value / maxValue;
            yield return null;
        }
        yield return null;
    }

    private void SetStartConditionsText()
    {
        ChangeViewText(_goldText, 0, _questCoinCollected);
        ChangeViewText(_KillsText, 0, _questEnemyKills);
        ChangeViewText(_bigBoxText, 0, 1);
    }

    private void OnChangedHealth(float health)
    {
        _healthBarSlider.value = health;

        CurrentHealth = health;

    }

    private void OnChangedPlayerCoins(int value)
    {
        ChangeViewText(_goldText, value, _questCoinCollected);

        CurrentCoins = value;

        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnChangedPlayerKills(int value)
    {
        ChangeViewText(_KillsText, value, _questEnemyKills);

        CurrentKills = value;

        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnDestroyBigBox(int count)
    {
        ChangeViewText(_bigBoxText, count, 1);

        IsBigboxDestroyed = count == 1;

        IsCurrentConditionsChanged?.Invoke();
    }

    private void ChangeViewText(TMP_Text textField, int currentValue, int questValue)
    {
        if (questValue == 0)
            textField.text = currentValue.ToString();
        else
            textField.text = $"{currentValue.ToString()}/{questValue.ToString()}";
    }
}