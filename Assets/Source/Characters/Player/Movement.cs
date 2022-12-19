using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    private const float AngleCorrection = -1f;
    private const float AddBoostMoveSpeed = 0.5f;
    private const int MaxPointCount = 10;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speedInAttack;
    [SerializeField] private int _massAttackDelay = 4;
    [SerializeField] private float _attackDistance = 15f;
    [SerializeField] private Joysticks _joystick;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private AreaAttack _areaAttack;

    public event Action<State,bool> IsStateChanged;
    public event Action<float> IsMoveSpeedChanged;
    public event Action IsBoostSpeedChanged;
    public event Action<int, int> IsMassAttackCooldownChanged;

    private CharacterController _controller;
    private CapsuleCollider _collider;
    private Vector3 _attackDirection = Vector3.forward;
    private Vector3 _moveDirection = Vector3.forward;
    private bool _isMoving;
    private bool _isTakingDamage;
    private bool _isMassAttackApplying;
    private int _currentAttacksCount;
    private float _halfRotation;
    private float _delayMassAttack = 3.5f;

    public float Speed => _speed;    

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        _joystick.ChangedDirection += OnJoystickDirectionChanged;
        _joystick.ReleasedTouch += OnJoystickTouchReleased;
        _joystick.AttackButtonClick += OnJoystickAttackClicked;
        _joystick.SkillButtonClick += OnJoystickMassAttackClicked;
    }

    private void OnDisable()
    {
        _joystick.ChangedDirection -= OnJoystickDirectionChanged;
        _joystick.ReleasedTouch -= OnJoystickTouchReleased;
        _joystick.AttackButtonClick -= OnJoystickAttackClicked;
        _joystick.SkillButtonClick -= OnJoystickMassAttackClicked;
    }

    private void Start()
    {
        _halfRotation = _rotationSpeed / 2f;
    }

    private void Update()
    {
        if (_isMoving)
        {
            TurnDirection();
            Vector3 moveDirection = _speed * transform.forward;

            if (_controller.isGrounded == false)
                moveDirection += Vector3.down; 

            _controller.Move(moveDirection * Time.deltaTime);
            float currentSpeed = _controller.velocity.magnitude / _speed;
            IsMoveSpeedChanged?.Invoke(currentSpeed);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Point point))
            _collider.enabled = true;
    }

    public void OnDied()
    {
        enabled = false;
    }

    public void Init(int speedLevel)
    {
        _collider.enabled = false;
        _speed += LoadBoostSpeed(speedLevel);

        if (speedLevel > 1)
            _rotationSpeed += _halfRotation * speedLevel;

        IsBoostSpeedChanged?.Invoke();
        IsMassAttackCooldownChanged?.Invoke(_currentAttacksCount, _massAttackDelay);
    }

    public void CountCurrentAttacks(bool isKilled)
    {
        if (isKilled)
        {
            _currentAttacksCount++;
            IsMassAttackCooldownChanged?.Invoke(_currentAttacksCount, _massAttackDelay);
        }

        if (_currentAttacksCount >= _massAttackDelay)
            _joystick.ButtonActivate();
    }

    public void ChangeTakingDamageState(bool isTakingDamage = false)
    {
        _isTakingDamage = isTakingDamage;

        if (_currentAttacksCount >= _massAttackDelay)
            _joystick.ButtonActivate();
    }

    private float LoadBoostSpeed(int speedLevel)
    {
        return (speedLevel - 1) * AddBoostMoveSpeed;
    }

    private void OnJoystickDirectionChanged(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _moveDirection = new Vector3(direction.x, 0, direction.y);
            _isMoving = true;
        }
    }

    private void OnJoystickMassAttackClicked()
    {
        if (!_isTakingDamage)
        {
            _isMassAttackApplying = true;
            StartMassAttack();
        }
    }

    private void TurnDirection()
    {
        float angle = Vector3.SignedAngle(_moveDirection, Vector3.forward, Vector3.up);
        float currentAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, angle * AngleCorrection, _rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
    }

    private void OnJoystickTouchReleased()
    {
        _isMoving = false;
        IsMoveSpeedChanged?.Invoke(0f);
    }

    private void OnJoystickAttackClicked()
    {
        StartAttack();
    }

    private void StartMassAttack()
    {
        IsStateChanged?.Invoke(State.Attack, true);
        SetMovingActive(false);
        _currentAttacksCount = 0;
        IsMassAttackCooldownChanged?.Invoke(_currentAttacksCount, _massAttackDelay);
        Invoke(nameof(EndMassAttack), _delayMassAttack);
        _areaAttack.Apply();
    }

    private void EndMassAttack()
    {
        IsStateChanged?.Invoke(State.Move, false);
        SetMovingActive(true);
        _isMassAttackApplying = false;
    }

    private void StartAttack()
    {
        SetMovingActive(false);
        _attackDirection = transform.forward;
        _attackDirection.y = 0;
        List<Vector3> movePoints = GetMovePoints();

        if (movePoints != null)
        {
            StartCoroutine(Move(movePoints));
            IsStateChanged?.Invoke(State.Attack, false);
        }
        else
        { 
            SetMovingActive(true);
        }
    }

    private List<Vector3> GetMovePoints()
    {
        float distance = 0;
        Vector3 _startAttackPosition = transform.position;
        List<Vector3> movePoints = new();

        while (distance < _attackDistance)
        {
            Vector3 newPoint = GetNextPoint(_startAttackPosition, _attackDistance - distance);
            distance += Vector3.Distance(_startAttackPosition, newPoint);
            _startAttackPosition = newPoint;
            movePoints.Add(newPoint);

            if (movePoints.Count > MaxPointCount)
                return null;
        }

        return movePoints;
    }

    private Vector3 GetNextPoint(Vector3 startPosition, float distance)
    {
        Vector3 point;

        if (Physics.Raycast(startPosition, _attackDirection, out RaycastHit hit, distance, _wallLayerMask))
        {
            point = hit.point;
            _attackDirection = Vector3.Reflect(_attackDirection, hit.normal);
        }
        else
        {
            point = startPosition + _attackDirection * distance;
        }

        return point;
    }    

    private void SetMovingActive(bool value)
    {
        _controller.enabled = value;
        _joystick.enabled = value;

        if (_isMassAttackApplying == false)
            _collider.enabled = !value;
    }

    private IEnumerator Move(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
            yield return StartMove(points[i]);

        IsStateChanged?.Invoke(State.Move, false);
        SetMovingActive(true);
    }

    private Coroutine StartMove(Vector3 point)
    {
        return StartCoroutine(MoveTo(point));
    }

    private IEnumerator MoveTo(Vector3 pointPosition, float stoppingDistance = 0.6f)
    {
        while (Vector3.Distance(transform.position, pointPosition) >= stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointPosition, _speedInAttack * Time.deltaTime);
            yield return null;
        }
    }
}