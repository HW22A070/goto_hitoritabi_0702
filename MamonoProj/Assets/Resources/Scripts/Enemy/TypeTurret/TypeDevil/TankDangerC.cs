﻿using System.Collections;
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
    protected AudioClip _seGun, _seEXP;

    [SerializeField]
    private ExpC _prfbFlashEffect;
    
    private GameObject _goCamera;

    private new void Start()
    {
        base.Start();
        _goCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (shotdown > 0) shotdown -= Time.deltaTime;
        if (_posPlayer.y >= _posOwn.y - 16 && _posPlayer.y <= _posOwn.y + 16 && shotdown <= 0)
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
            spriteRenderer.flipX = false;
        }
        else
        {
            angle = 0;
            spriteRenderer.flipX = true;
        }
        _isCharging = true;
        if (_posOwn.x > _posPlayer.x) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;

        float movetemp = _move;
        _move = 0;
        yield return new WaitForSeconds(1.5f);

        Quaternion rot = transform.localRotation;
        _audioGO.PlayOneShot(_seGun);
        _audioGO.PlayOneShot(_seEXP);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 10);
        EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
        shot.EShot1(angle, 100, 0);
        shot.transform.position += shot.transform.up * 64;

        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, rot).EShot1(0, 0, 0.1f);
        yield return new WaitForSeconds(0.5f);
        _move = movetemp;
        _isDontDown = false;
        _isCharging = false;
    }
}
