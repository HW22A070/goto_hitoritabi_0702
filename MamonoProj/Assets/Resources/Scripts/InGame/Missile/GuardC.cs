using UnityEngine;

public class GuardC : EMCoreC
{
    private Vector3 _velocity,  fpos;

    private float _radius, _radiusDelta, _speed, _offset;
    private float _spped = 0;

    public void ShotGuard(float radius, float offset, float speed, float deltar, Vector3 defpos)
    {
        _radius = radius;
        _offset = offset;
        _speed = speed;
        _radiusDelta = deltar;
        _posOwn = defpos;
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        fpos = _posOwn + new Vector3(Mathf.Sin((_spped + _offset) * Mathf.Deg2Rad) * _radius, Mathf.Cos((_spped + _offset) * Mathf.Deg2Rad) * _radius, 0);

        transform.position = fpos;

        _spped += _speed;
        _radius += _radiusDelta;
        while (_spped > 360)
        {
            _spped -= 360;
        }

        if (GetComponent<EMCoreC>().DeleteMissileCheck())
        {
            DoDelete();
        }
    }

    protected virtual void DoDelete() => Destroy(gameObject);
}
