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
    [SerializeField] private TMP_Text _KillsText;
    [SerializeField] private MassAttackView _massKillButtonCooldown;
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _volumeOn;
    [SerializeField] private Sprite _volumeOff;

    public event Action IsCurrentConditionsChanged;

    private const int MaxVolume = 1;
    private const int MinVolume = 0;
    private Movement _movement;
    private Button _soundButton;
    private QuestInfo _missonConditions;
    private int _questCoinCollected;
    private int _questEnemyKills;
    private int _questBigboxCount = 1;
    private bool _questBigbox;
    private float _maxHealth;
    private float _currentHealth;

    public QuestInfo MissionConditions => _missonConditions;
    public float CurrentHealth { get; private set; }
    public int CurrentKills { get; private set; }
    public int CurrentCoins { get; private set; }
    public bool IsBigboxDestroyed { get; private set; }

    private void Awake()
    {
        _movement = _player.GetComponent<Movement>();
        _soundButton = _soundImage.GetComponent<Button>();
    }

    private void OnEnable()
    {
        _enemySpawner.IsPLayerKillsIncreased += OnChangedPlayerKills;
        _player.ChangedHealth += OnChangedHealth;
        _player.ChangedCoin += OnChangedPlayerCoins;
        _boxSpawner.IsBigBoxCollected += OnDestroyBigBox;
        _soundButton.onClick.AddListener(OnMuteToggled);
        _movement.ChangedMassAttackCooldown += OnChangedMassCooldown;
    }

    private void OnDisable()
    {
        _enemySpawner.IsPLayerKillsIncreased -= OnChangedPlayerKills;
        _player.ChangedHealth -= OnChangedHealth;
        _player.ChangedCoin -= OnChangedPlayerCoins;
        _boxSpawner.IsBigBoxCollected -= OnDestroyBigBox;
        _soundButton.onClick.RemoveListener(OnMuteToggled);
        _movement.ChangedMassAttackCooldown -= OnChangedMassCooldown;
    }

    void Start()
    {
        _maxHealth = _player.MaxHealth;
        _currentHealth = _maxHealth;
        _healthBarSlider.maxValue = _maxHealth;
        _healthBarSlider.value = _currentHealth;

        ToggleSoundIcon(DataHandler.Instance.GetSavedMuteValue());
    }

    public void SetQuestCollected(QuestInfo conditions)
    {
        _missonConditions = conditions;
        _questCoinCollected = conditions.NeedCoinCollected;
        _questBigbox = conditions.NeedDestroyBigBox;
        _questEnemyKills = conditions.NeedEnemyKilled;

        SetStartConditionsText();
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

    private void SetStartConditionsText()
    {
        ChangeViewText(_goldText, 0, _questCoinCollected);
        ChangeViewText(_KillsText, 0, _questEnemyKills);
        ChangeViewText(_bigBoxText, 0, _questBigbox ? _questBigboxCount : 0);
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