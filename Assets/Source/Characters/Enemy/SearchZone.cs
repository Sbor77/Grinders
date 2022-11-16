using System;
using UnityEngine;

public class SearchZone : MonoBehaviour
{
    public event Action<Player> ChangedTarget;    

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.IsDead == false)
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