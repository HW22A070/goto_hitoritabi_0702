using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceC : ETypeTurretC
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
            float angle = GameData.GetAngle(transform.position,_posPlayer);
            Quaternion rot = transform.localRotation;
            EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
            shot.EShot1(angle, 5, 0);
            _audioGO.PlayOneShot(shotS);
            shotdown = 2.0f;
        }

    }
}
