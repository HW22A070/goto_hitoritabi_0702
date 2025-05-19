using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move2DSystem;

public class ETypeSnowC : ETypeCoreC
{
    protected Vector3 _muki, _velocity;
    
    [SerializeField]
    protected float _fixTargetPosTime = 1.0f,_speed=1.0f;
    protected float _time = 0;
   

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (_time >= 0) _time -= Time.deltaTime;
        else
        {
            float angle = Moving2DSystems.GetAngle(_posOwn, _posPlayer);
            var direction = Moving2DSystems.GetDirection(angle);
            _velocity = direction * 2;

            _time = _fixTargetPosTime;
        }
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += _velocity * _speed;
    }
}
