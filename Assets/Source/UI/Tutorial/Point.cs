using UnityEngine;

public abstract class Point : MonoBehaviour
{
    public abstract void OnTriggerStay(Collider other);
}