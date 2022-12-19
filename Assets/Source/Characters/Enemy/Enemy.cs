using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] protected AudioSource TakeDamageSFX;
    [SerializeField] protected float Health;
    [SerializeField] private float _delayDieHiding = 3f;
    [SerializeField] private float _delayStartStunEffect = 1.5f;
    [SerializeField] private float _stunEffectDuration = 3f;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private ParticleSystem _dieEffect;
    [SerializeField] private ParticleSystem _stunEffect;

    public event Action IsDied;
    public event Action IsTakenDamage;
    public event Action IsDeactivated;
    public event Action <bool>IsStunned;

    protected bool IsDead = false;
    protected float CurrentHealth;
    
    private Mover _mover;    

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        CurrentHealth = Health;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void ChangeMoverSpeed(float value)
    {
        _mover.ChangeSpeed(value);
    }

    public override void TakeDamage(float value)
    {
        if (IsDead)
            return;

        TakeDamageSFX.Play();
        CurrentHealth -= value;
        IsTakenDamage?.Invoke();
        IsAlive();
    }

    public virtual void Restore()
    {        
        CurrentHealth = Health;
        _mover.ResetState();
        IsDead = false;
    }

    protected void IsAlive()
    {
        if (CurrentHealth <= 0)
        {
            IsDied?.Invoke();
            IsDead = true;
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
            DOVirtual.DelayedCall(_delayStartStunEffect, () => 
            {
                _stunEffect.gameObject.SetActive(true);
                IsStunned?.Invoke(true);
            });
            DOVirtual.DelayedCall(_stunEffectDuration, () =>
            {
                _stunEffect.gameObject.SetActive(false);
                IsStunned?.Invoke(false);
            });
        }
    }
}