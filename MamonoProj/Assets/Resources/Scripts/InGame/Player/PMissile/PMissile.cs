using Move2DSystem;
using UnityEngine;

public class PMissile : PMCoreC
{
    protected Vector3 _velocity;

    protected float _speed, _speedDelta, _angle;

    public void ShotMissle(float angle, float speed, float kasoku)
    {
        _velocity = Moving2DSystems.GetDirection(angle) * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        _speed = speed;
        _speedDelta = kasoku;
        _angle = angle;
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        _posOwn = transform.position;

        //進める
        transform.localPosition += _velocity;
        _speed += _speedDelta;
        var direction = Moving2DSystems.GetDirection(_angle);
        _velocity = direction * _speed;

        if (DeleteMissileCheck())
        {
            DoDelete();
        }
    }

    protected virtual void DoDelete() => Destroy(gameObject);
}
