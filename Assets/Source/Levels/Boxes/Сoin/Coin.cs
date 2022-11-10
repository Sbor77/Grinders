using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private int _value;
    private Sequence _scalingSequence;
    private Sequence _collectingSequence;
    private Vector3 _defaultScale;
    private Vector3 _defaultRotation;
    private float _defaultHeight;
    private float _rotationTime = 1;
    private float _scaleTime = 0.5f;
    private float _decreaseScaleRatio = 0.11f;
    private float _increaseScaleRatio = 3f;    
    
    public int Value => _value;

    private void Start()
    {
        _defaultScale = transform.localScale;

        _defaultRotation = transform.eulerAngles;

        _defaultHeight = transform.position.y;

        RotateOnY();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.GetComponent<Player>())
            ScaleToDestroy();*/
    }

    public void Collect()
    {
        _collectingSequence = DOTween.Sequence();

        _collectingSequence.Append(transform.DOMoveY(_defaultHeight + 3, 0.6f).SetEase(Ease.InQuart));        
        _collectingSequence.Append(transform.DOScale(_defaultScale * _increaseScaleRatio, _scaleTime));
        _collectingSequence.Append(transform.DORotate(new Vector3(_defaultRotation.x, 360, _defaultRotation.z), 0.3f, RotateMode.FastBeyond360)
            .SetLoops(3).SetEase(Ease.Linear));
        _collectingSequence.Append(transform.DOScale(_defaultScale * _decreaseScaleRatio, _scaleTime));
        _collectingSequence.AppendCallback(Deactivate);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);        
    }

    public void Activate()
    {
        gameObject.SetActive(true);        
    }

    private void RotateOnY()
    {
        transform.DORotate(new Vector3(_defaultRotation.x, 360, _defaultRotation.z), _rotationTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void ScaleToDestroy()
    {
        _scalingSequence = DOTween.Sequence();

        _scalingSequence.Append(transform.DOScale(_defaultScale * _increaseScaleRatio, _scaleTime));
        _scalingSequence.Append(transform.DOScale(_defaultScale * _decreaseScaleRatio, _scaleTime));
        _scalingSequence.AppendCallback(() => Destroy(gameObject));
    }


}