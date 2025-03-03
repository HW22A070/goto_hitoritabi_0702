using UnityEngine;

public class EMissile1C : MonoBehaviour
{
    private Vector3 _velocity, _posOwn;
    private float _speed, _speedDelta, _angle;


    public void EShot1(float angle, float speed, float kasoku)
    {
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        _speed = speed;
        _speedDelta = kasoku;
        _angle = angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _posOwn = transform.position;

        transform.localPosition += _velocity;
        _speed += _speedDelta;
        var direction = GameData.GetDirection(_angle);
        _velocity = direction * _speed;

        if (GetComponent<EMCoreC>().DeleteMissileCheck())
        {
            Destroy(gameObject);
        }

    }
}
