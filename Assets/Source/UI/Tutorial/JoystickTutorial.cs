using UnityEngine;
using UnityEngine.EventSystems;

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
        _attackButton.onClick.AddListener(OnAttackClick);
    }

    private void OnDisable()
    {
        ReleasedJoystick();
        _skillButton.onClick.RemoveListener(OnSkillButtonClick);
        _attackButton.onClick.RemoveListener(OnAttackClick);
    }   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (_canMassAttack)
                OnSkillButtonClick();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0) && _canMove)
        {
            if (_moveToTouchDownPosition)
                _joystickBackground.transform.position = eventData.position;

            OnDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && _canMove)
        {
            CalculateJoystickInnerPosition(eventData.position);
            CalculateInputVector();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (DataHandler.Instance.IsMobile())
        {
            ReleasedJoystick();
        }
        else
        {
            if (Input.GetMouseButtonUp(0) && _canMove)
            {
                ReleasedJoystick();
            }
            else
            {
                if (_canAttack && DataHandler.Instance.IsMobile() == false)
                    OnAttackButtonClick();
            }
        }
    }

    public void EnableMoving()
    {
        _canMove = true;
    }

    public void EnableAttacking()
    {
        _canAttack = true;
    }

    public void DisableAttacking()
    {
        _canAttack = false;
    }

    public void DisableMoving()
    {
        _canMove = false;
        ReleasedJoystick();
    }

    public void EnableMassAttacking()
    {
        _canMassAttack = true;
    }

    public override void ButtonActivate()
    {
        base.ButtonActivate();
        _tutorial.OnButtonMassAttackActivate();
    }

    private void OnAttackClick()
    {
        if (_canAttack)
            OnAttackButtonClick();
    }
}