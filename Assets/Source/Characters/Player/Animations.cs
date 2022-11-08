using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Movement))]
public class Animations : MonoBehaviour
{
    [SerializeField] private float _secondsPerSpin;
    [SerializeField] private ParticleSystem _attackVFX;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Movement _movement;
    private Vector3 _angleRotate = new Vector3(0, -360, 0);

    private const string Speed = "MoveSpeed";
    private const string Attack = "Attack";
    //private const float Angle = 360f;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<Movement>();
        _movement.ChangedStateAttackSpin += OnChangedStateAttackSpin;
        
    }

    private void OnDisable()
    {
        _movement.ChangedStateAttackSpin -= OnChangedStateAttackSpin;
    }

    private void FixedUpdate()
    {
        float currentSpeed = _agent.velocity.magnitude / _agent.speed;
        _animator.SetFloat(Speed, currentSpeed);
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
