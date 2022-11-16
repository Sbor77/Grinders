using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
        
    private WaitForSeconds _waitDelay = new WaitForSeconds (0.5f);

    void Start()
    {
        StopAllEffects();        
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
        StartCoroutine(StartEffectsWithDelay());
    }

    private IEnumerator StartEffectsWithDelay()
    {
        foreach (var effect in _effects)
        {
            effect.Play();

            yield return _waitDelay;
        }        
    }
}
