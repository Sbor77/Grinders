using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Joysticks : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private const float Half = .5f;

    [SerializeField] protected RectTransform _joystickBackground;
    [SerializeField] protected RectTransform _joystickInner;
    [SerializeField] protected Button _attackButton;
    [SerializeField] protected Button _skillButton;
    [SerializeField] private Image _mobileJoyPosition;

    public event Action SkillButtonClick;
    public event Action<Vector2> ChangedDirection;
    public event Action ReleasedTouch;
    public event Action AttackButtonClick;

    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnPointerDown(PointerEventData eventData);
    public abstract void OnPointerUp(PointerEventData eventData);

    private void Start()
    {
        if (DataHandler.Instance.IsMobile())
        {
            //_moveToTouchDownPosition = true;
            if (_mobileJoyPosition != null)
                _joystickBackground.transform.position = _mobileJoyPosition.transform.position;

            _joystickBackground.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
            _joystickInner.GetComponent<Image>().color = new Color(1, 1, 1, 0.7f);
        }
        else
        {
            _attackButton.gameObject.SetActive(false);
            _joystickBackground.GetComponent<Image>().color = Color.clear;
            _joystickInner.GetComponent<Image>().color = Color.clear;
        }
    }

    protected void CalculateJoystickInnerPosition(Vector2 position)
    {
        Vector2 directPosition = position - (Vector2)_joystickBackground.position;
        float halfJoySize = _joystickBackground.rect.width * Half;

        if (directPosition.magnitude > halfJoySize)
            directPosition = directPosition.normalized * halfJoySize;

        _joystickInner.anchoredPosition = directPosition;
    }

    protected void CalculateInputVector()
    {
        Vector2 inputVector = _joystickInner.anchoredPosition / (_joystickBackground.rect.size * Half);
        ChangedDirection?.Invoke(inputVector.normalized);
    }

    protected void ReleasedJoystick()
    {
        _joystickInner.anchoredPosition = Vector2.zero;
        //_inputVector = Vector2.zero;
        ReleasedTouch?.Invoke();
    }

    public virtual void ButtonActivate()
    {
        _skillButton.interactable = true;
    }

    protected void OnAttackButtonClick()
    {
        AttackButtonClick?.Invoke();
    }

    protected void OnSkillButtonClick()
    {
        if (_skillButton.interactable)
        {
            SkillButtonClick?.Invoke();
            _skillButton.interactable = false;
        }
    }
}
