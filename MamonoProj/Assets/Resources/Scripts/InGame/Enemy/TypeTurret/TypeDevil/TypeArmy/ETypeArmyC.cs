using UnityEngine;

public class ETypeArmyC : ETypeDevilC
{
    [SerializeField]
    protected float _moveSpeedDelta, _moveSpeedMax;

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (_posOwn.x > _posPlayer.x - 32)
        {
            if (_move > -_moveSpeedMax) _move -= _moveSpeedDelta;
            _srOwnBody.flipX = false;
        }
        if (_posOwn.x < _posPlayer.x + 32)
        {
            if (_move < _moveSpeedMax) _move += _moveSpeedDelta;
            _srOwnBody.flipX = true;
        }
    }
}
