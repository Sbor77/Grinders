using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        //_animator.speed = _agent.speed / 5f;
        _enemy = GetComponent<Enemy>();
        _enemy.Dying += OnDying;
    }

    private void FixedUpdate()
    {
        _animator.SetFloat(Speed, _agent.velocity.magnitude / _agent.speed);
    }

    private void OnDying()
    {
        _animator.SetTrigger(Died);
        this.enabled = false;
    }
}
