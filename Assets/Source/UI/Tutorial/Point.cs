using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Point : MonoBehaviour
{
    public abstract void OnTriggerStay(Collider other);
}
