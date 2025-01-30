using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperC : ETypeTurretC
{
    private float shotdown = 0.5f;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (shotdown != 0) shotdown -= Time.deltaTime; ;
        if (shotdown <= 0)
        {
            float angle = 90;
            Quaternion rot = transform.localRotation;
            pos.y += 14;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            shot.EShot1(angle, 0.5f, 2);
            _audioGO.PlayOneShot(shotS);
            shotdown = 1.5f;
        }

    }
}
