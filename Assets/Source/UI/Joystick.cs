using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _joystickBackground;
    [SerializeField] private RectTransform _joystickInner;
    [SerializeField] [Range(0.01f, 0.5f)] private float _clickTimeDelta = .3f;
    [SerializeField] private bool _moveToTouchDownPosition = false;
    [SerializeField] private Button _skillButton;

    private Vector2 _inputVector;
    private bool _isTouchDown = false;
    private float _currentDownTime;

    public event Action<Vector2> ChangedDirection;
    public event Action ChangedClickStatus;
    public event Action ReleasedTouch;
    public event Action SkillButtonClick;

    private const float Half = .5f;

    private void OnEnable()
    {
        _skillButton.onClick.AddListener(OnSkillButtonClick);
    }

    private void Start()
    {
        if (DataHandler.Instance.IsMobile())
        {
            _moveToTouchDownPosition = true;
            _joystickBackground.GetComponent<Image>().color = Color.white;
            _joystickInner.GetComponent<Image>().color = Color.white;
        }
        else
        {
            _joystickBackground.GetComponent<Image>().color = Color.clear;
            _joystickInner.GetComponent<Image>().color = Color.clear;
        }
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

        if (Input.GetKeyDown(KeyCode.Space))
            OnSkillButtonClick();
    }

    private void OnDisable()
    {
        ReleasedJoystick();
        _skillButton.onClick.RemoveListener(OnSkillButtonClick);
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

    public void ButtonActivate()
    {
        _skillButton.interactable = true;
    }

    private void OnSkillButtonClick()
    {
        if (_skillButton.interactable)
        {
            SkillButtonClick?.Invoke();
            _skillButton.interactable = false;
        }
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
