using UnityEngine;

[RequireComponent(typeof(Box))]
public class Subject : MonoBehaviour, IDamageable
{
    private Box _box;

    private void Start()
    {
        _box = GetComponent<Box>();
    }

    public void TakeDamage(float damage)
    {
        _box.Crush();
    }
}