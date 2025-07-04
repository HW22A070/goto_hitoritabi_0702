﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///榴弾
/// </summary>
public class HowitzerC : EMissile1C
{
    protected float _expCount, _expCountTime;
    protected int _hunjins;

    /// <summary>
    /// スピーカ
    /// </summary>
    protected private AudioSource _audioGO;

    [SerializeField]
    protected ExpC ExpPrefab;

    [SerializeField]
    protected AudioClip expS;

    [SerializeField]
    protected bool bombbarrier = true, bombsosai;

    [SerializeField]
    [Tooltip("爆発物")]
    protected ExpC _prhbExpShining;

    // Start is called before the first frame update
    protected void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void ShotHowitzer(float angle, float speed, float kasoku, float expCountdown, int hunjins, float exptime)
    {
        ShotMissile(angle, speed, kasoku);
        _expCount = expCountdown;
        _expCountTime = exptime;
        _hunjins = hunjins;
    }

    // Update is called once per frame
    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        //time_ex
        _expCount--;
        if (_expCount <= 0) DoDelete();
    }

    protected override void DoDelete()
    {
        if (_prhbExpShining != null) ExpEffect(4);

        for (int i = 0; i < _hunjins; i++)
        {
            
            Vector3 direction2 = new Vector3(Random.Range(10, 630), Random.Range(10, 470), 0);
            float angle2 = Random.Range(0, 360);
            Quaternion rot2 = transform.localRotation;
            ExpC shot2 = Instantiate(ExpPrefab, _posOwn, rot2);
            shot2.ShotEXP(angle2, Random.Range(1, 10.0f), _expCountTime);
        }
        _audioGO.PlayOneShot(expS);
        Destroy(gameObject);
    }

    protected void ExpEffect(int shiningValue)
    {
        Instantiate(_prhbExpShining, _posOwn, Quaternion.Euler(0, 0, 0)).ShotEXP(0, 0, 0.3f);
        for (int i = 0; i < shiningValue; i++)
        {
            Instantiate(_prhbExpShining, _posOwn + new Vector3(Random.Range(-48, 48), Random.Range(-48, 48), 0), Quaternion.Euler(0, 0, 0))
                .ShotEXP(0, 0, 0.3f);
        }
    }

}
