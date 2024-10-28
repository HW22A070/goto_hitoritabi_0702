using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETypeArmyC : ETypeDevilC
{
    [SerializeField]
    protected float _moveSpeedDelta, _moveSpeedMax;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (pos.x > ppos.x - 32)
        {
            if (move > -_moveSpeedMax) move -= _moveSpeedDelta;
            spriteRenderer.flipX = false;
        }
        if (pos.x < ppos.x + 32)
        {
            if (move < _moveSpeedMax) move += _moveSpeedDelta;
            spriteRenderer.flipX = true;
        }
    }
}
