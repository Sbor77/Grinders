using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private float _bias = 1f;
    [SerializeField] private float _maxDelay = 3f;

    private NavMeshAgent _agent;
    private Coroutine GetNewPointWithDelay;
    private WaitForSeconds _delay;
    private Vector3 _zeroPoint;
    private Vector3 _movePoint;
    private Player _target;
    private float _maxStopedDistance;
    private bool _isAttaking = false;

    private bool _isAcquireTarget => _target != null;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _zeroPoint = transform.position;
        Patrol();
    }

    private void FixedUpdate()
    {
        if (_isAcquireTarget)
        {
            if (_isAttaking == false)
            {
                if (Vector3.Distance(_target.transform.position, transform.position) > _maxStopedDistance)
                {
                    _agent.ResetPath();
                    _agent.destination = _target.transform.position;
                }
            }
        }
        else
        {
            if (GetNewPointWithDelay == null)
                if (_agent.remainingDistance <= _stoppingDistance)
                    GetNewPointWithDelay = StartCoroutine(GetNextPoint());
        }
    }

    public void Init(float maxStopedDistance)
    {
        _maxStopedDistance = maxStopedDistance;
    }

    public void SetTarget(Player player)
    {
        _target = player;

        if (_target != null)
            _agent.destination = _target.transform.position;
        else
            _agent.SetDestination(_movePoint);
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
}
