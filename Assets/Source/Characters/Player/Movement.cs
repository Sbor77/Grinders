using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    private const float AngleCorrection = -1f;
    private const float AddBoostMoveSpeed = 0.5f;
    private const int _maxPointCount = 10;

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speedInAttack;
    [SerializeField] private int _beforeMassAttack = 2;
    [SerializeField] private float _attackDistance = 15f;
    [SerializeField] private Joysticks _joystick;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private AreaAttack _areaAttack;

    private CharacterController _controller;
    private CapsuleCollider _collider;
    private Vector3 _attackDirection = Vector3.forward;
    private Vector3 _moveDirection = Vector3.forward;
    private bool _isMoving = false;
    private bool _isTakingDamage = false;
    private bool _massAttack = false;
    private int _currentAttacksCount;
    private float _delayMassAttack = 3.5f;

    public float Speed => _speed;

    private float _halfRotation => _rotationSpeed / 2f;

    public event Action<State,bool> ChangedState;
    public event Action<float> ChangedMoveSpeed;
    public event Action ChangedBoostSpeed;
    public event Action<int, int> ChangedMassAttackCooldown;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        _joystick.ChangedDirection += OnChangedDirection;
        _joystick.ReleasedTouch += OnReleasedTouch;
        _joystick.AttackButtonClick += StartAttack;
        _joystick.SkillButtonClick += UseMassAttack;
    }

    private void OnDisable()
    {
        _joystick.ChangedDirection -= OnChangedDirection;
        _joystick.ReleasedTouch -= OnReleasedTouch;
        _joystick.AttackButtonClick -= StartAttack;
        _joystick.SkillButtonClick -= UseMassAttack;
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
            ChangedMoveSpeed?.Invoke(currentSpeed);
        }
    }

    public void OnDied()
    {
        this.enabled = false;
    }

    public void Init(int speedLevel)
    {
        _collider.enabled = false;
        _speed += LoadBoostSpeed(speedLevel);

        if (speedLevel > 1)
            _rotationSpeed += _halfRotation * speedLevel;

        ChangedBoostSpeed?.Invoke();
        ChangedMassAttackCooldown?.Invoke(_currentAttacksCount, _beforeMassAttack);
    }

    public void KilledPerAttack(bool isKilled)
    {
        if (isKilled)
        {
            _currentAttacksCount++;
            ChangedMassAttackCooldown?.Invoke(_currentAttacksCount, _beforeMassAttack);
        }

        if (_currentAttacksCount >= _beforeMassAttack)
            _joystick.ButtonActivate();
    }

    public void ChangedHitDamage(bool isTakingDamage = false)
    {
        _isTakingDamage = isTakingDamage;

        if (_currentAttacksCount >= _beforeMassAttack)
            _joystick.ButtonActivate();
    }

    private float LoadBoostSpeed(int speedLevel)
    {
        return (speedLevel - 1) * AddBoostMoveSpeed;
    }

    private void OnChangedDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _moveDirection = new Vector3(direction.x, 0, direction.y);
            _isMoving = true;
        }
    }

    private void UseMassAttack()
    {
        if (!_isTakingDamage)
        {
            _massAttack = true;
            StartMassAttack();
        }
    }

    private void TurnDirection()
    {
        float angle = Vector3.SignedAngle(_moveDirection, Vector3.forward, Vector3.up);
        float currentAngle = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, angle * AngleCorrection, _rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);
    }

    private void OnReleasedTouch()
    {
        _isMoving = false;
        ChangedMoveSpeed?.Invoke(0f);
    }

    private void StartAttack()
    {
       StartMoveingAttack();
    }

    private void StartMassAttack()
    {
        ChangedState?.Invoke(State.Attack, true);
        SetChangeMoving(false);
        _currentAttacksCount = 0;
        ChangedMassAttackCooldown?.Invoke(_currentAttacksCount, _beforeMassAttack);
        Invoke(nameof(EndMassAttack), _delayMassAttack);
        _areaAttack.Apply();
    }

    private void EndMassAttack()
    {
        ChangedState?.Invoke(State.Move, false);
        SetChangeMoving(true);
        _massAttack = false;
    }

    private void StartMoveingAttack()
    {
        SetChangeMoving(false);
        _attackDirection = transform.forward;
        _attackDirection.y = 0;
        List<Vector3> movePoints = GetMovePoints();

        if (movePoints != null)
        {
            StartCoroutine(Move(movePoints));
            ChangedState?.Invoke(State.Attack, false);
        }
        else
        { 
            SetChangeMoving(true);
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

            if (movePoints.Count > _maxPointCount)
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

    private void SetChangeMoving(bool activate)
    {
        _controller.enabled = activate;
        _joystick.enabled = activate;

        if (_massAttack == false)
            _collider.enabled = !activate;
    }

    private IEnumerator Move(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
            yield return StartMove(points[i]);

        ChangedState?.Invoke(State.Move, false);
        SetChangeMoving(true);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Point point))
            _collider.enabled = true;
    }
}