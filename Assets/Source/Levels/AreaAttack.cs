using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private ParticleSystem _groundEffect;
    [SerializeField] private ParticleSystem _burstEffect;
    [SerializeField] private AudioSource _mainSound;
    [SerializeField] private AudioSource _chargeSound;
    [SerializeField] private float _radius;
    [SerializeField] private float _chargeDuration;


    public void Apply()
    {
        print("BOOOOOOM !");
    }
}
