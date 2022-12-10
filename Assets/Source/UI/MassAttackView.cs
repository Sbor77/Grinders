using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassAttackView : MonoBehaviour
{
    [SerializeField] private Image[] _massAttackActivate;
    [SerializeField] private Sprite _activeImage;
    [SerializeField] private Sprite _notActiveImage;

    private void Start()
    {
        SetMassAttackCooldown(0);
    }

    public void ChangedMassAttackCooldown(int active)
    {
        SetMassAttackCooldown(active);
    }

    private void SetMassAttackCooldown(int activeValue)
    {
        for (int i = 0; i < _massAttackActivate.Length; i++)
        {
            if (i < activeValue)
                _massAttackActivate[i].sprite = _activeImage;
            else
                _massAttackActivate[i].sprite = _notActiveImage;
        }
    }
}
