using System;
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
    [SerializeField] private TMP_Text _enemyKillsText;
    [SerializeField] private TMP_Text _bossKillsText;
    [SerializeField] private GameObject _bossesField;
    [SerializeField] private MassAttackView _massKillButtonCooldown;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _volumeOn;
    [SerializeField] private Sprite _volumeOff;

    public event Action IsCurrentConditionsChanged;

    private const int MaxVolume = 1;
    private const int MinVolume = 0;
    private Movement _movement;
    private Button _soundButton;
    //private QuestInfo _missonConditions;
    private int _questCoinCollected;
    private int _questEnemyKills;
    private int _questBossEnemyKills;
    private int _questBigboxCount = 1;
    private bool _questBigbox;
    private float _maxHealth;
    private float _currentHealth;

    private int _currentZoneKills;
    private int _currentZoneCoins;
    private int _bossKills;

    public int CurrentZoneKills => _currentZoneKills;
    public int CurrentZoneCoins => _currentZoneCoins;
    public int CurrentZoneBossKills => _bossKills;

    //public QuestInfo MissionConditions => _missonConditions;
    public float CurrentHealth { get; private set; }
    public int LevelKills { get; private set; }
    public int LevelCoins { get; private set; }
    public bool IsBigboxDestroyed { get; private set; }

    private void Awake()
    {
        _movement = _player.GetComponent<Movement>();
        _soundButton = _soundImage.GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (_boxSpawner != null)
            _boxSpawner.IsBigBoxCollected += OnDestroyBigBox;

        if (_enemySpawner != null)
        {
            _enemySpawner.IsPLayerKillsIncreased += OnChangedPlayerKills;
            _enemySpawner.IsBossKilled += OnBossKilled;
        }            

        _player.ChangedHealth += OnChangedHealth;
        _player.ChangedCoin += OnChangedPlayerCoins;
        _soundButton.onClick.AddListener(OnMuteToggled);
        _movement.ChangedMassAttackCooldown += OnChangedMassCooldown;
    }

    private void OnDisable()
    {
        if (_boxSpawner != null)
            _boxSpawner.IsBigBoxCollected -= OnDestroyBigBox;

        if (_enemySpawner != null)
        {
            _enemySpawner.IsPLayerKillsIncreased -= OnChangedPlayerKills;
            _enemySpawner.IsBossKilled -= OnBossKilled;
        }

        _player.ChangedHealth -= OnChangedHealth;
        _player.ChangedCoin -= OnChangedPlayerCoins;
        _soundButton.onClick.RemoveListener(OnMuteToggled);
        _movement.ChangedMassAttackCooldown -= OnChangedMassCooldown;
    }

    void Start()
    {
        _maxHealth = _player.MaxHealth;
        _currentHealth = _maxHealth;
        _healthBarSlider.maxValue = _maxHealth;
        _healthBarSlider.value = _currentHealth;

        _bossesField.SetActive(false);

        ToggleSoundIcon(DataHandler.Instance.GetSavedMuteValue());
    }

    public void SetCurrentZoneTargets(QuestInfo conditions)
    {
        //_missonConditions = conditions;
        _questCoinCollected = conditions.TargetCoinCount;
        _questEnemyKills = conditions.TargetEnemyKills;
        _questBossEnemyKills = conditions.TargetBossKills;
        _questBigbox = conditions.NeedDestroyBigBox;

        _currentZoneKills = 0;
        _currentZoneCoins = 0;
        _bossKills = 0;

        ShowZoneTargets();
    }       

    private void OnMuteToggled()
    {
        int muteValue = DataHandler.Instance.GetSavedMuteValue() == MaxVolume ? MinVolume : MaxVolume;

        DataHandler.Instance.SaveMuteValue(muteValue);
        AudioListener.volume = muteValue == MaxVolume ? DataHandler.Instance.GetSavedTotalVolume() : MinVolume;        
        ToggleSoundIcon(muteValue);
    }

    private void ToggleSoundIcon(float volume)
    {
        _soundImage.sprite = volume == MaxVolume ? _volumeOn : _volumeOff;
    }

    private void OnChangedMassCooldown(int current, int max)
    {
        float value;
        value = Mathf.Clamp(current, 0, max);
        _massKillButtonCooldown.ChangedMassAttackCooldown(current);
    }

    private void ShowZoneTargets()
    {
        ChangeViewText(_goldText, 0, _questCoinCollected);
        ChangeViewText(_enemyKillsText, 0, _questEnemyKills);
        ChangeViewText(_bigBoxText, 0, _questBigbox ? _questBigboxCount : 0);

        if (_questBossEnemyKills > 0)
        {
            _bossesField.SetActive(true);
            ChangeViewText(_bossKillsText, 0, _questBossEnemyKills);
        }
    }

    private void OnChangedHealth(float health)
    {
        _healthBarSlider.value = health;
        CurrentHealth = health;
    }

    private void OnChangedPlayerCoins(int value)
    {
        _currentZoneCoins += value;

        LevelCoins = value;
        ChangeViewText(_goldText, _currentZoneCoins, _questCoinCollected);
        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnBossKilled(int value)
    {
        _bossKills = value;
        _bossesField.SetActive(true);

        ChangeViewText(_bossKillsText, _bossKills, _questBossEnemyKills);
        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnChangedPlayerKills(int value)
    {
        _currentZoneKills++;

        ChangeViewText(_enemyKillsText, _currentZoneKills, _questEnemyKills);
        LevelKills = value;
        IsCurrentConditionsChanged?.Invoke();
    }

    private void OnDestroyBigBox(int count)
    {
        ChangeViewText(_bigBoxText, count, _questBigboxCount);
        IsBigboxDestroyed = count == _questBigboxCount;
        IsCurrentConditionsChanged?.Invoke();
    }

    private void ChangeViewText(TMP_Text textField, int currentValue, int questValue)
    {
        if (questValue == 0)
            textField.text = currentValue.ToString();
        else
            textField.text = $"{currentValue}/{questValue}";
    }
}