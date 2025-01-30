using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMissile : MonoBehaviour
{
    private Vector3 velocity, pos;

    private float sspeed, kkaso, aang;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Shot(float angle, float speed, float kasoku)
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
    private void FixedUpdate()
    {
        pos = transform.position;

        transform.localPosition += velocity;
        sspeed += kkaso;
        var direction = GameData.GetDirection(aang);
        velocity = direction * sspeed;

        if (GetComponent<PMCoreC>().DeleteMissileCheck())
        {
            DeleteEMissile();
        }
    }

    private void DeleteEMissile()
    {
        Destroy(gameObject);
    }
}
