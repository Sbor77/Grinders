using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{
    public override void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        _takeDamageSFX.Play();
        _currentHealth -= damage;
        IsAlive();
    }
}