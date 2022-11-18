using UnityEngine;
using DG.Tweening;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private GameObject _doorObject;    
    [SerializeField] private float _openedHeight;
    [SerializeField] private float _doorMovingTime;

    private float _closedHeight;

    private void Start()
    {
        _closedHeight = transform.localPosition.y;     
    }

    public void Open()
    {
        _doorObject.transform.DOLocalMoveY(_openedHeight, _doorMovingTime);
    }

    public void Close()
    {
        _doorObject.transform.DOLocalMoveY(_closedHeight, _doorMovingTime);
    }
}