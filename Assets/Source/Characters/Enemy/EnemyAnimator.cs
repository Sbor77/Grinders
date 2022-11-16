using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Enemy))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private AnimationClip _attackAnimation;
    [SerializeField] private AnimationClip _dancingAnimation;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Enemy _enemy;
    private float _attackLenght;
    private float _attackMultiplie = 1.5f;

    private const string Speed = "Speed";
    private const string Attack = "Attack";
    private const string AttackSpeed = "AttackSpeed";
    private const string Died = "Died";
    private const string Reset = "Reset";
    private const string Dancing = "Win";

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _enemy.Dying += OnDying;
        _animator.SetFloat(AttackSpeed, _attackMultiplie);
        _attackLenght = _attackAnimation.length / _attackMultiplie;
    }

    private void OnDisable()
    {
        _enemy.Dying -= OnDying;
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

    public float StartWin()
    {
        _animator.SetTrigger(Dancing);
        return _dancingAnimation.length;
    }

    public void ResetState()
    {
        _animator.SetTrigger(Reset);
    }

    private void OnDying()
    {
        _animator.SetTrigger(Died);
    }
}