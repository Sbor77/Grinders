using UnityEngine;
using DG.Tweening;

public class StunRotator : MonoBehaviour
{
    [SerializeField] private float _yTurnoverTime;

    private Vector3 _rotationAroundY;

    private void Start()
    {
        _rotationAroundY = new Vector3(90, 360, 0);
        transform.DOLocalRotate(_rotationAroundY, _yTurnoverTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);        
    }
}
