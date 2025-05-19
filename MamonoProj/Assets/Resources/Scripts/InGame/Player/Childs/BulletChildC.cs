using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move2DSystem;

public class BulletChildC : ChildsCoreC
{
    private Vector3 _posOfset,_shotPos;


    private float angle, _ownAngle;

    /// <summary>
    /// ターゲット
    /// </summary>
    public Vector3 GOTarget;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile PBulletP, PRifleP,_prfbRailGun;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

    private bool _isTarget=false;

    [SerializeField]
    private Sprite _spNormal, _spSpesial;

    // Update is called once per frame
    void Update()
    {
        _pos = transform.position;
        _shotPos = _pos + transform.right * 32;
        _posPlayer = _playerGO.transform.position + _posOfset + new Vector3(_scPlayer.CheckPlayerAngleIsRight() ? -24: 24, 0,0);
        _posDelta = Moving2DSystems.GetSneaking(_pos, _posPlayer, 4);
        _isTarget = _scPlayer.GetFlontEnemy()!= null;
        if (_isTarget) GOTarget = _scPlayer.GetFlontEnemy().transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += _posDelta;

        if (_isTarget) angle = Moving2DSystems.GetAngle(_pos, GOTarget);
        else angle = _scPlayer.CheckPlayerAngleIsRight() ? 0 : 180;


        if (_ownAngle != angle)
        {
            _ownAngle = angle;
        }
        
        transform.eulerAngles = transform.forward * _ownAngle++;

        if (90 < angle && angle < 270)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }

        SetSpriteBySpecial();
    }

     public void SetOfset(Vector3 ofset)
    {
        _posOfset = ofset;
    }

    public void DoAttackSniper()
    {
        float angle;
        if (_isTarget) angle = Moving2DSystems.GetAngle(_pos, GOTarget);
        else angle = _scPlayer.CheckPlayerAngleIsRight() ? 0 : 180;
        PMissile shot = Instantiate(PRifleP, _shotPos, transform.rotation);
        shot.ShotMissle(angle + Random.Range(-1, 2), 0, 32);
        shot.transform.position += shot.transform.up * 128;
        PlayBulletEffect();
        transform.position -= transform.right * 64;

    }

    public void DoAttackRailGun()
    {
        float angle;
        if (_isTarget) angle = Moving2DSystems.GetAngle(_pos, GOTarget);
        else angle = _scPlayer.CheckPlayerAngleIsRight() ? 0 : 180;
        PMissile shot = Instantiate(_prfbRailGun, _shotPos, transform.rotation);
        shot.ShotMissle(angle, 100, 0);
        shot.transform.position += shot.transform.up * 128;
        transform.position -= transform.right * 96;

    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void PlayBulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _shotPos+(transform.right*64), transform.rotation);
        bulletEf.transform.parent = transform;
        bulletEf.ShotEXP(angle, 0, 0.12f);
    }

    private void SetSpriteBySpecial()
    {
        spriteRenderer.sprite = _scPlayer.CheckIsSpecial() ? _spSpesial : _spNormal;
    }
}
