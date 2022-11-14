using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent), typeof(Enemy), typeof(EnemyAnimator))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private float _bias = 1f;
    [SerializeField] private float _maxDelay = 3f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private SearchZone _searchZone;

    private NavMeshAgent _agent;
    private Enemy _enemy;
    private EnemyAnimator _animator;
    private Coroutine GetNewPointWithDelay;
    private WaitForSeconds _delay;
    private Vector3 _zeroPoint;
    private Vector3 _movePoint;
    private Player _target;
    private bool _isAttaking = false;
    private bool _isAlive = true;

    private bool _isAcquireTarget => _target != null;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        _animator = GetComponent<EnemyAnimator>();
        _zeroPoint = transform.position;
        _searchZone.ChangedTarget += OnChangedTarget;
        _enemy.Dying += OnDying;
        _enemy.ResetState += OnResetState;
        Patrol();
    }

    private void OnDisable()
    {
        _searchZone.ChangedTarget -= OnChangedTarget;
        _enemy.Dying -= OnDying;
        _enemy.ResetState -= OnResetState;
    }

    private void FixedUpdate()
    {
        if (_isAlive == false)
        {
            return;
        }

        if (_isAcquireTarget)
        {
            if (_isAttaking == false)
                _agent.destination = _target.transform.position;
        }
        else
        {
            if (GetNewPointWithDelay == null)
                if (_agent.remainingDistance <= _stoppingDistance)
                    GetNewPointWithDelay = StartCoroutine(GetNextPoint());
        }
    }

    private void OnDying()
    {
        _isAlive = false;
        _agent.ResetPath();
        _searchZone.gameObject.SetActive(false);
    }

    private void Attack()
    {
        _isAttaking = false;

        if (_isAlive == false || _target == null)
            return;

        float distanceToPlayer = Vector3.Distance(_target.transform.position, transform.position);

        if (distanceToPlayer <= _attackDistance)
        {
            if (_target.CurrentState == State.Move)
                _enemy.Attack(_target);
        }
    }

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
        _target = player;

        if (_target != null)
            _agent.destination = _target.transform.position;
        else
            _agent.SetDestination(_movePoint);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !_isAttaking)
        {
            _isAttaking = true;
            _agent.ResetPath();
            float attackDelay = _animator.StartAttack();
            Invoke(nameof(Attack), attackDelay);
        }
    }

    private void OnResetState()
    {
        _isAlive = true;
        _searchZone.gameObject.SetActive(true);
    }
}
