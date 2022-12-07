using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private float _radius;
    [SerializeField] private float _damage;
    [SerializeField] private float _stunDamage;
    [SerializeField] private float _chargeDuration;
        
    private float _effectHeiht = 0.05f;
    private float _radiusOffset = 2;
    private Vector3 _rotationAroundY = new Vector3(0, 360, 0);
    private Vector3 _effectPosition;
    
    private float _stunRadius => _radius * 2f;

    public void Apply()
    {
        Animate();

        DamageTargets(_radius, 0);
    }

    private void SetChargeEffectPosition()
    {
        _chargeEffect.transform.localPosition = new Vector3(0, _effectHeiht, - (_radius + _radiusOffset));
    }

    private void ShowChargeEffect()
    {
        float effectLiveTime = 1.5f;

        _chargeEffect.gameObject.SetActive(true);

        _chargeRotator.transform.DORotate(_rotationAroundY, _chargeDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);

        DOVirtual.DelayedCall(effectLiveTime, () => _chargeEffect.gameObject.SetActive(false));
    }

    private void DamageTargets(float radius, float damage)
    {
        ApplyAreaDamage(radius, damage);
    }

    private void StunTargets()
    {
        ApplyAreaDamage(_stunRadius, _stunDamage);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _stunRadius);
    }

    private void Animate()
    {
        _effectPosition = new Vector3 (_player.transform.position.x, _effectHeiht, _player.transform.position.z);

        transform.position = _effectPosition;

        SetChargeEffectPosition();

        Sequence animation = DOTween.Sequence();

        _auraEffect.gameObject.SetActive(true);
        
        animation.AppendCallback(() => 
        {
            ShowChargeEffect();
            _chargeSound.Play();
        });
        animation.Join(_auraEffect.transform.DOScale(_radius, _chargeDuration).SetEase(Ease.Linear));

        animation.AppendInterval(0.5f);
        animation.AppendCallback(() =>
        {
            DamageTargets(_radius, _damage);
            _auraEffect.gameObject.SetActive(false);
            _explosionEffect.transform.localScale = Vector3.one * _radius;
            _explosionEffect.gameObject.SetActive(true);
        });

        animation.AppendInterval(_chargeDuration);
        animation.AppendCallback( () =>
        {
            StunTargets();
            _explosionSound.Play();
            _decalEffect.transform.localScale = Vector3.one * _radius;
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
}