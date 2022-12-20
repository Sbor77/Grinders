using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private const float Damage = 1000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(Damage);
        }
    }
}