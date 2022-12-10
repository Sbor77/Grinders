using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : Point
{
    [SerializeField]private Tutorial _tutorial;

    public override void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            _tutorial.OnStayMovePoint();
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }
}
