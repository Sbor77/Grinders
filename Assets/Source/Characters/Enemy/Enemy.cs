using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Mover))]
public class Enemy : Characters
{
    [SerializeField] private float _health;
    [SerializeField] private SearchZone _searchZone;
    [SerializeField] private float _attackDistance;

    private Mover _mover;
    private float _currentHealth;
    private Player _target;

    //public event UnityAction<float> ChangedHealth;
    public event UnityAction Dying;

    private void Start()
    {
        _mover = GetComponent<Mover>();
        //_searchZone = GetComponentInChildren<SearchZone>();
        _searchZone.ChangedTarget += OnChanngedTarget;
    }

    private void OnDisable()
    {
        _searchZone.ChangedTarget -= OnChanngedTarget;
    }

    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        IsAlive();
    }

    public void Init(List<Transform> patrolPointsList)
    {
        _mover.Init(patrolPointsList, _attackDistance);
    }

    private void OnChanngedTarget(Player target)
    {
        _target = target;
        _mover.SetTarget(_target);
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
