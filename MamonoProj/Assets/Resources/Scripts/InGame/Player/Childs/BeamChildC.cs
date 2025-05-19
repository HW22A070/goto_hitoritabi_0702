using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamChildC : ChildsCoreC
{
    private Vector3 _center,_loopOfset;

    private float _loopSpeed = 0,_loopSpeedDelta,_loopSpeedBase=3,_posOfset,_looprRadius, _looprRadiusBase = 32;

    [SerializeField, Tooltip("ビームballアタッチ")]
    private BeamCristalC _prfbBeamCristal;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile PBeamP, PRaserP;

    [SerializeField]
    private PExpC _prfbEffect;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _looprRadius = _looprRadiusBase;
        _loopSpeedDelta = _loopSpeedBase;
    }

    public void SetOfset(float ofset) {
        _posOfset = ofset;
    }

    private void Update()
    {
        SetPosition();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.position +=_posDelta;

        _loopSpeed += _loopSpeedDelta;
        while (_loopSpeed > 360)
        {
            _loopSpeed -= 360;
        }

        /*
        if (_loopSpeedDelta > 10)
        {
            Instantiate(_prfbEffect, transform.position, transform.rotation).EShot1(Random.Range(0, 360), 0.1f, 0.2f);
        }
        */

        if (_loopSpeedDelta > _loopSpeedBase) _loopSpeedDelta--;
        if (_looprRadius > _looprRadiusBase) _looprRadius-=2;
    }

    private void SetPosition()
    {
        _pos = transform.position;
        _posPlayer = _playerGO.transform.position;
        _loopOfset = new Vector3(Mathf.Sin((_loopSpeed + _posOfset) * Mathf.Deg2Rad) * _looprRadius, Mathf.Cos((_loopSpeed + _posOfset) * Mathf.Deg2Rad) * _looprRadius, 0);
        _posDelta = Moving2DSystems.GetSneaking(_pos, _posPlayer, 4) + Moving2DSystems.GetSneaking(_center, _loopOfset, 4);
    }

    /// <summary>
    /// レーザー攻撃
    /// </summary>
    public void DoAttackRaser()
    {
        for (int i = -3; i <= 3; i += 2)
        {
            PMissile shot = Instantiate(PRaserP, transform.position, transform.rotation);
            shot.ShotMissle((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) + i * 10, 0, 1000);
            shot.transform.position += shot.transform.up * 320;
        }
    }

    /// <summary>
    /// スラッシュ攻撃
    /// </summary>
    public void DoAttackSlash()
    {
        Instantiate(_prfbEffect, transform.position, Quaternion.Euler(0,0,0)).ShotEXP(Random.Range(0, 360), 0.0f, 0.08f);
        if (_loopSpeedDelta < 20) _loopSpeedDelta += 6;
        if (_looprRadius <96) _looprRadius+=12;
    }

    /// <summary>
    /// レーザー攻撃2
    /// </summary>
    public void DoPutBeamCristal() => Instantiate(_prfbBeamCristal, _pos, Quaternion.Euler(0, 0, 0));

    /// <summary>
    /// 敵場所特定、自分とのアングルを求める
    /// </summary>
    /// <returns></returns>
    private float GetAngleWithEnemy(Vector3 _posOwn)
    {
        GameObject[] myObjects;
        myObjects = GameObject.FindGameObjectsWithTag("Enemy");
        if (myObjects.Length > 0)
        {
            Vector3 enemyPos = GameData.FixPosition(myObjects[Random.Range(0, myObjects.Length)].transform.position, 32, 32);
            return Moving2DSystems.GetAngle(_posOwn, enemyPos);
        }
        else
        {
            if (GameData.Round == 0)
            {
                myObjects = GameObject.FindGameObjectsWithTag("Target");
                if (myObjects.Length > 0)
                {
                    Vector3 enemyPos = GameData.FixPosition(myObjects[Random.Range(0, myObjects.Length)].transform.position, 32, 32);
                    return Moving2DSystems.GetAngle(_posOwn, enemyPos);
                }
            }
            return _scPlayer.CheckPlayerAngleIsRight() ? 0 : 180;
        }
    }
}
