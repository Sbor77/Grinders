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

    private Mover _mover;
    private float _currentHealth;
    private Vector3 _defaultPosition;

    //public event UnityAction<float> ChangedHealth;
    public event Action Dying;
    public event Action IsDeactivated;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    public void SetDefaultPosition(Vector3 position)
    {
        _defaultPosition = position;
    }

    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        IsAlive();
    }

    private void IsAlive()
    {
        if (_currentHealth <= 0)
        {
            Dying?.Invoke();

            DOVirtual.DelayedCall(_delayDieHiding, () =>
            {
                Deactivate();
                IsDeactivated?.Invoke();
            });
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Restore()
    {
        transform.position = _defaultPosition;        
        _currentHealth = _health;
        _mover.ResetState();
    }
}