using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator),typeof(Movement))]
public class Animations : MonoBehaviour
{
    [SerializeField] private float _secondsPerSpin;
    [SerializeField] private ParticleSystem _attackVFX;

    private Animator _animator;
    private Movement _mover;
    private Vector3 _angleRotate = new Vector3(0, -360, 0);

    private const string Speed = "MoveSpeed";
    private const string Attack = "Attack";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Movement>();
        _mover.ChangedStateAttackSpin += OnChangedStateAttackSpin;
        _mover.ChangedMoveSpeed += OnChangedMoveSpeed;
    }

    private void OnDisable()
    {
        _mover.ChangedStateAttackSpin -= OnChangedStateAttackSpin;
        _mover.ChangedMoveSpeed -= OnChangedMoveSpeed;
    }

    private void OnChangedMoveSpeed(float speed)
    {
        _animator.SetFloat(Speed, speed);
    }

    private void OnChangedStateAttackSpin(bool state)
    {
        if (state)
        {
            transform.localRotation = Quaternion.identity;
            transform.DOLocalRotate(_angleRotate, _secondsPerSpin, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        else
        {
            DOTween.Kill(transform);
        }

        _attackVFX.gameObject.SetActive(state);
        _animator.SetBool(Attack, state);
    }
}
