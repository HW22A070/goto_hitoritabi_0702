using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnBeastC : ETypeDevilC
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
        if (CheckIsTargetingAnyPlayers(16) && shotdown <= 0)
        {
            StartCoroutine(Shoot());
            shotdown = 3;
        }
    }

    protected IEnumerator Shoot()
    {
        _isDontDown = true;
        if (_posOwn.x > _posPlayer.x)
        {
            angle = 180;
            _srOwnBody.flipX = false;
        }
        else
        {
            angle = 0;
            _srOwnBody.flipX = true;
        }
        _isCharging = true;
        if (_posOwn.x > _posPlayer.x) _srOwnBody.flipX = false;
        else _srOwnBody.flipX = true;

        float movetemp = _move;
        _move = 0;
        yield return new WaitForSeconds(1.00f);

        Quaternion rot = transform.localRotation;
        Instantiate(EMissile1Prefab, _posOwn, rot).ShotMissile(angle, 1, 0.07f);
        _audioGO.PlayOneShot(shotS);
        yield return new WaitForSeconds(0.5f);
        _move = movetemp;
        _isDontDown = false;
        _isCharging = false;
    }
}
