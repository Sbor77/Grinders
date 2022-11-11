using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance;

    private NavMeshAgent _agent;
    private Vector3 _movePoint;

    public event UnityAction InPosition;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _movePoint = transform.position;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _movePoint) <= _stoppingDistance)
            InPosition?.Invoke();
    }

    public void SetMovePosition(Vector3 position)
    {
        _movePoint = position;
        _agent.SetDestination(_movePoint);
    }
}
