using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance;
    [SerializeField] private List<Transform> _patrolPoints;

    private NavMeshAgent _agent;
    private float _bias = 1f;
    private Vector3 _currentPoint;
    private int _currentPointIndex = 0;

    private Vector3 _movePoint;
    private Player _target;
    private float _maxStopedDistance;
    private bool _isAttaking = false;

    private bool _isAcquireTarget => _target != null;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _movePoint = transform.position;

        if (_patrolPoints.Count > 0)
            StartMove(_patrolPoints[0].position);
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
                //Debug.Log(_agent.remainingDistance + " " + );
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _movePoint) <= _stoppingDistance)
                Patrolling();
        }
    }

    public void Init(List<Transform> patrolPoints, float maxStopedDistance)
    {
        _patrolPoints = patrolPoints;
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

    private void Patrolling()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _patrolPoints.Count)
            _currentPointIndex = 0;

        _movePoint = _patrolPoints[_currentPointIndex].position;
        Vector3 bias = new(Random.Range(-_bias, _bias), 0, Random.Range(-_bias, _bias));
        StartMove(_movePoint + bias);
    }

    private void StartMove(Vector3 position)
    {
        _agent.SetDestination(_movePoint);
    }

}
