using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenC : MonoBehaviour
{
    private Vector3 _velocity, _posOwn;
    private float _speed, _speedDelta, _angle,_rollValue;

    [SerializeField]
    private bool geigeki,bombbarrier = true, bombsosai;

    public void EShot1(float angle, float speed, float kasoku,float kaiten)
    {
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        _speed = speed;
        _speedDelta = kasoku;
        _angle = angle;
        _rollValue = kaiten;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _posOwn = transform.position;

        transform.localEulerAngles += new Vector3(0, 0, _rollValue);

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
