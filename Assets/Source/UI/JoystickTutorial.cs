using System;
using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickTutorial : Joysticks
{
    [SerializeField] private bool _moveToTouchDownPosition = false;
    [SerializeField] private Button _skillButton;

    [SerializeField] private bool _canMove = false;
    [SerializeField] private bool _canAttack = false;
    [SerializeField] private bool _canMassAttack = false;

    public event Action<Vector2> ChangedDirection;
    public event Action ChangedClickStatus;
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
        if (Input.GetKeyDown(KeyCode.Space) && _canMassAttack)
            OnSkillButtonClick();
    }

    private void OnDisable()
    {
        ReleasedJoystick();
        _skillButton.onClick.RemoveListener(OnSkillButtonClick);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!DataHandler.Instance.IsMobile())
        {
            if (Input.GetMouseButtonDown(0) && _canMove)
            {
                if (_moveToTouchDownPosition)
                    _joystickBackground.transform.position = eventData.position;

                OnDrag(eventData);
            }

        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!DataHandler.Instance.IsMobile())
        {
            if (Input.GetMouseButtonDown(0) && _canMove)
            {
                CalculateJoystickInnerPosition(eventData.position);
                CalculateInputVector();
                ChangedDirection?.Invoke(_inputVector.normalized);
            }
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!DataHandler.Instance.IsMobile())
        {
            if (Input.GetMouseButtonUp(0) && _canMove)
            {
                ReleasedJoystick();
            }
            else
            {
                if (_canAttack)
                    ChangedClickStatus?.Invoke();
            }
        }
    }

    public void ButtonActivate()
    {
        _skillButton.interactable = true;
    }

    public void AllowToMove()
    {
        _canMove = true;
    }

    public void AllowToAttack()
    {
        _canAttack = true;
    }

    public void AllowToMassAttack()
    {
        _canMassAttack = true;
    }

    private void OnSkillButtonClick()
    {
        if (_canMassAttack)
        {
            if (_skillButton.interactable)
            {
                SkillButtonClick?.Invoke();
                _skillButton.interactable = false;
            }
        }
    }
}