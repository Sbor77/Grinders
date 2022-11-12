using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Enemy))]
public class EnemyAnimator : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private Enemy _enemy;

    private const string Speed = "Speed";
    private const string Attack = "Attack";
    private const string Died = "Died";

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemy.Dying += OnDying;
    }

    private void FixedUpdate()
    {
        _animator.SetFloat(Speed, _agent.velocity.magnitude / _agent.speed);
    }

    public float StartAttack()
    {
        _animator.SetTrigger(Attack);
        return _animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    private void OnDying()
    {
        _animator.SetTrigger(Died);
        this.enabled = false;
    }
}