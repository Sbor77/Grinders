using UnityEngine;
using DG.Tweening;
using System;

public class AreaAttack : MonoBehaviour
{    
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _layer;
    [Space]
    [SerializeField] private ParticleSystem _auraEffect;
    [SerializeField] private ParticleSystem _decalEffect;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private AudioSource _chargeSound;
    [Space]
    [SerializeField] private GameObject _chargeRotator;
    [SerializeField] private ParticleSystem _chargeEffect;
    [Space]
    [SerializeField] private float _mainRadius;
    [SerializeField] private float _mainDamage;
    [SerializeField] private float _stunDamage;
    [SerializeField] private float _chargeDuration;

    public event Action IsActivated;
        
    private float _effectHeiht = 0.05f;
    private float _radiusOffset = 2;
    private Vector3 _rotationAroundY = new Vector3(0, 360, 0);
    private Vector3 _effectPosition;    
    private float _stunRadius;

    private void Start()
    {
        _stunRadius = _mainRadius * _radiusOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _mainRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _stunRadius);
    }

    public void Apply()
    {
        Animate();
        ApplyAreaDamage(_mainRadius, _stunDamage);
        IsActivated?.Invoke();
    }   

    private void ApplyAreaDamage(float radius, float damage)
    {
        Collider[] _targetColliders = Physics.OverlapSphere(transform.position, radius, _layer);

        foreach (var _target in _targetColliders)
        {
            if (_target.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(damage);
        }
    }

    private void Animate()
    {
        float interval = 0.5f;

        _effectPosition = new Vector3 (_player.transform.position.x, _effectHeiht, _player.transform.position.z);
        transform.position = _effectPosition;
        _auraEffect.gameObject.SetActive(true);
        SetChargeEffectPosition();

        Sequence animation = DOTween.Sequence();
        
        animation.AppendCallback(() => 
        {
            ShowChargeEffect();
            _chargeSound.Play();
        });
        animation.Join(_auraEffect.transform.DOScale(_mainRadius, _chargeDuration).SetEase(Ease.Linear));
        animation.AppendInterval(interval);
        animation.AppendCallback(() =>
        {
            ApplyAreaDamage(_mainRadius, _mainDamage);
            _auraEffect.gameObject.SetActive(false);
            _explosionEffect.transform.localScale = Vector3.one * _mainRadius;
            _explosionEffect.gameObject.SetActive(true);
        });
        animation.AppendInterval(_chargeDuration);
        animation.AppendCallback( () =>
        {
            StunTargets();
            _explosionSound.Play();
            _decalEffect.transform.localScale = Vector3.one * _mainRadius;
            _decalEffect.gameObject.SetActive(true);
        });
        animation.AppendInterval(_explosionEffect.main.duration);
        animation.AppendCallback(() => StunTargets());
        animation.AppendInterval(_chargeDuration );
        animation.AppendCallback(() =>
        {
            _explosionEffect.gameObject.SetActive(false);
        });

        animation.AppendInterval(_chargeDuration + _explosionEffect.main.duration + _decalEffect.main.duration);
        animation.AppendCallback(() => _decalEffect.gameObject.SetActive(false));
    }

    private void SetChargeEffectPosition()
    {
        _chargeEffect.transform.localPosition = new Vector3(0, _effectHeiht, - (_mainRadius + _radiusOffset));
    }

    private void ShowChargeEffect()
    {
        float effectLiveTime = 1.5f;

        _chargeEffect.gameObject.SetActive(true);
        _chargeRotator.transform.DORotate(_rotationAroundY, _chargeDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        DOVirtual.DelayedCall(effectLiveTime, () => _chargeEffect.gameObject.SetActive(false));
    }

    private void StunTargets()
    {
        ApplyAreaDamage(_stunRadius, _stunDamage);
    }
}