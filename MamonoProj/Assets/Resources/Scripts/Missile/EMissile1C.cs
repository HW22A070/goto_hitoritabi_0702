using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMissile1C : MonoBehaviour
{
    Vector3 velocity, pos;
    float sspeed, kkaso, aang;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void EShot1(float angle, float speed, float kasoku)
    {
        var direction = GameData.GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        kkaso = kasoku;
        aang = angle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;

        transform.localPosition += velocity;
        sspeed += kkaso;
        var direction = GameData.GetDirection(aang);
        velocity = direction * sspeed;

        if (GetComponent<EMCoreC>().DeleteMissileCheck())
        {
            Destroy(gameObject);
        }

    }
}
