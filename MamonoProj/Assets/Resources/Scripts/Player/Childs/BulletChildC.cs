using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletChildC : ChildsCoreC
{
    private Vector3 _posOfset,_shotPos;


    private float angle, _ownAngle;

    /// <summary>
    /// ターゲット
    /// </summary>
    public Vector3 GOTarget;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile PBulletP, PRifleP;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

    private bool _isTarget=false;

    // Update is called once per frame
    void Update()
    {
        _pos = transform.position;
        _shotPos = _pos + transform.right * 32;
        _posPlayer = playerGO.transform.position + _posOfset + new Vector3(_scPlayer.CheckPlayerAngleIsRight() ? -24: 24, 0,0);
        _posDelta = GameData.GetSneaking(_pos, _posPlayer, 4);
        _isTarget = _scPlayer.GetFlontEnemy()!= null;
        if (_isTarget) GOTarget = _scPlayer.GetFlontEnemy().transform.position;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.position += _posDelta;

        if (_isTarget) angle = GameData.GetAngle(_pos, GOTarget);
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
    }

     public void SetOfset(Vector3 ofset)
    {
        _posOfset = ofset;
    }

    public void DoAttackSniper()
    {
        float angle;
        if (_isTarget) angle = GameData.GetAngle(_pos, GOTarget);
        else angle = _scPlayer.CheckPlayerAngleIsRight() ? 0 : 180;
        PMissile shot = Instantiate(PRifleP, _shotPos, transform.rotation);
        shot.Shot(angle + Random.Range(-1, 2), 0, 32);
        shot.transform.position += shot.transform.up * 128;
        PlayBulletEffect();
        transform.position -= transform.right * 64;

    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void PlayBulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _shotPos+(transform.right*64), transform.rotation);
        bulletEf.transform.parent = transform;
        bulletEf.EShot1(angle, 0, 0.12f);
    }
}
