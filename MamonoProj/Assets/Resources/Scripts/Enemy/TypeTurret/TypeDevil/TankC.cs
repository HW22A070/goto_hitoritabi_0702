using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankC : ETypeDevilC
{
    protected float shotdown = 1.0f;

    protected bool _isCharging;

    protected float angle = 0;

    [SerializeField]
    protected EMissile1C EMissile1Prefab;

    [SerializeField]
    protected AudioClip shotS;

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (ppos.y >= pos.y - 16 && ppos.y <= pos.y + 16 && shotdown <= 0)
        {
            StartCoroutine(Shoot());
            shotdown = 3;
        }
    }

    protected IEnumerator Shoot()
    {
        _isDontDown = true;
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
        _isCharging = true;
        if (pos.x > ppos.x) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        float movetemp = move;
        move = 0;
        yield return new WaitForSeconds(1.00f);

        Quaternion rot = transform.localRotation;
        Instantiate(EMissile1Prefab, pos, rot).EShot1(angle, 1, 0.07f);
        _audioGO.PlayOneShot(shotS);
        yield return new WaitForSeconds(0.5f);
        move = movetemp;
        _isDontDown = false;
        _isCharging = false;
    }
}
