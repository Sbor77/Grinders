using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] private float _health;
    [SerializeField] private Collider _searchZoneCollider;
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private float _attackDistance;

    private Mover _mover;
    private float _currentHealth;
    private Vector3 _currentPoint;
    private int _currentPointIndex = 0;
    private bool _isAcquireTarget;
    //private Player _target;

    public event UnityAction<float> ChangedHealth;
    public event UnityAction Dying;

    private void Start()
    {
        _mover = GetComponent<Mover>();
        _mover.InPosition += Patrolling;

        if (_patrolPoints.Count > 0)
            Patrolling();
    }

    private void OnDisable()
    {
        _mover.InPosition -= Patrolling;
    }

    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        IsAlive();
    }

    private void Patrolling()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _patrolPoints.Count)
            _currentPointIndex = 0;

        _currentPoint = _patrolPoints[_currentPointIndex].position;
        _mover.SetMovePosition(_currentPoint);
    }

    private void IsAlive()
    {
        if (_currentHealth == 0)
        {
            Dying?.Invoke();
            this.enabled = false;
        }
    }


}
