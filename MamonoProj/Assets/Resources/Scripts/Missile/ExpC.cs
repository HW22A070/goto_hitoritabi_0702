using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpC : MonoBehaviour
{
    private Vector3 _velocity, _posOwn;

    [SerializeField, Tooltip("エフェクト扱い")]
    private bool _isEffect;

    public void EShot1(float angle, float speed,float delete)
    {
        var direction = GameData.GetDirection(angle);
        _velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
        Destroy(gameObject,delete);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition += _velocity;

        if (!_isEffect)
        {
            if (GetComponent<EMCoreC>().DeleteMissileCheck())
            {
                Destroy(gameObject);
            }
        }

    }

}
