using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDangerC : ETypeDevilC
{
    protected float shotdown = 1.0f;

    protected bool _isCharging;

    protected float angle = 0;

    [SerializeField]
    protected EMissile1C EMissile1Prefab;

    [SerializeField]
    protected AudioClip _seGun, _seEXP,_seCharge,_seAlarm;

    [SerializeField]
    private ExpC _prfbFlashEffect,_prfbThunderDot;

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (shotdown > 0) shotdown -= Time.deltaTime;
        if (CheckIsTargetingAnyPlayers(16) && shotdown <= 0)
        {
            StartCoroutine(Shoot());
            shotdown = 6;
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
        _audioGO.PlayOneShot(_seAlarm);

        for (int i = 0; i < 60; i++)
        {
            if (_srOwnBody.flipX == false)
            {
                int y = 0;
                for (int x = -16; x >= -(int)_posOwn.x; x -= 4)
                {
                    ExpC thunder = Instantiate(_prfbThunderDot, _posOwn + new Vector3(x, y, 0), _rotOwn);
                    thunder.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255,(byte)( (255 / 60) * i));
                    thunder.ShotEXP(0, 0, 0.03f);
                    if (y > 12) y -= 1;
                    else if (y < -12) y += 1;
                    else y += Random.Range(0, 2) == 0 ? 1 : -1;
                }
            }
            else
            {
                int y = 0;
                for (int x = 16; x <=GameData.WindowSize.x- (int)_posOwn.x; x += 4)
                {
                    ExpC thunder = Instantiate(_prfbThunderDot, _posOwn + new Vector3(x, y, 0), _rotOwn);
                    thunder.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)((255 / 60) * i));
                    thunder.ShotEXP(0, 0, 0.03f);
                    if (y > 12) y -= 1;
                    else if (y < -12) y += 1;
                    else y += Random.Range(0, 2) == 0 ? 1 : -1;
                }
            }
            if (Random.Range(0, 5) == 0) _audioGO.PlayOneShot(_seCharge);
            yield return new WaitForFixedUpdate();
        }

        Quaternion rot = transform.localRotation;
        _audioGO.PlayOneShot(_seGun);
        _audioGO.PlayOneShot(_seEXP);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 10);
        EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
        shot.ShotMissile(angle, 100, 0);
        shot.transform.position += shot.transform.up * 64;

        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, Quaternion.Euler(0,0,0)).ShotEXP(0, 0, 0.1f);
        yield return new WaitForSeconds(0.5f);
        _move = movetemp;
        _isDontDown = false;
        _isCharging = false;
    }
}
