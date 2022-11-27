using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AreaAttack : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private ParticleSystem _auraEffect;
    [SerializeField] private ParticleSystem _decalEffect;
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private AudioSource _chargeSound;
    [SerializeField] private float _radius;
    [SerializeField] private float _chargeDuration;
        
    private float _effectHeiht = 0.05f;
    private Vector3 _effectPosition;


    public void Apply()
    {
        print("BOOOOOOM !");

        Animate();
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