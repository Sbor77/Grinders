using System;
//using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
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
//    [SerializeField] private Image _attackCooldownTimer;
    [SerializeField] private Image _massKillButtonCooldown;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _volumeOn;
    [SerializeField] private Sprite _volumeOff;
    [SerializeField] private AudioMixer _audio;

    private Movement _movement;
    private Button _soundButton;
    private int _questCoinCollected;
    private int _questEnemyKills;
    private bool _questbigBoxDestroyed;
    private float _maxHealth;
    private float _currentHealth;
    private QuestInfo _missonConditions;
//    private Coroutine _cooldownCoroutine;

    public event Action IsCurrentConditionsChanged;

    public QuestInfo MissionConditions => _missonConditions;
    public float CurrentHealth { get; private set; }
    public int CurrentKills { get; private set; }
    public int CurrentCoins { get; private set; }
    public bool IsBigboxDestroyed { get; private set; }

    private const string Master = "MasterVolume";
    private const float MaxVolume = 0;
    private const float MinVolume = -80;


    private void Awake()
    {
        _movement = _player.GetComponent<Movement>();
        _soundButton = _soundImage.GetComponent<Button>();
    }

    private void OnEnable()
    {
        _player.ChangedHealth += OnChangedHealth;

        _player.ChangedCoin += OnChangedPlayerCoins;

        _enemySpawner.IsPLayerKillsIncreased += OnChangedPlayerKills;

        _boxSpawner.IsBigBoxCollected += OnDestroyBigBox;

        _soundButton.onClick.AddListener(OnChangedSoundVolume);
        //_movement.StartAttackCooldown += OnStartCooldown;
        //_movement.ChangedState += OnStartAttack;
        _movement.ChangedMassAttackCooldown += OnChangedMassCooldown;
    }

    void Start()
    {
        _maxHealth = _player.MaxHealth;

        _currentHealth = _maxHealth; // load in PlayerPrefs

        SetSoundIcon(DataHandler.Instance.GetSavedTotalVolume());

        _healthBarSlider.maxValue = _maxHealth;

        _healthBarSlider.value = _currentHealth;
    }

    private void OnDisable()
    {
        _enemySpawner.IsPLayerKillsIncreased -= OnChangedPlayerKills; 
        
        _player.ChangedHealth -= OnChangedHealth;

        _player.ChangedCoin -= OnChangedPlayerCoins;

        _boxSpawner.IsBigBoxCollected -= OnDestroyBigBox;

        _soundButton.onClick.RemoveListener(OnChangedSoundVolume);
        //_movement.StartAttackCooldown -= OnStartCooldown;
        //_movement.ChangedState -= OnStartAttack;
        _movement.ChangedMassAttackCooldown -= OnChangedMassCooldown;
    }

    public void SetQuestCollected(QuestInfo conditions)
    {
        _questCoinCollected = conditions.NeedCoinCollected;

        _questbigBoxDestroyed = conditions.NeedDestroyBigBox;

        _questEnemyKills = conditions.NeedEnemyKilled;

        SetStartConditionsText();

        _missonConditions = conditions;
    }

    private void OnChangedSoundVolume()
    {
        _audio.GetFloat(Master, out float volume);

        if (volume == MaxVolume)
            volume = MinVolume;
        else
            volume = MaxVolume;

        SetSoundIcon(volume);

        _audio.SetFloat(Master, volume);

        DataHandler.Instance.SaveTotalVolume(volume);
    }

    private void SetSoundIcon(float volume)
    {
        if (volume == MaxVolume)
            _soundImage.sprite = _volumeOn;
        else
            _soundImage.sprite = _volumeOff;
    }

    private void OnChangedMassCooldown(float current, int max)
    {
        float value;
        value = Mathf.Clamp(current, 0, max);
        _massKillButtonCooldown.fillAmount = 1 - value / max;
    }

    //private void OnStartAttack(State state, bool mass)
    //{
    //    if (state == State.Attack)
    //        _attackCooldownTimer.fillAmount = 0;
    //}

    //private void OnStartCooldown(float delay)
    //{
    //    _cooldownCoroutine = StartCoroutine(Cooldown(delay));

    //}

    //private IEnumerator Cooldown(float maxValue)
    //{
    //    float value = 0;
    //    while (value < maxValue)
    //    {
    //        value = Mathf.Clamp(value + Time.deltaTime, 0, maxValue);
    //        _attackCooldownTimer.fillAmount = value / maxValue;
    //        yield return null;
    //    }
    //    yield return null;
    //}

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