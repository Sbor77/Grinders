using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private Tweener _endlessRotation;    
    private Sequence _collectingSequence;
    private Vector3 _defaultScale;
    private Vector3 _defaultRotation;
    private Vector3 _defaultPosition;
    private Vector3 _rotationAroundY = new Vector3(0, 360, 0);
    private float _defaultHeight;
    private float _rotationTime = 1;
    private float _scaleTime = 0.5f;
    private float _decreaseScaleRatio = 0.11f;
    private float _increaseScaleRatio = 3f;        

    private void Start()
    {
        _defaultScale = transform.localScale;

        _defaultRotation = transform.eulerAngles;

        _defaultHeight = transform.position.y;

        _defaultPosition = transform.position;        
    }

    public void AnimateCollection()
    {
        _endlessRotation.Kill();

        _collectingSequence = DOTween.Sequence();

        transform.eulerAngles = Vector3.zero;

        _collectingSequence.Append(transform.DOMoveY(_defaultHeight + 3, 0.6f).SetEase(Ease.InQuart));        
        _collectingSequence.Append(transform.DOScale(_defaultScale * _increaseScaleRatio, _scaleTime));        
        _collectingSequence.Append(transform.DORotate(_rotationAroundY, 0.3f, RotateMode.FastBeyond360).SetLoops(3).SetEase(Ease.Linear));
        _collectingSequence.Append(transform.DOScale(_defaultScale * _decreaseScaleRatio, _scaleTime));
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
        transform.eulerAngles = Vector3.zero;

        _endlessRotation = transform.DORotate(_rotationAroundY, _rotationTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);        
    }
}