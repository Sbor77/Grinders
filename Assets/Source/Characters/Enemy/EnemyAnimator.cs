using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Enemy))]
public class EnemyAnimator : MonoBehaviour
{
    private const float SpeedModifier = 4f;
    private const string Speed = "Speed";
    private const string Attack = "Attack";
    private const string AttackSpeed = "AttackSpeed";
    private const string TakeDamage = "TakeDamage";
    private const string Died = "Died";
    private const string Reset = "Reset";
    private const string Dancing = "Win";
    private const string Modifier = "SpeedModifier";

    [SerializeField] private AnimationClip _attackAnimation;
    [SerializeField] private AnimationClip _dancingAnimation;
    [SerializeField] private AnimationClip _walkAnimation;
    [SerializeField] private AudioSource _moveSFX;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Enemy _enemy;
    private float _attackLength;
    private float _attackMultiplier = 1.5f;
    private float _minSpeed = 0.1f;
    private bool _isMoveing;

    private float _newCurrentSpeed;

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
        _animator.SetFloat(Modifier, _agent.speed / SpeedModifier);        
    }

    private void OnDisable()
    {
        _enemy.Dying -= OnDying;
        _enemy.TakedDamage -= OnTakeDamage;        
    }

    private void Start()
    {
        ChangeAttackSpeedModifier(_attackMultiplier);
    }

    private void FixedUpdate()
    {
        //float currentSpeed = _agent.velocity.magnitude / _agent.speed;

        _newCurrentSpeed = _agent.velocity.magnitude / _agent.speed;
        _animator.SetFloat(Speed, _newCurrentSpeed);

        if (_newCurrentSpeed > _minSpeed)
        {
            if (_isMoveing == false)
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

    public void ChangeSpeedModifier(float value)
    {
        _animator.SetFloat(Modifier, _agent.speed / SpeedModifier);
    }

    public void ChangeAttackSpeed(float modifier)
    {
        ChangeAttackSpeedModifier(_attackMultiplier * modifier);
    }

    public float StartAttack()
    {
        _animator.SetTrigger(Attack);
        return _attackLength;
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

    private void ChangeAttackSpeedModifier(float attackSpeed)
    {
        _animator.SetFloat(AttackSpeed, attackSpeed);
        _attackLength = _attackAnimation.length / attackSpeed;
    }

    private void OnDying()
    {
        _animator.SetTrigger(Died);
    }

    private void OnTakeDamage()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(TakeDamage))
            _animator.SetTrigger(TakeDamage);
    }
}