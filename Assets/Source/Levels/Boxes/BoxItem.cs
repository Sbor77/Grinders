using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxItem : MonoBehaviour
{
    private Tweener _endlessRotation;    
    private Sequence _collectingSequence;
    private Vector3 _defaultScale;    
    private Vector3 _defaultPosition;    
    private Vector3 _rotationAroundY = new Vector3(0, 360, 0);
    private float _defaultHeight;    

    private void Start()
    {
        _defaultScale = transform.localScale;        

        _defaultHeight = transform.position.y;

        _defaultPosition = transform.position;                
    }

    public void AnimateCollection()
    {
        int heightOffset = 3;
        float liftTime = 0.6f;
        float scaleTime = 0.5f;
        float rotationTime = 0.3f;
        int rotationLoops = 3;
        float decreaseScaleRatio = 0.11f;
        float increaseScaleRatio = 3f;

        _endlessRotation.Kill();

        _collectingSequence = DOTween.Sequence();

        transform.eulerAngles = Vector3.zero;

        _collectingSequence.Append(transform.DOMoveY(_defaultHeight + heightOffset, liftTime).SetEase(Ease.InQuart));        
        _collectingSequence.Append(transform.DOScale(_defaultScale * increaseScaleRatio, scaleTime));        
        _collectingSequence.Append(transform.DORotate(_rotationAroundY, rotationTime, RotateMode.FastBeyond360).SetLoops(rotationLoops).SetEase(Ease.Linear));
        _collectingSequence.Append(transform.DOScale(_defaultScale * decreaseScaleRatio, scaleTime));
        _collectingSequence.AppendCallback(() =>
        {
            Deactivate();
            transform.localScale = _defaultScale;
            transform.position = _defaultPosition;
        });        
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);        
    }

    public void Activate()
    {
        gameObject.SetActive(true);

        RotateOnY();
    }

    private void RotateOnY()
    {
        float rotationTime = 1;

        transform.eulerAngles = Vector3.zero;

        _endlessRotation = transform.DORotate(_rotationAroundY, rotationTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);        
    }
}