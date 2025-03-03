using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PExpC : MonoBehaviour
{
    Vector3 _velocity, _posOwn;
    float _speed, _speedDelta, _angle;

    public void EShot1(float angle, float speed,float delete)
    {
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
        Destroy(gameObject,delete);

        StartCoroutine("Game");

    }

    // Update is called once per frame
    private IEnumerator Game()
    {
        for (; ; )
        {
            _posOwn = transform.position;
            transform.localPosition += _velocity;

            if (GetComponent<PMCoreC>().DeleteMissileCheck())
            {
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(0.03f);
        }
    }

}
