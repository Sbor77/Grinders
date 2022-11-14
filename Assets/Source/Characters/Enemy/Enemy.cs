using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] private float _health;

    private Mover _mover;
    private float _currentHealth;

    //public event UnityAction<float> ChangedHealth;
    public event UnityAction Dying;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log(_currentHealth);
        IsAlive();
    }

    private void IsAlive()
    {
        if (_currentHealth <= 0)
            Dying?.Invoke();
    }

    public void Resetting()
    {
        _currentHealth = _health;
        _mover.ResetState();
    }
}