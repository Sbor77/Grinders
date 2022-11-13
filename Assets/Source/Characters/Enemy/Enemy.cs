using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] private float _health;

    private float _currentHealth;

    //public event UnityAction<float> ChangedHealth;
    public event UnityAction Dying;

    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log(_currentHealth);
        IsAlive();
    }

    private void IsAlive()
    {
        if (_currentHealth <= 0)
        {
            Dying?.Invoke();
            this.enabled = false;
        }
    }
}