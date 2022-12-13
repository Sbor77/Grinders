using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : Enemy
{
    [SerializeField] private Slider _healthBar;

    public override void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        _takeDamageSFX.Play();
        _currentHealth -= damage;
        _healthBar.value = _currentHealth / _health;
        IsAlive();
    }
}