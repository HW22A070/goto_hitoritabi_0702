using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretC : ETypeTurretC
{
    private float shotdown=0.5f;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        if (shotdown != 0) shotdown -= Time.deltaTime; ;
        if (shotdown <= 0)
        {
            float angle = Random.Range(0.0f, 360.0f);
            Quaternion rot = transform.localRotation;
            _posOwn.y += 14;
            EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
            shot.ShotMissile(angle, 10, 0);
            _audioGO.PlayOneShot(shotS);
            shotdown = 1;
        }
    }
}
