using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : Point
{
    [SerializeField] private Tutorial _tutorial;

    public override void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _tutorial.OnStayFinishPoint();
            Destroy(gameObject);
        }
    }

}
