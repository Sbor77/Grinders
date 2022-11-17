using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
        
    private WaitForSeconds _waitDelay = new WaitForSeconds (1f);        
    private int _cycles = 8;

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
