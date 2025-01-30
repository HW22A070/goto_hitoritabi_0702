using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSnakeC : ETypeFireC
{
    private float shotdown;
    private float angle = 0;
    private int i;
    public EMissile1C EMissile1Prefab;
    
    public AudioClip shotS;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (ppos.y >= pos.y - 16 && ppos.y <= pos.y + 16 && shotdown <= 0&&mode==1)
        {
            

            if (pos.x > ppos.x)
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
            pos.y += 8;
            for (i = 0; i <= 1; i++)
            {
                pos.y -= i * 16;
                EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
                shot.EShot1(angle, 10, 0);
            }
            _audioGO.PlayOneShot(shotS);
            shotdown = 0.5f;
        }

    }
}
