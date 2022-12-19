using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss : Enemy
{
    [SerializeField] private Slider _healthBar;

    public override void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        TakeDamageSFX.Play();
        CurrentHealth -= damage;
        _healthBar.value = CurrentHealth / Health;
        IsAlive();
    }

    public override void Restore()
    {
        base.Restore();
        _healthBar.value = CurrentHealth / Health;
    }
}