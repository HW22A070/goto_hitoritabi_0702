using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeameyeC : ETypeTurretC
{
    private float shotdown = 0.5f;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (down == 0 && tate == 0)
        {
            pull = 1;
            tate = 1;
        }
        if (pos.y <= -50)
        {
            Destroy(gameObject);
        }
        down = Random.Range(0, 1000);

        if (shotdown != 0) shotdown -= Time.deltaTime; ;
        if (shotdown <= 0)
        {
            for (int eye = 0; eye < 8; eye++)
            {
                pos = transform.position;
                if (eye==1) pos -= new Vector3(-22, 23, 0);
                else if (eye == 2) pos -= new Vector3(21, 25, 0);
                else if (eye == 3) pos -= new Vector3(28, 12, 0);
                else if (eye == 4) pos -= new Vector3(25, -3, 0);
                else if (eye == 5) pos -= new Vector3(22, -24, 0);
                else if (eye == 6) pos -= new Vector3(-23, -22, 0);
                else if (eye == 7) pos -= new Vector3(-28, 7, 0);
                float angle = GameData.GetAngle(pos,ppos);
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 20, 1);
                pos -= new Vector3(-24, 0, 0);
            }
            _audioGO.PlayOneShot(shotS);
            shotdown = 2;
        }

        pos = transform.position;
    }
}
