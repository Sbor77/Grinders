using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Animator),typeof(Movement),typeof(Player))]
public class Animations : MonoBehaviour
{
    [SerializeField] private float _secondsPerSpin;
    [SerializeField] private ParticleSystem _attackVFX;

    private Animator _animator;
    private Movement _mover;
    private Player _player;
    private Vector3 _angleRotate = new Vector3(0, -360, 0);
    private Vector3 _startAngleRotate;

    private const float FinishedSpin = 0.1f;
    private const string Speed = "MoveSpeed";
    private const string Attack = "Attack";
    private const string Died = "Died";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Movement>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _mover.ChangedState += OnChangedStateAttackSpin;
        _mover.ChangedMoveSpeed += OnChangedMoveSpeed;
        _player.Dying += OnDying;
    }

    private void OnDisable()
    {
        _mover.ChangedState -= OnChangedStateAttackSpin;
        _mover.ChangedMoveSpeed -= OnChangedMoveSpeed;
        _player.Dying += OnDying;
    }

    private void OnDying() => _animator.SetTrigger(Died);

    private void OnChangedMoveSpeed(float speed) => _animator.SetFloat(Speed, speed);

    private void OnChangedStateAttackSpin(State state)
    {
        if (state == State.Attack)
        {
            _startAngleRotate = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.identity;
            transform.DOLocalRotate(_angleRotate, _secondsPerSpin, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        else
        {
            DOTween.Kill(transform);
            transform.localRotation = Quaternion.identity;
            transform.DOLocalRotate(_startAngleRotate, FinishedSpin, RotateMode.FastBeyond360).SetLoops(0).SetEase(Ease.Linear);
        }

        bool convertedState = Convert.ToBoolean((int)state);
        _attackVFX.gameObject.SetActive(convertedState);
        _animator.SetBool(Attack, convertedState);
    }
}
