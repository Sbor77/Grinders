using UnityEngine;

public abstract class Characters : MonoBehaviour, IDamageable
{
    [SerializeField] private float _damage;
    [SerializeField] private LayerMask _obstacleMask;

    public abstract void TakeDamage(float damage);

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(_damage);
    }

    public bool CanSee(Transform target)
    {
        Vector3 heading = target.position - transform.position;
        Vector3 direction = heading / heading.magnitude;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, heading.magnitude, _obstacleMask))
            return false;

        return true;
    }

}

public interface IDamageable
{
    void TakeDamage(float damage);
}