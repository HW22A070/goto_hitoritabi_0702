using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamC : MonoBehaviour
{
    private Vector3 _velocity, _posOwn;
    private float _speed, _speedDelta, _angle, _expCounttime;
    private int _hunj;

    [SerializeField]
    private ExpC ExpPrefab;

    public void EShot1(float angle, float speed, float kasoku, int hunjin, float exptime)
    {
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        _speed = speed;
        _speedDelta = kasoku;
        _angle = angle;
        _expCounttime = exptime;
        _hunj = hunjin;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _posOwn = transform.position;

        transform.localPosition += _velocity;
        _speed += _speedDelta;
        var direction = GameData.GetDirection(_angle);
        _velocity = direction * _speed;

        if (_posOwn.y < 0 || _posOwn.y > 480 || _posOwn.x < 0 || _posOwn.x > 640)
        {
            for (int i = 0; i < _hunj; i++)
            {
                Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
                float angle2 = Random.Range(0, 360);
                Quaternion rot2 = transform.localRotation;
                ExpC shot2 = Instantiate(ExpPrefab, _posOwn, rot2);
                shot2.EShot1(angle2, Random.Range(1, 10.0f), _expCounttime);
            }
            Destroy(gameObject);
        }
    }
}
