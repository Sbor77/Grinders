using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : Joysticks
{
    [SerializeField] [Range(0.01f, 0.5f)] private float _clickTimeDelta = .1f;
    [SerializeField] private bool _moveToTouchDownPosition = false;

    private bool _isTouchDown = false;
    private float _currentDownTime;

    private void OnEnable()
    {
        _skillButton.onClick.AddListener(OnSkillButtonClick);
        _attackButton.onClick.AddListener(OnAttackButtonClick);
    }

    private void Start()
    {
        if (DataHandler.Instance.IsMobile())
        {
            _moveToTouchDownPosition = true;
            _attackButton.gameObject.SetActive(false);
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
        _attackButton.onClick.RemoveListener(OnAttackButtonClick);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!DataHandler.Instance.IsMobile())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isTouchDown = true;
                _currentDownTime = 0;

                if (_moveToTouchDownPosition)
                    _joystickBackground.transform.position = eventData.position;

                OnDrag(eventData);
            }
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            CalculateJoystickInnerPosition(eventData.position);
            CalculateInputVector();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!DataHandler.Instance.IsMobile())
        {
            if (Input.GetMouseButtonUp(0))
            {
                ReleasedJoystick();

                if (_isTouchDown)
                    OnAttackButtonClick();
            }
            else
            {
                OnAttackButtonClick();
            }
        }
    }
}