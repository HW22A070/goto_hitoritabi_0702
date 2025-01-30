using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingC : BombC
{
    Vector3 ppos;
    float hms;

    private int mode;

    [SerializeField]
    private ExpC  _prhbTargetEffect;



    public void EShot1(float angle, float speed, float exp, int hunjin, float exptime, float hormingstart)
    {
        var direction = GameData.GetDirection(angle);
        velocity = direction * speed;
        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;

        sspeed = speed;
        aang = angle;
        eexp = exp;
        eexptim = exptime;
        hunj = hunjin;
        hms = hormingstart;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;

        transform.localPosition += velocity;

        hms--;
        if ((hms < 0 || pos.y > 450) && mode == 0)
        {
            Instantiate(_prhbTargetEffect, ppos, Quaternion.Euler(0,0,0)).EShot1(0, 0, 0.8f);
            float angle2 = GameData.GetAngle(pos, ppos);
            var direction2 = GameData.GetDirection(angle2);
            velocity = direction2 * sspeed;
            var angles = transform.localEulerAngles;
            angles.z = angle2 - 90;
            transform.localEulerAngles = angles;
            mode = 1;
        }

        eexp--;
        if (eexp <= 0) Explosion();

        if (GetComponent<EMCoreC>().DeleteMissileCheck()) Explosion();
    }

}
