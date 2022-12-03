using System;
using System.Collections;
using UnityEngine;

public class SearchZone : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private WaitForSeconds _delay = new WaitForSeconds(.5f);
    private Coroutine _waitCanSee = null;
    private bool _isAcquireTarget;

    public event Action<Player> ChangedTarget;

    private void OnEnable()
    {
        _isAcquireTarget = false;
    }

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
                    ChangedTarget?.Invoke(player);
                else
                    _waitCanSee = StartCoroutine(CanSeeTarget(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            ChangedTarget?.Invoke(null);
            _isAcquireTarget = false;
            StopCoroutine();
        }
    }

    //public void OnDie()
    //{
    //    StopCoroutine();
    //}

    private IEnumerator CanSeeTarget(Player target)
    {
        while (_enemy.CanSee(target.transform) == false)
        {
            yield return _delay;
            //print($"не вижу плеера");
        }
        
        ChangedTarget?.Invoke(target);
        _isAcquireTarget = true;
        //print($"Вижу плеера");
        StopCoroutine();
    }

    private void StopCoroutine()
    {
        if (_waitCanSee != null)
        {
            StopCoroutine(_waitCanSee);
            _waitCanSee = null;
        }
    }
}