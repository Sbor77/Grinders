using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
    [SerializeField] private AudioSource _effectSFX;
        
    private WaitForSeconds _waitDelay;
    private int _cycles = 2;
    private float _delay = 0.92f;

    public float Duration { get; private set; }

    private void Start()
    {
        StopAllEffects();
        _waitDelay = new WaitForSeconds(_delay);
        Duration = _effects.Count * _cycles * _delay;
    }

    private void StopAllEffects()
    {
        foreach (var effect in _effects)
        {
            effect.Stop();
        }
    }

    public void PlayAllEffects()
    {
        StartCoroutine(StartEffectsWithDelay(_cycles));
        _effectSFX.Play();
    }

    private IEnumerator StartEffectsWithDelay(int cycle)
    {
        int i = 0;

        while (i < cycle)
        {
            foreach (var effect in _effects)
            {
                effect.Play();
                yield return _waitDelay;
            }

            i++;
        }
    }
}