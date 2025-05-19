using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingC : BombC
{
    protected GameObject _goTarget;
    protected float _timeHormingStart;

    protected int _mode;

    [SerializeField]
    protected ExpC  _prhbTargetEffect;

    public void ShotHoming(float angle, float speed, float exp, float radius, float hormingstart,GameObject target)
    {
        ShotBomb(angle, speed, 0, exp, radius);
        _timeHormingStart = hormingstart;
        _goTarget = target;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        _timeHormingStart--;
        if ((_timeHormingStart < 0 || _posOwn.y > 450) && _mode == 0)
        {
            Instantiate(_prhbTargetEffect, _goTarget.transform.position, Quaternion.Euler(0,0,0)).ShotEXP(0, 0, 0.8f);
            _angle = Moving2DSystems.GetAngle(_posOwn, _goTarget.transform.position);
            var angles = transform.localEulerAngles;
            angles.z = _angle - 90;
            transform.localEulerAngles = angles;
            _mode = 1;
        }
    }

}
