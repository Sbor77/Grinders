using UnityEngine;

public class MovePoint : Point
{
    [SerializeField]private Tutorial _tutorial;

    public override void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _tutorial.OnStayMovePoint();
            Destroy(gameObject);         
        }
    }
}