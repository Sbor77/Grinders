using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private GameObject _doorObject;
    [SerializeField] private Vector3 _openedPosition;
    [SerializeField] private Vector3 _closedPosition;
    [SerializeField] private float _doorMovingTime;


    public void Open()
    {
        _doorObject.transform.DOMove(_openedPosition, _doorMovingTime);
    }

    public void Close()
    {
        _doorObject.transform.DOMove(_closedPosition, _doorMovingTime);
    }
}