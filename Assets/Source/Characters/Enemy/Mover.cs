using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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

    protected Enemy Enemy;
    protected Player PlayerTarget;

    private NavMeshAgent _agent;
    private EnemyAnimator _enemyAnimator;
    private Coroutine _getNewPointWithDelay;    
    private WaitForSeconds _delay;
    private Vector3 _zeroPoint;
    private Vector3 _movePoint;
    private float _baseAgentSpeed;
    private bool _isAttaking;
    private bool _canMove = true;
    private bool _isAlive = true;
    private bool _isDancing;
    private bool _isStunned;    

    protected bool IsAcquireTarget => PlayerTarget != null;

    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = GetComponent<EnemyAnimator>();
        _baseAgentSpeed = _agent.speed;
    }

    private void OnEnable()
    {
        _zeroPoint = transform.position;
        _searchZone.IsTargetOutOfSight += OnTargetChanged;
        Enemy.IsDied += OnEnemyDied;        
        Enemy.IsStunned += OnEnemyStunned;
        _agent.enabled = true;
        Patrol();
    }

    private void OnDisable()
    {
        _searchZone.IsTargetOutOfSight -= OnTargetChanged;
        Enemy.IsDied -= OnEnemyDied;        
        Enemy.IsStunned -= OnEnemyStunned;
    }

    private void FixedUpdate()
    {
        if (_isAlive == false)
            return;

        if (_canMove)
        {
            if (IsAcquireTarget)
            {
                if (_isAttaking == false)
                    _agent.destination = PlayerTarget.transform.position;
            }
            else
            {
                if (_getNewPointWithDelay == null && _isDancing == false)
                    if (_agent.remainingDistance <= _stoppingDistance)
                        _getNewPointWithDelay = StartCoroutine(GetNextPoint());
            }
        }
        else
        {
            _agent.destination = transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isAttaking)
        {
            if (_canMove && Enemy.CanSee(player.transform))
            {
                if (player.IsDead == false)
                {
                    _isAttaking = true;
                    _enemyAnimator.StartAttack();                    
                    _canMove = false;
                }
                else
                {
                    if (_isDancing == false)
                        OnTargetChanged(null);
                }
            }
        }
    }

    public void ChangeSpeed(float value)
    {
        _agent.speed = _baseAgentSpeed * value;
        _enemyAnimator.ChangeSpeedModifier();
    }

    public virtual void SetDamage()
    {
        if (_isAlive == false || PlayerTarget == null || PlayerTarget.IsDead)
            return;

        float distanceToPlayer = Vector3.Distance(PlayerTarget.transform.position, transform.position);

        if (distanceToPlayer <= _attackDistance)
        {
            if (PlayerTarget.CurrentState == State.Move)
                Enemy.Attack(PlayerTarget);

            if (PlayerTarget.IsDead)
            {
                float delay = _enemyAnimator.StartWin();
                _weapon.gameObject.SetActive(false);
                _isDancing = true;
                Invoke(nameof(StopDance), delay);
            }
        }
    }

    public void ResetState()
    {
        _isAlive = true;
        PlayerTarget = null;
        _searchZone.gameObject.SetActive(true);
        _enemyAnimator.ResetState();
        _agent.enabled = true;
        _canMove = true;
    }

    private void OnEnemyStunned(bool isStunned)
    {
        _canMove = !isStunned;
        _isStunned = isStunned;
        _isAttaking = isStunned;
    }

    private void StopDance()
    {
        _isDancing = false;
        _weapon.gameObject.SetActive(true);
    }

    private void OnEnemyDied()
    {
        _searchZone.gameObject.SetActive(false);
        _agent.enabled = false;
        _isAlive = false;
    }

    #region Patrol
    private void Patrol()
    {
        _movePoint = GetNewPatrolPoint();
        NavMeshPath path = new NavMeshPath();

        if (_agent.enabled)
            _agent.CalculatePath(_movePoint, path);

        if (path.status == NavMeshPathStatus.PathComplete)
            _agent.destination = _movePoint;
    }

    private IEnumerator GetNextPoint()
    {
        float timeDelay = Random.Range(0, _maxDelay);
        _delay = new WaitForSeconds(timeDelay);

        yield return _delay;
        Patrol();
        _getNewPointWithDelay = null;
    }

    private Vector3 GetNewPatrolPoint()
    {
        float randomX = Random.Range(-_bias, _bias);
        float randomZ = Random.Range(-_bias, _bias);
        Vector3 newPoint = new(_zeroPoint.x + randomX, _zeroPoint.y, _zeroPoint.z + randomZ);
        return newPoint;
    }

    private void OnTargetChanged(Player player)
    {
        if (_canMove)
        {
            PlayerTarget = player;

            if (PlayerTarget != null)
                _agent.destination = PlayerTarget.transform.position;
            else
                _agent.SetDestination(_movePoint);
        }
    }
    #endregion
}