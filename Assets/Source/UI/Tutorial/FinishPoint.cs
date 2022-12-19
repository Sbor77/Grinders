using UnityEngine;

public class FinishPoint : Point
{
    [SerializeField] private Tutorial _tutorial;

    public override void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _tutorial.OnStayFinishPoint();
            Destroy(gameObject);
        }
    }
}