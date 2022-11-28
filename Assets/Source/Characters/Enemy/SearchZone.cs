using System;
using UnityEngine;

public class SearchZone : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public event Action<Player> ChangedTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.IsDead == false && _enemy.CanSee(player.transform))
                ChangedTarget?.Invoke(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            ChangedTarget?.Invoke(null);
        }
    }
}