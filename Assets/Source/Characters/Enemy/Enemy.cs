using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] protected AudioSource _takeDamageSFX;

    [SerializeField] private float _health;
    [SerializeField] private float _delayDieHiding = 3f;
    [SerializeField] private float _delayStartStunEffect = 1.5f;
    [SerializeField] private float _stunEffectDuration = 3f;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private ParticleSystem _dieEffect;
    [SerializeField] private ParticleSystem _stunEffect;

    protected bool _isDead = false;
    protected float _currentHealth;
    
    private Mover _mover;

    public float StanEffectDuration => _stunEffectDuration;

    public event Action Dying;
    public event Action TakedDamage;
    public event Action IsDeactivated;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        _currentHealth = _health;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void ChangeSpeed(float value)
    {
        _mover.ChangeSpeed(value);
    }

    public override void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        _takeDamageSFX.Play();
        _currentHealth -= damage;
        TakedDamage?.Invoke();
        IsAlive();
    }

    public void Restore()
    {        
        _currentHealth = _health;
        _mover.ResetState();
        _isDead = false;
    }

    protected void IsAlive()
    {
        if (_currentHealth <= 0)
        {
            Dying?.Invoke();
            _isDead = true;
            _dieEffect.gameObject.SetActive(true);

            DOVirtual.DelayedCall(_delayDieHiding, () =>
            {
                _dieEffect.gameObject.SetActive(false);
                Deactivate();
                IsDeactivated?.Invoke();
            });
        }
        else
        {
            DOVirtual.DelayedCall(_delayStartStunEffect, () => _stunEffect.gameObject.SetActive(true));
            DOVirtual.DelayedCall(_stunEffectDuration, () => _stunEffect.gameObject.SetActive(false));
        }
    }
}