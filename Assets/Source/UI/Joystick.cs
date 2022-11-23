using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickInner;
    [SerializeField] [Range(0.01f, 0.5f)] private float _clickTimeDelta = .3f;
    [SerializeField] private bool _moveToTouchDownPosition = false;

    private Vector2 _inputVector;
    private bool _isTouchDown = false;
    private float _currentDownTime;

    public event Action<Vector2> ChangedDirection;
    public event Action ChangedClickStatus;
    public event Action ReleasedTouch;

    private const float Half = .5f;

    private void Start()
    {
        if (DataHandler.Instance.IsMobile())
            _moveToTouchDownPosition = true;
    }

    private void Update()
    {
        if (_isTouchDown)
        {
            if (_currentDownTime < _clickTimeDelta)
                _currentDownTime += Time.deltaTime;
            else
                _isTouchDown = false;
        }
    }

    private void OnDisable()
    {
        ReleasedJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isTouchDown = true;
        _currentDownTime = 0;

        if (_moveToTouchDownPosition)
            _joystickBackground.transform.position = eventData.position;
        
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        CalculateJoystickInnerPosition(eventData.position);
        CalculateInputVector();
        ChangedDirection?.Invoke(_inputVector.normalized);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ReleasedJoystick();

        if (_isTouchDown)
            ChangedClickStatus?.Invoke();
    }

    private void CalculateJoystickInnerPosition(Vector2 position)
    {
        Vector2 directPosition = position - (Vector2)_joystickBackground.position;
        float halfJoySize = _joystickBackground.rect.width * Half;

        if (directPosition.magnitude > halfJoySize)
            directPosition = directPosition.normalized * halfJoySize;

        _joystickInner.anchoredPosition = directPosition;
    }

    private void CalculateInputVector()
    {
        _inputVector = _joystickInner.anchoredPosition / (_joystickBackground.rect.size * Half);
    }

    private void ReleasedJoystick()
    {
        _joystickInner.anchoredPosition = Vector2.zero;
        _inputVector = Vector2.zero;
        ReleasedTouch?.Invoke();
    }
}
