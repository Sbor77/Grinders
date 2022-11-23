using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positioner : MonoBehaviour
{
    [SerializeField] private Transform StartPoint;

    private void Awake()
    {
        transform.position = StartPoint.position;
    }

    private void OnEnable()
    {
        transform.position = StartPoint.position;
    }
}
