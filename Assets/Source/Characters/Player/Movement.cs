using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _speedInAttack;
    [SerializeField] private float _attackDistance = 15f;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private LayerMask _wallLayerMask;

    private NavMeshAgent _agent;
    private Vector3 _attackDirection = Vector3.zero;
    private Vector3 _startAttackPosition;

    public event UnityAction<bool> ChangedStateAttackSpin;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _joystick.ChangedClickPosition += OnChangedPointPosition;
        _joystick.ChangedDirection += OnChangedAttackDirection;
        _joystick.ReleasedTouch += StartMoveingAtack;
    }

    private void OnDisable()
    {
        _joystick.ChangedClickPosition -= OnChangedPointPosition;
    }

    private IEnumerator Move(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
            yield return StartMove(points[i]);

        ChangedStateAttackSpin?.Invoke(false);
        SetChangeMoving(true);
    }

    private Coroutine StartMove(Vector3 point)
    {
        return StartCoroutine(MoveTo(point));
    }

    private IEnumerator MoveTo(Vector3 pointPosition, float stoppingDistance = 0.6f)
    {
        while (Vector3.Distance(transform.position, pointPosition) >= stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointPosition, _speedInAttack * Time.deltaTime);
            yield return null;
        }
    }

    private void OnChangedAttackDirection(Vector2 direction)
    {
        _attackDirection = new Vector3(direction.x, 0, direction.y);
    }

    private void StartMoveingAtack()
    {
        _agent.ResetPath();
        SetChangeMoving(false);
        ChangedStateAttackSpin?.Invoke(true);
        _startAttackPosition = transform.position;
        _startAttackPosition.y = 0;
        List<Vector3> movePoints = GetMovePoints();
        StartCoroutine(Move(movePoints));
    }

    private void OnChangedPointPosition(Vector3 point)
    {
        _agent.SetDestination(point);
    }

    private List<Vector3> GetMovePoints()
    {
        float distance = 0;
        List<Vector3> movePoints = new List<Vector3>();

        while (distance < _attackDistance)
        {
            Vector3 newPoint = GetNextPoint(_startAttackPosition, _attackDistance - distance);
            movePoints.Add(newPoint);
            distance += Vector3.Distance(_startAttackPosition, newPoint);
            _startAttackPosition = newPoint;
        }

        return movePoints;
    }

    private Vector3 GetNextPoint(Vector3 startPosition, float distance)
    {
        Vector3 point = Vector3.zero;
        RaycastHit hit;

        if (Physics.Raycast(startPosition, _attackDirection, out hit, distance))
        {
            point = hit.point;
            _attackDirection = Vector3.Reflect(_attackDirection, hit.normal);
        }
        else
            point = startPosition + _attackDirection * distance;

        return point;
    }

    private void SetChangeMoving(bool activate)
    {
        _agent.enabled = activate;
        _joystick.enabled = activate;
    }
}
