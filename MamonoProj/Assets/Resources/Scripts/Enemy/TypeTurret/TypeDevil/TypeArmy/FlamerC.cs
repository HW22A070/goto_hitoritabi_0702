using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerC : ETypeArmyC
{
   
    float shotdown = 2;

    public BombC BombPrefab;

    public AudioClip shotS;


    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (shotdown != 0) shotdown -= Time.deltaTime; ;
        if (shotdown <= 0)
        {
            float angle = GameData.GetAngle(transform.position,ppos);
            Quaternion rot = transform.localRotation;
            BombC shot = Instantiate(BombPrefab, pos, rot);
            shot.EShot1(angle, 3, 0, 100, 30, 1.0f);
            _audioGO.PlayOneShot(shotS);
            shotdown = 3;

        }
    }
}
