using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingC : BombC
{
    private Vector3 _posPlayer;
    private float _timeHormingStart;

    private int _mode;

    [SerializeField]
    private ExpC  _prhbTargetEffect;

    public void EShot1(float angle, float speed, float exp, int hunjin, float exptime, float hormingstart)
    {
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        _speed = speed;
        _angle = angle;
        _expCount = exp;
        _expCountTime = exptime;
        _hunj = hunjin;
        _timeHormingStart = hormingstart;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        _posOwn = transform.position;
        _posPlayer = GameObject.Find("Player").transform.position;

        transform.localPosition += _velocity;

        _timeHormingStart--;
        if ((_timeHormingStart < 0 || _posOwn.y > 450) && _mode == 0)
        {
            Instantiate(_prhbTargetEffect, _posPlayer, Quaternion.Euler(0,0,0)).EShot1(0, 0, 0.8f);
            float angle2 = GameData.GetAngle(_posOwn, _posPlayer);
            var direction2 = GameData.GetDirection(angle2);
            _velocity = direction2 * _speed;
            var angles = transform.localEulerAngles;
            angles.z = angle2 - 90;
            transform.localEulerAngles = angles;
            _mode = 1;
        }

        _expCount--;
        if (_expCount <= 0) Explosion();

        if (GetComponent<EMCoreC>().DeleteMissileCheck()) Explosion();
    }

}
