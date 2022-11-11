using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SearchZone : MonoBehaviour
{
    public event UnityAction<Player> ChangedTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log($"� ���� ���������: {player.name}");
            ChangedTarget?.Invoke(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log($"����� �� ���� ���������: {player.name}");
            ChangedTarget?.Invoke(null);
        }
    }
}
