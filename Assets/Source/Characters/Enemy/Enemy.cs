using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] private float _health;
    [SerializeField] private float _delayDieHiding = 3f;
    [SerializeField] private float _delayStartStunEffect = 1.5f;
    [SerializeField] private float _stunEffectDuration = 3f;
    [SerializeField] private AudioSource _takeDamageSFX;
    [SerializeField] private ParticleSystem _dieEffect;
    [SerializeField] private ParticleSystem _stunEffect;

    private Mover _mover;
    private float _currentHealth;
    private Vector3 _defaultPosition;
    private bool _isDead = false;

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

    private void IsAlive()
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

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}