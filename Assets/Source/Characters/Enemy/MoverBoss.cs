using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBoss : Mover
{
    public override void SetDamage()
    {
        base.SetDamage();

        if (_target.CurrentState == State.Attack)
            _enemy.Attack(_target);
    }
}
