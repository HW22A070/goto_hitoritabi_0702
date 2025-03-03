using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCannonC : ETypeFireC
{
    private float shotdown=0.7f;
    private float angle = 0;
    private int i;

    public EMissile1C EMissile1Prefab;

    public AudioClip shotS;

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (shotdown != 0 && mode !=0) shotdown -= Time.deltaTime;
        if (_posPlayer.y >= _posOwn.y - 16 && _posPlayer.y <= _posOwn.y + 16 && shotdown <= 0&&mode==1)
        {
            if (_posOwn.x > _posPlayer.x)
            {
                angle = 180;
                spriteRenderer.flipX = false;
            }
            else
            {
                angle = 0;
                spriteRenderer.flipX = true;
            }
            Quaternion rot = transform.localRotation;
            _posOwn.y += 8;
            for (i = 0; i <= 1; i++)
            {
                _posOwn.y -= i*16;
                Instantiate(EMissile1Prefab, _posOwn, rot).EShot1(angle, 10, 0);
                _audioGO.PlayOneShot(shotS);
            }
            shotdown = 3;
        }
    }

}
