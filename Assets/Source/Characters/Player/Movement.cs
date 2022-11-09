using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController), typeof(CapsuleCollider))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speedInAttack;
    [SerializeField] private float _attackDistance = 15f;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private LayerMask _wallLayerMask;

    private CharacterController _controller;
    private CapsuleCollider _collider;
    private Vector3 _attackDirection = Vector3.forward;
    private Vector3 _moveDirection = Vector3.forward;
    private bool _isMoving = false;

    public event UnityAction<bool> ChangedStateAttackSpin;
    public event UnityAction<float> ChangedMoveSpeed;

    private const float AngleCorrection = -1f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _collider = GetComponent<CapsuleCollider>();
        _collider.enabled = _isMoving;
        _joystick.ChangedDirection += OnChangedDirection;
        _joystick.ReleasedTouch += OnReleasedTouch;
        _joystick.ChangedClickStatus += StartMoveingAtack;
    }

    private void Update()
    {
        if (_isMoving)
        {
            TurnDirection();
            _controller.Move(_speed * Time.deltaTime * transform.forward);
            float currentSpeed = _controller.velocity.magnitude / _speed;
            ChangedMoveSpeed?.Invoke(currentSpeed);
        }
    }

    private void OnDisable()
    {
        _joystick.ChangedDirection -= OnChangedDirection;
        _joystick.ReleasedTouch -= OnReleasedTouch;
        _joystick.ChangedClickStatus -= StartMoveingAtack;

    }

    private void OnChangedDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _moveDirection = new Vector3(direction.x, 0, direction.y);
            _isMoving = true;
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

    private void StartMoveingAtack()
    {
        SetChangeMoving(false);
        _attackDirection = transform.forward;
        _attackDirection.y = 0;
        List<Vector3> movePoints = GetMovePoints();

        if (movePoints != null)
        {
            StartCoroutine(Move(movePoints));
            ChangedStateAttackSpin?.Invoke(true);
        }
        else
            SetChangeMoving(true);
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

            if (movePoints.Count > 50)
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
            point = startPosition + _attackDirection * distance;

        return point;
    }

    private void SetChangeMoving(bool activate)
    {
        _controller.enabled = activate;
        _joystick.enabled = activate;
        _collider.enabled = !activate;
    }

    private IEnumerator Move(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
            yield return StartMove(points[i]);

        ChangedStateAttackSpin?.Invoke(false);
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
}
