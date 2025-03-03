using UnityEngine;

public class GuardC : MonoBehaviour
{
    private Vector3 _velocity, _posOwn, fpos;

    private float _radius, _radiusDelta, _speed, _offset;
    private float _spped = 0;

    public void EShot1(float radius, float offset, float speed, float deltar, Vector3 defpos)
    {
        _radius = radius;
        _offset = offset;
        _speed = speed;
        _radiusDelta = deltar;
        _posOwn = defpos;
    }

    void FixedUpdate()
    {

        fpos = _posOwn + new Vector3(Mathf.Sin((_spped + _offset) * Mathf.Deg2Rad) * _radius, Mathf.Cos((_spped + _offset) * Mathf.Deg2Rad) * _radius, 0);

        transform.localPosition = fpos;

        _spped += _speed;
        _radius += _radiusDelta;
        while (_spped > 360)
        {
            _spped -= 360;
        }

        if (GetComponent<EMCoreC>().DeleteMissileCheck())
        {
            Destroy(gameObject);
        }
    }
}
