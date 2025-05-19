using Move2DSystem;
using UnityEngine;

public class EMissile1C : EMCoreC
{
    protected Vector3 _velocity;
    protected float _speed, _speedDelta, _angle;
    
    public void ShotMissile(float angle, float speed, float kasoku)
    {
        var direction = Moving2DSystems.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.eulerAngles;
        angles.z = angle - 90;
        transform.eulerAngles = angles;

        _speed = speed;
        _speedDelta = kasoku;
        _angle = angle;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        transform.position += _velocity;
        _speed += _speedDelta;
        _velocity = Moving2DSystems.GetDirection(_angle) * _speed;

        if (DeleteMissileCheck())
        {
            DoDelete();
        }

    }

    protected virtual void DoDelete()
    {
        Destroy(gameObject);
    }
}
