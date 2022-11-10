using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour, IDamageable
{
    [SerializeField] private float _damage;

    public abstract void TakeDamage(float damage);

    protected void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(_damage);
    }
}

public interface IDamageable
{
    void TakeDamage(float damage);
}
