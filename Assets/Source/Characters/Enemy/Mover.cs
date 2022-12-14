using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(NavMeshAgent), typeof(Enemy), typeof(EnemyAnimator))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private float _bias = 1f;
    [SerializeField] private float _maxDelay = 3f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private SearchZone _searchZone;
    [SerializeField] private Transform _weapon;
    [SerializeField] private LayerMask _wallMask;
    //[SerializeField] [Range(0.1f,1f)] private float _canAttackDelay = 0.5f;

    protected Enemy _enemy;
    protected Player _target;

    private NavMeshAgent _agent;
    private EnemyAnimator _animator;
    private Coroutine GetNewPointWithDelay;
    private Coroutine _canAttackCoroutine;
    private WaitForSeconds _delay;
    private Vector3 _zeroPoint;
    private Vector3 _movePoint;
    private float _baseSpeed;
    private bool _isAttaking = false;
    private bool _canMove = true;
    private bool _isAlive = true;
    private bool _isDancing = false;
    private bool _isStunned;

    //private bool _isCanAttack = false;

    private Tween _onTakeDamageTween;

    protected bool _isAcquireTarget => _target != null;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<EnemyAnimator>();
        _baseSpeed = _agent.speed;
    }

    private void OnEnable()
    {
        _zeroPoint = transform.position;
        _searchZone.ChangedTarget += OnChangedTarget;
        _enemy.Dying += OnDying;
        _enemy.TakedDamage += OnTakeDamage;
        _enemy.IsStunned += OnEnemyStunned;
        _agent.enabled = true;
        Patrol();
    }

    private void OnDisable()
    {
        _searchZone.ChangedTarget -= OnChangedTarget;
        _enemy.Dying -= OnDying;
        _enemy.TakedDamage -= OnTakeDamage;
        _enemy.IsStunned -= OnEnemyStunned;
    }

    private void FixedUpdate()
    {
        if (_isAlive == false)
            return;

        if (_canMove)
        {
            if (_isAcquireTarget)
            {
                if (_isAttaking == false)
                    _agent.destination = _target.transform.position;
            }
            else
            {
                if (GetNewPointWithDelay == null && _isDancing == false)
                    if (_agent.remainingDistance <= _stoppingDistance)
                        GetNewPointWithDelay = StartCoroutine(GetNextPoint());
            }
        }
        else
        {
            _agent.destination = transform.position;
        }
    }

    public void ChangeSpeed(float value)
    {
        _agent.speed = _baseSpeed * value;
        _animator.ChangeSpeedModifier(value);
    }

    public virtual void SetDamage()
    {
        if (_isAlive == false || _target == null || _target.IsDead)
            return;

        float distanceToPlayer = Vector3.Distance(_target.transform.position, transform.position);

        if (distanceToPlayer <= _attackDistance)
        {
            if (_target.CurrentState == State.Move)
                _enemy.Attack(_target);

            if (_target.IsDead)
            {
                float delay = _animator.StartWin();
                _weapon.gameObject.SetActive(false);
                _isDancing = true;
                Invoke(nameof(StopDance), delay);
            }
        }
    }

    public void ResetState()
    {
        _isAlive = true;
        _target = null;
        _searchZone.gameObject.SetActive(true);
        _animator.ResetState();
        _agent.enabled = true;
        _canMove = true;        
    }

    private void OnEnemyStunned(bool isStunned)
    {
        if (isStunned)
            print("ÊÎÍÒÓÆÅÍ");
        else
            print("ÎÒÏÓÑÒÈËÎ");

        _isStunned = isStunned;

        _canMove = !isStunned;
        _isAttaking = isStunned;
    }

    private void OnTakeDamage()
    {        
        /*_onTakeDamageTween.Kill();
        _onTakeDamageTween = null;            
        

        _canMove = false;
        _isAttaking = true;

        _onTakeDamageTween = DOVirtual.DelayedCall(_enemy.StanEffectDuration, () => 
        { 
            _canMove = true;
            _isAttaking = false;            
        });*/
    }

    private void StopDance()
    {
        _isDancing = false;
        _weapon.gameObject.SetActive(true);
    }

    private void OnDying()
    {
        _searchZone.gameObject.SetActive(false);
        _agent.enabled = false;
        _isAlive = false;
    }

    private void Attack()
    {
        if (_isStunned == false)
        {
            _isAttaking = false;
            _canMove = true;
        }
    }

    #region Patrol
    private IEnumerator GetNextPoint()
    {
        float timeDelay = Random.Range(0, _maxDelay);
        _delay = new WaitForSeconds(timeDelay);

        yield return _delay;
        Patrol();
        GetNewPointWithDelay = null;
    }

    private void Patrol()
    {
        _movePoint = GetNewPatrolPoint();
        NavMeshPath path = new NavMeshPath();

        if (_agent.enabled)
            _agent.CalculatePath(_movePoint, path);

        if (path.status == NavMeshPathStatus.PathComplete)
            _agent.destination = _movePoint;
    }

    private Vector3 GetNewPatrolPoint()
    {
        float randomX = Random.Range(-_bias, _bias);
        float randomZ = Random.Range(-_bias, _bias);
        Vector3 newPoint = new(_zeroPoint.x + randomX, _zeroPoint.y, _zeroPoint.z + randomZ);
        return newPoint;
    }

    private void OnChangedTarget(Player player)
    {
        if (_canMove)
        {
            _target = player;

            if (_target != null)
                _agent.destination = _target.transform.position;
            else
                _agent.SetDestination(_movePoint);
        }
    }
    #endregion

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isAttaking)
        {
            if (_canMove && _enemy.CanSee(player.transform))
            {
                if (player.IsDead == false)
                {
                    _isAttaking = true;
                    float attackDelay = _animator.StartAttack();
                    Invoke(nameof(Attack), attackDelay);
                    _canMove = false;
                }
                else
                {
                    if (_isDancing == false)
                        OnChangedTarget(null);
                }
            }
        }
    }
}
