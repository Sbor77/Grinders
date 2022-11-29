using UnityEngine;
using DG.Tweening;

public class StunRotator : MonoBehaviour
{
    [SerializeField] private float _yTurnoverTime;

    private Vector3 _rotationAroundY;

    private void Start()
    {
        _rotationAroundY = new Vector3(90, 360, 0);

        //transform.eulerAngles = Vector3.zero;

        transform.DOLocalRotate(_rotationAroundY, _yTurnoverTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);        


        
    }

    /*private void Update()
    {
        print("Ёйлеровы углы локальные" + transform.localEulerAngles);

        print("Ёйлеровы углы мировые" + transform.eulerAngles);
    }*/
}
