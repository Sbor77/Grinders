using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
        
    private WaitForSeconds _waitDelay;        
    private int _cycles = 1;
    private float _delay = 1f;

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
    }

    private IEnumerator StartEffectsWithDelay(int cycle)
    {
        int i = 0;

        while (i < cycle)
        {
            foreach (var effect in _effects)
            {
                effect.Play();

                i++;                

                yield return _waitDelay;
            }
        }
    }
}
