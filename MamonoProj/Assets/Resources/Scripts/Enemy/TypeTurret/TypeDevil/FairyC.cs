using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyC : ETypeDevilC
{
    private bool _isCharging;

    private float shotdown;
    
    private float angle = 0;

    public EMissile1C EMissile1Prefab;

    public AudioClip damageS, deadS,shotS;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        playerGO = GameObject.Find("Player");
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        ppos = playerGO.transform.position;

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (ppos.y >= pos.y - 16 && ppos.y <= pos.y + 16 && shotdown <= 0)
        {
            StartCoroutine("Shoot");
            shotdown = 3;
        }

        
    }

    private IEnumerator Shoot()
    {
        _isCharging = true;
        if (pos.x > ppos.x)spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        float  movetemp = move;
        move = 0;
        yield return new WaitForSeconds(1.00f);
        for (int i = 0; i < 4; i++)
        {
            if (pos.x > ppos.x)
            {
                angle = 180 + (-30 + (i * 20));
                spriteRenderer.flipX = false;
            }
            else
            {
                angle = -30 + (i * 20);
                spriteRenderer.flipX = true;
            }
            Quaternion rot = transform.localRotation;
            EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
            shot.EShot1(angle, 20, 0);
            yield return new WaitForSeconds(0.03f);
        }
        _audioGO.PlayOneShot(shotS);
        yield return new WaitForSeconds(0.5f);
        move = movetemp;
        _isCharging=false;
    }

   
}
