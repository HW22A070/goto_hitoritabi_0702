using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpC : MonoBehaviour
{
    Vector3 velocity, pos;

    [SerializeField, Tooltip("エフェクト扱い")]
    private bool _isEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed,float delete)
    {
        var direction = GameData.GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;
        Destroy(gameObject,delete);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition += velocity;

        if (!_isEffect)
        {
            if (GetComponent<EMCoreC>().DeleteMissileCheck())
            {
                Destroy(gameObject);
            }
        }

    }

}
