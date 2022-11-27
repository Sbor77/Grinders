using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Player : Characters
{
    [SerializeField] private float _health;
    [SerializeField] private AudioSource _takeDamageSFX;
    [SerializeField] private ParticleSystem _weaponEffect;
    [SerializeField] private ParticleSystem _damageEffect;
    [SerializeField] private ParticleSystem _woundEffect;

    private Movement _movement;
    private float _currentHealth;
    private int _coins;
    private State _currentState = State.Move;
    private float _effectDuration = 3f;
    private int _addBoostMaxHealth = 10;
    private bool _isKilledPerAttack;

    public event Action<float> ChangedHealth;
    public event Action<int> ChangedCoin;
    public event Action IsDied;
    public event Action TakedDamage;

    public State CurrentState => _currentState;

    public float MaxHealth => _health;

    public bool IsDead => _currentHealth == 0;


    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    private void Start()
    {
        _currentHealth = _health;
        _weaponEffect.Stop();
        _damageEffect.Stop();
        _woundEffect.Stop();
    }

    private void OnEnable()
    {
        _movement.ChangedState += OnChangedState;
    }

    private void OnDisable()
    {
        _movement.ChangedState -= OnChangedState;
    }

    public void Init(int healthSkillLevel, int speedSkillLevel)
    {
        _movement.Init(speedSkillLevel);
        _health += LoadBoostHealth(healthSkillLevel);
        _currentHealth = _health;
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
            _takeDamageSFX.Play();
            _currentHealth = ChangeHealth(-damage);
            TakedDamage?.Invoke();
            ActivateEffect(_damageEffect, _effectDuration);
            ActivateEffect(_woundEffect, _effectDuration);
            IsAlive();
        }
    }

    private float LoadBoostHealth(int healthLevel)
    {
        return _addBoostMaxHealth * (healthLevel - 1);
    }

    private bool IsValid(int value) => value > 0;

    private void OnChangedState(State state, bool mass)
    {
        _currentState = state;

        if (state == State.Attack)
            _isKilledPerAttack = false;
        else
            _movement.KilledPerAttack(_isKilledPerAttack);
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
            IsDied?.Invoke();
            _movement.OnDied();
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (_currentState == State.Attack && other.TryGetComponent(out IDamageable damageable))
        if (_currentState == State.Attack && other.TryGetComponent(out Characters enemy))
        {
            if (CanAttack(enemy.transform))
            {
                Attack(enemy);

                //if (other.GetComponent<Enemy>())
                ActivateEffect(_weaponEffect, _effectDuration);

                _isKilledPerAttack = true;
            }
        }
    }

    private bool CanAttack(Transform target)
    {
        Vector3 heading = target.position - transform.position;
        Vector3 direction = heading / heading.magnitude;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit))
            return hit.collider.TryGetComponent(out Enemy enemy);

        return false;
    }

    private void ActivateEffect(ParticleSystem effect, float duration)
    {
        effect.Play();
        DOVirtual.DelayedCall(duration, effect.Stop);
    }
}

public enum State
{
    Move,
    Attack
}

public enum AttackType
{
    Low,
    Strong
}