using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : Characters
{
    [SerializeField] private float _health;

    private Movement _movement;
    private float _currentHealth;
    private int _coins;
    private State _currentState = State.Move;

    public event Action<float> ChangedHealth;
    public event Action<int> ChangedCoin;
    public event Action Dying;
    public event Action TakedDamage;

    public State CurrentState => _currentState;
    public float MaxHealth => _health;
    public bool IsDead => _currentHealth == 0;

    private const int AddBoostMaxHealth = 10;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Start()
    {
        _currentHealth = _health;

        _health += LoadBoostHealth();
    }

    private void OnEnable()
    {
        _movement.ChangedState += OnChangedState;        
    }

    private void OnDisable()
    {
        _movement.ChangedState -= OnChangedState;
    }

    public void AddMoney(int value)
    {
        _coins += value;
        ChangedCoin?.Invoke(_coins);
    }

    public void Heal(int value)
    {
        if (IsValid(value))
            _currentHealth = ChangeHealth(value);
    }

    public override void TakeDamage(float damage)
    {
        if (_currentState == State.Move && IsValid((int)damage))
        {
            TakedDamage?.Invoke();
            _currentHealth = ChangeHealth(-damage);
            IsAlive();
        }
    }

    private float LoadBoostHealth()
    {
        int boostHealthLevel = DataHandler.Instance.GetSavedHealth();
        return (AddBoostMaxHealth * boostHealthLevel);
    }

    private bool IsValid(int value) => value > 0;

    private void OnChangedState(State state)
    {
        _currentState = state;
    }

    private float ChangeHealth(float value)
    {
        float healthValue = Mathf.Clamp(_currentHealth + value, 0, _health);
        ChangedHealth?.Invoke(healthValue);
        return healthValue;
    }

    private void IsAlive()
    {
        if (_currentHealth == 0)
        {
            Dying?.Invoke();
            _movement.OnDied();
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentState == State.Attack)
        {
            if (other.TryGetComponent(out IDamageable damageable))
                Attack(damageable);
        }
    }
    /*    else
        {
            if (other.TryGetComponent(out Coin coin))
                Debug.Log($"GetCoin: {coin.name}");//coin.GetCoin();
        }
    }*/
}

public enum State
{
    Move,
    Attack
}