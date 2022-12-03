using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private LayerMask _ceilLayerMask;

    private void Update()
    {
        if (CeilCheck())
            print("Под потолком!!!");
        else
            print("Ушли мимо!!!!!!!!!!!!!!!!!!!!!!!!!!!");


    }

    
    private bool CeilCheck()
    {
        bool result = false;

        Vector3 _direction = Vector3.up;

        float distance = 30;

        if (Physics.Raycast(transform.position, _direction, out RaycastHit hit, distance, _ceilLayerMask))
        {
            var point = hit.point;

            Debug.DrawRay(transform.position, _direction * distance, Color.red);

            result = true; 
        }

        return result;
    }
    
}