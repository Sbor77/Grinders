using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Joysticks : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected RectTransform _joystickBackground;
    [SerializeField] protected RectTransform _joystickInner;

    protected Vector2 _inputVector;

    private const float Half = .5f;

    public event Action ReleasedTouch;

    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnPointerDown(PointerEventData eventData);
    public abstract void OnPointerUp(PointerEventData eventData);

    public void CalculateJoystickInnerPosition(Vector2 position)
    {
        Vector2 directPosition = position - (Vector2)_joystickBackground.position;
        float halfJoySize = _joystickBackground.rect.width * Half;

        if (directPosition.magnitude > halfJoySize)
            directPosition = directPosition.normalized * halfJoySize;

        _joystickInner.anchoredPosition = directPosition;
    }

    protected Vector2 CalculateInputVector()
    {
        _inputVector = _joystickInner.anchoredPosition / (_joystickBackground.rect.size * Half);
        return _inputVector;
    }

    protected void ReleasedJoystick()
    {
        _joystickInner.anchoredPosition = Vector2.zero;
        _inputVector = Vector2.zero;
        ReleasedTouch?.Invoke();
    }
}
