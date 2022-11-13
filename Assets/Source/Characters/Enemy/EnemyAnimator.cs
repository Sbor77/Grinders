using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Enemy))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private AnimationClip _attackAnimation;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Enemy _enemy;
    private float _attackLenght;
    private float _attackMultiplie = 1.5f;

    private const string Speed = "Speed";
    private const string Attack = "Attack";
    private const string AttackSpeed = "AttackSpeed";
    private const string Died = "Died";

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemy.Dying += OnDying;
        _animator.SetFloat(AttackSpeed, _attackMultiplie);
        _attackLenght = _attackAnimation.length / _attackMultiplie;
    }

    private void FixedUpdate()
    {
        _animator.SetFloat(Speed, _agent.velocity.magnitude / _agent.speed);
    }

    public float StartAttack()
    {
        _animator.SetTrigger(Attack);
        return _attackLenght;
    }

    private void OnDying()
    {
        this.enabled = false;
        _animator.SetTrigger(Died);
    }
}