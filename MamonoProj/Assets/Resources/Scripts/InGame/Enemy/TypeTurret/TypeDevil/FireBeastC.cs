using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move2DSystem;

public class FireBeastC : ETypeDevilC
{
    protected float shotdown = 1.0f;

    protected bool _isCharging;

    protected float angle = 0;

    [SerializeField]
    protected BombC _prfbBomb;

    [SerializeField]
    private Transform _tfMouse;

    [SerializeField]
    protected AudioClip shotS,chargeS;

    private Animator _animOwn;

    private new void Start()
    {
        
        base.Start();
        _animOwn = _srOwnBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        if (shotdown != 0) shotdown -= Time.deltaTime;
        if (CheckIsUnderAnyPlayers() && shotdown <= 0)
        {
            StartCoroutine(Shoot());
            shotdown = 4;
        }
    }

    protected IEnumerator Shoot()
    {
        _animOwn.SetBool("isCharge", true);
        _audioGO.PlayOneShot(chargeS);
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

        float angleTarget = Moving2DSystems.GetAngle(_tfMouse.position, _posPlayer);
        yield return new WaitForSeconds(1.00f);

        Quaternion rot = transform.localRotation;
        Instantiate(_prfbBomb, _tfMouse.position, rot)
            .ShotBomb(angleTarget, 15,0,100,64);
        _audioGO.PlayOneShot(shotS);
        yield return new WaitForSeconds(0.5f);

        _animOwn.SetBool("isCharge", false);
        _move = movetemp;
        _isDontDown = false;
        _isCharging = false;
    }

    /// <summary>
    /// 射程圏内にプレイヤーがいるか確認
    /// </summary>
    /// <returns></returns>
    protected bool CheckIsUnderAnyPlayers()
    {
        foreach (GameObject player in _scPlsM.GetAlivePlayers())
        {
            if (player.transform.position.y <= _posOwn.y)
            {
                //見つけたターゲットに上書き
                _goPlayer = player;
                return true;
            }
        }
        return false;
    }
}
