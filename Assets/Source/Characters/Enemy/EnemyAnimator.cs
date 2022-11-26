using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Enemy))]
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private AnimationClip _attackAnimation;
    [SerializeField] private AnimationClip _dancingAnimation;
    [SerializeField] private AudioSource _moveSFX;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Enemy _enemy;
    private float _attackLenght;
    private float _attackMultiplie = 1.5f;
    private bool _isMoveing;
    private const string Speed = "Speed";
    private const string Attack = "Attack";
    private const string AttackSpeed = "AttackSpeed";
    private const string TakeDamage = "TakeDamage";
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

        _enemy.TakedDamage += OnTakeDamage;
    }

    private void Start()
    {
        _animator.SetFloat(AttackSpeed, _attackMultiplie);

        _attackLenght = _attackAnimation.length / _attackMultiplie;
    }

    private void OnDisable()
    {
        _enemy.Dying -= OnDying;

        _enemy.TakedDamage -= OnTakeDamage;
    }

    private void FixedUpdate()
    {
        float currentSpeed = _agent.velocity.magnitude;

        _animator.SetFloat(Speed, currentSpeed / _agent.speed);

        if (currentSpeed / _agent.speed > 0.1f)
        {
            if (!_isMoveing)
            {
                _isMoveing = true;

                _moveSFX.Play();
            }
        }
        else
        {
            _isMoveing = false;

            _moveSFX.Stop();
        }
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
        
        _animator.ResetTrigger(Reset);
    }

    private void OnDying()
    {
        _animator.SetTrigger(Died);
    }

    private void OnTakeDamage()
    {
        _animator.SetTrigger(TakeDamage);
    }
}