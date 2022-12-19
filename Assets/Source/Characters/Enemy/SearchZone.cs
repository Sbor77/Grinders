using System;
using System.Collections;
using UnityEngine;

public class SearchZone : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public event Action<Player> IsTargetOutOfSight;

    private WaitForSeconds _delay = new WaitForSeconds(.5f);
    private Coroutine _canSeeJob;

    private void OnDisable()
    {
        StopCoroutine();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.IsDead == false)
            {
                if (_enemy.CanSee(player.transform))
                    IsTargetOutOfSight?.Invoke(player);
                else
                    _canSeeJob = StartCoroutine(CanSeeTarget(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            IsTargetOutOfSight?.Invoke(null);            
            StopCoroutine();
        }
    }
 
    private IEnumerator CanSeeTarget(Player target)
    {
        while (_enemy.CanSee(target.transform) == false)
        {
            yield return _delay;            
        }
        
        IsTargetOutOfSight?.Invoke(target);        
        StopCoroutine();
    }

    private void StopCoroutine()
    {
        if (_canSeeJob != null)
        {
            StopCoroutine(_canSeeJob);
            _canSeeJob = null;
        }
    }
}