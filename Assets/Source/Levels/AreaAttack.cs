using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AreaAttack : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _layer;
    [Space]
    [SerializeField] private ParticleSystem _auraEffect;
    [SerializeField] private ParticleSystem _decalEffect;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private AudioSource _chargeSound;
    [Space]
    [SerializeField] private float _radius;
    [SerializeField] private float _damage;
    [SerializeField] private float _chargeDuration;
        
    private float _effectHeiht = 0.05f;
    private Vector3 _effectPosition;
    

    private void Start()
    {
        _collider.enabled = false;

        _collider.radius = _radius;
    }

    private void DamageTargets()
    {
        Collider [] _targetColliders = Physics.OverlapSphere(transform.position, _collider.radius, _layer);

        foreach (var _target in _targetColliders)
        {
            _target.GetComponent<IDamageable>().TakeDamage(_damage);
        }
    }

    public void Apply()
    {
        print("BOOOOOOM !");

        Animate();

        _collider.enabled = true;

        DamageTargets();        
    }



    private void Animate()
    {
        _effectPosition = new Vector3 (_player.transform.position.x, _effectHeiht, _player.transform.position.z);

        transform.position = _effectPosition;        

        Sequence animation = DOTween.Sequence();

        _auraEffect.gameObject.SetActive(true);

        animation.Append(_auraEffect.transform.DOScale(_radius, _chargeDuration).SetEase(Ease.Linear));
        animation.AppendCallback(_chargeSound.Play);

        animation.AppendInterval(_chargeDuration);
        animation.AppendCallback(() =>
        {
            _auraEffect.gameObject.SetActive(false);
            _explosionEffect.transform.localScale = Vector3.one * _radius;
            _explosionEffect.gameObject.SetActive(true);            
        });

        animation.AppendInterval(0.5f);
        animation.AppendCallback(_explosionSound.Play);

        animation.AppendInterval(_chargeDuration + _explosionEffect.main.duration);
        animation.AppendCallback(() =>
        {
            _explosionEffect.gameObject.SetActive(false);
            _decalEffect.transform.localScale = Vector3.one * _radius;
            _decalEffect.gameObject.SetActive(true);
        });
        animation.AppendInterval(_chargeDuration + _explosionEffect.main.duration + _decalEffect.main.duration);

        animation.AppendCallback(() => _decalEffect.gameObject.SetActive(false));
    }


}