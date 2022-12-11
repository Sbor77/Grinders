using System;
using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickTutorial : Joysticks
{
    [SerializeField] private bool _moveToTouchDownPosition = false;

    [SerializeField] private bool _canMove = false;
    [SerializeField] private bool _canAttack = false;
    [SerializeField] private bool _canMassAttack = false;
    [SerializeField] private Tutorial _tutorial;

    public Vector3 MassAttackButtonPosition => _skillButton.transform.position;
    public Vector3 AttackButtonPosition => _attackButton.transform.position;

    private void OnEnable()
    {
        _skillButton.onClick.AddListener(OnSkillButtonClick);
        _attackButton.onClick.AddListener(OnAttackButtonClick);
    }

    private void Start()
    {
        if (DataHandler.Instance.IsMobile())
        {
            _attackButton.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.Space))
            if (_canMassAttack)
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
            if (Input.GetMouseButton(0) && _canMove)
            {
                CalculateJoystickInnerPosition(eventData.position);
                CalculateInputVector();
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
                    OnAttackButtonClick();
            }
        }
    }

    public void AllowToMove()
    {
        _canMove = true;
    }

    public void AllowToAttack()
    {
        _canAttack = true;
    }

    public void NotAllowedToAttack()
    {
        _canAttack = false;
    }

    public void NotAllowedToMove()
    {
        _canMove = false;
        ReleasedJoystick();
    }

    public void AllowToMassAttack()
    {
        _canMassAttack = true;
    }

    public override void ButtonActivate()
    {
        base.ButtonActivate();
        _tutorial.OnButtonMassAttackActivate();
    }
}