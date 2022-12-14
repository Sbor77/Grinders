using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Animator),typeof(Movement),typeof(Player))]
public class Animations : MonoBehaviour
{
    private const float SpeedModifier = 4f;
    private const float FinishedSpin = 0.1f;
    private const string Speed = "MoveSpeed";
    private const string Modifier = "SpeedModifier";
    private const string Attack = "Attack";
    private const string AreaAttack = "AreaAttack";
    private const string Died = "Died";
    private const string TakedDamage = "TakeDamage";

    [SerializeField] private float _secondsPerSpin;
    [SerializeField] private AnimationClip _takeDamageClip;
    [SerializeField] private ParticleSystem _attackVFX;
    [SerializeField] private AudioSource _moveSFX;
    [SerializeField] private AudioClip _spinAttackSound;
    [SerializeField] private AudioClip _walkSound;

    private Animator _animator;
    private Movement _mover;
    private Player _player;
    private Vector3 _angleRotate = new Vector3(0, -360, 0);
    private Vector3 _startAngleRotate;
    private bool _isMoving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Movement>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _mover.ChangedState += OnChangedStateAttack;
        _mover.ChangedMoveSpeed += OnChangedMoveSpeed;
        _mover.ChangedBoostSpeed += OnChangedBoostSpeed;
        _player.IsDied += OnDying;
        _player.TakedDamage += OnTakedDamage;
        OnChangedBoostSpeed();
    }

    private void OnChangedBoostSpeed()
    {
        _animator.SetFloat(Modifier, _mover.Speed / SpeedModifier);
    }

    private void OnDisable()
    {
        _mover.ChangedState -= OnChangedStateAttack;
        _mover.ChangedMoveSpeed -= OnChangedMoveSpeed;
        _player.IsDied -= OnDying;
        _player.TakedDamage -= OnTakedDamage;
    }

    private void OnTakedDamage()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(TakedDamage) == false)
        {
            _animator.SetTrigger(TakedDamage);
            float takeDamageDuration = _takeDamageClip.length;
            DOVirtual.DelayedCall(takeDamageDuration, () => _mover.ChangedHitDamage(false));
        }

    }

    private void OnDying() => _animator.SetTrigger(Died);

    private void OnChangedMoveSpeed(float speed)
    {
        _animator.SetFloat(Speed, speed);

        if (speed > 0)
        {
            if (!_isMoving)
            {
                StartAudioClip(_walkSound);
                _isMoving = true;
            }
        }
        else
        {
            _moveSFX.Stop();
            _isMoving = false;
        }
    }

    private void OnChangedStateAttack(State state, bool massAttack)
    {
        if (state == State.Attack && massAttack)
        {
            MassAttack();
            state = State.Move;
        }

        StateAttackSpin(state);
    }

    private void MassAttack()
    {
        _animator.SetTrigger(AreaAttack);
        _animator.ResetTrigger(TakedDamage);
    }

    private void StateAttackSpin(State state)
    {
        if (state == State.Attack)
        {
            _startAngleRotate = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.identity;
            transform.DOLocalRotate(_angleRotate, _secondsPerSpin, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            StartAudioClip(_spinAttackSound);
        }
        else
        {
            DOTween.Kill(transform);
            transform.localRotation = Quaternion.identity;
            transform.DOLocalRotate(_startAngleRotate, FinishedSpin, RotateMode.FastBeyond360).SetLoops(0).SetEase(Ease.Linear);
            _moveSFX.Stop();
        }

        bool convertedState = Convert.ToBoolean((int)state);
        _attackVFX.gameObject.SetActive(convertedState);
        _animator.SetBool(Attack, convertedState);
    }

    private void StartAudioClip(AudioClip audio)
    {
        _moveSFX.clip = audio;
        _moveSFX.Play();
    }
}