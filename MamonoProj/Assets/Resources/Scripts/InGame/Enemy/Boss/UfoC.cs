using EnumDic.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move2DSystem;
using EnumDic.System;

/// <summary>
/// ボス「UFO」挙動制御
/// </summary>
public class UfoC : BossCoreC
{
    /// <summary>
    /// 加速度
    /// </summary>
    private float _xMove=0,_yMove=0;

    private int texture;
        
    private MODE_ATTACK _attackMode;

    private float _xPosMove = 5;
    private float _yPosMove = 5;
    private float movexx, moveyy;

    private bool _isFirstAttack;

    [SerializeField]
    private GuardC GuardPrefab;

    [SerializeField]
    private EMissile1C EMissile1Prefab;

    [SerializeField]
    private ExpC LEPrefab;

    [SerializeField]
    private ExpC _prfbTarget;

    [SerializeField]
    private Sprite a, b;

    /// <summary>
    /// アニメーション
    /// </summary>
    private Animator _animAttackEffect;

    [SerializeField]
    private AudioClip magicS, moveS, chargeS;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        GetComponents();
    }

    private void GetComponents()
    {
        //アニメーション
        _animAttackEffect = _tfOwnBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (_eCoreC.GetModeBossLife()!= MODE_LIFE.Dead) _gameManaC.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        if (_posOwn.x > 632)_xMove = -14f;
        if (_posOwn.x < 8)_xMove = 14f;
        if (_posOwn.y > 462) _yMove = -14f;
        if (_posOwn.y < 8) _yMove = 14f;
    }

    protected override void FxUpDead()
    {
        if (_eCoreC.CheckIsAlive())
        {
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;

            AllCoroutineStop();
            _eCoreC.SetIsAlive(false);
            StartCoroutine(DeadAction());
        }
    }

    //移動
    private IEnumerator Moving()
    {
        ChangeTarget();

        _xPosMove = Random.Range(50, 590);
        _yPosMove = Random.Range(50, 430);
        for (int j = 0; j < 30; j++)
        {
            float angle = Random.Range(0, 360);
            Quaternion rot = transform.localRotation;
            Instantiate(LEPrefab, new Vector3(_xPosMove, _yPosMove, 0), rot).ShotEXP(angle, 0, 0.05f);
            yield return new WaitForSeconds(0.03f);
        }
        movexx = (_xPosMove - _posOwn.x) / 10;
        moveyy = (_yPosMove - _posOwn.y) / 10;
        _audioGO.PlayOneShot(moveS);
        //攻撃アニメーションスタート
        _animAttackEffect.SetBool("Attack", true);

        for (int j = 0; j < 10; j++)
        {
            transform.localPosition += new Vector3(movexx, moveyy, 0);
            yield return new WaitForSeconds(0.03f);
        }
        //攻撃アニメーションおわり
        _animAttackEffect.SetBool("Attack", false);
        yield return new WaitForSeconds(0.03f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    private enum MODE_ATTACK
    {
        GuardianBeam,
        MachineGun,
        MachineGun_Horming,
        Horming
    }

    //行動変わるヤツ
    protected override IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.3f);

        _attackMode = (MODE_ATTACK)Random.Range(0, 4);
        switch (_attackMode)
        {
            case MODE_ATTACK.GuardianBeam:
                _movingCoroutine = StartCoroutine(GuardianBeam());
                break;

            case MODE_ATTACK.MachineGun:
                if (_posPlayer.y < 240)
                {
                    _movingCoroutine = StartCoroutine(MachineGun_Under());
                }
                else
                {
                    _movingCoroutine = StartCoroutine(
                        GameData.MultiPlayerCount > 1 ? MachineGun_RandomPlayers(): MachineGun_Random());
                }
                break;

            case MODE_ATTACK.MachineGun_Horming:
                _movingCoroutine = StartCoroutine(MachineGun_Horming());
                break;

            case MODE_ATTACK.Horming:
                _movingCoroutine = StartCoroutine(Horming());
                break;

        }
        _audioGO.PlayOneShot(magicS);
        //GetAttackCount();
    }

    //回転エネルギー弾
    private IEnumerator GuardianBeam()
    {
        _animAttackEffect.SetBool("Beam", true);

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        _animAttackEffect.SetBool("Beam", false);
        for (int k = 0; k < 5; k++)
        {
            for (int j = 0; j < 6; j++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, _posOwn, rot);
                shot.ShotGuard(10, j * 60, 1.5f, 0.03f, _posOwn);
            }
            for (int j = 0; j < 6; j++)
            {
                Quaternion rot = transform.localRotation;
                GuardC shot = Instantiate(GuardPrefab, _posOwn, rot);
                shot.ShotGuard(10, j * 60, -1.5f, 0.03f, _posOwn);
            }
            yield return new WaitForSeconds(0.3f);
            _audioGO.PlayOneShot(magicS);
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン下方向
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_Under()
    {
        _animAttackEffect.SetBool("Beam", true);

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);
        _animAttackEffect.SetBool("Beam", false);



        for (int j = 0; j < 20; j++)
        {
            _audioGO.PlayOneShot(magicS);
            Vector3 direction = _posPlayer - _posOwn;
            float angle = Moving2DSystems.GetAngle(_posOwn, _posPlayer);
            angle = Random.Range(260, 280);
            for (int k = 10; k < 40; k += 2)
            {
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                shot.ShotMissile(angle, 0, k + 1);
            }
            yield return new WaitForSeconds(0.06f);
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン乱射（プレイヤー避ける）
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_Random()
    {
        _animAttackEffect.SetBool("Beam", true);

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);

        _animAttackEffect.SetBool("Beam", false);

        for (int j = 0; j < 20; j++)
        {
            _audioGO.PlayOneShot(magicS);
            Vector3 direction = _posPlayer - _posOwn;
            for (int i = 0; i < 3; i++)
            {
                float angle = Moving2DSystems.GetAngle(_posOwn, _posPlayer);
                angle += Random.Range(20, 340);
                for (int k = 10; k < 40; k += 2)
                {
                    Quaternion rot = transform.localRotation;
                    EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                    shot.ShotMissile(angle, 0, k + 1);
                }
            }
            yield return new WaitForSeconds(0.06f);
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン乱射（ランダムにプレイヤー避ける）
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_RandomPlayers()
    {
        _animAttackEffect.SetBool("Beam", true);

        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 180; i += 5)
        {
            transform.eulerAngles += Vector3.forward * 5;
            yield return new WaitForFixedUpdate();
        }
        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);

        _animAttackEffect.SetBool("Beam", false);

        for (int j = 0; j < 20; j++)
        {
            Vector3 posRandPlayer = _scPlsM.GetRandomAlivePlayer().transform.position;
            _audioGO.PlayOneShot(magicS);
            Vector3 direction = posRandPlayer - _posOwn;
            for (int i = 0; i < 3; i++)
            {
                float angle = Moving2DSystems.GetAngle(_posOwn, posRandPlayer);
                angle += Random.Range(20, 340);
                for (int k = 10; k < 40; k += 2)
                {
                    Quaternion rot = transform.localRotation;
                    EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                    shot.ShotMissile(angle, 0, k + 1);
                }
            }
            yield return new WaitForSeconds(0.06f);
        }

        for (int i = 0; i < 180; i += 5)
        {
            transform.eulerAngles -= Vector3.forward * 5;
            yield return new WaitForFixedUpdate();
        }

        GetAttackCount();
    }

    /// <summary>
    /// マシンガン狙撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun_Horming()
    {
        _animAttackEffect.SetBool("Beam", true);

        Vector3 targetPos = _posPlayer;
        float angle = Moving2DSystems.GetAngle(_posOwn, targetPos);
        Vector3 direction = Moving2DSystems.GetDirection(angle).normalized;

        Quaternion rot = transform.localRotation;

        float distance = Moving2DSystems.GetDistance(_posOwn, _posPlayer);
        if (distance < 0) distance *= -1;

        for (int k = 0; k < 7; k++)
        {
            _audioGO.PlayOneShot(moveS);
            Instantiate(_prfbTarget, _posOwn+direction*(distance/7)*(7-k), rot).ShotEXP(0, 0, 0.3f+(7-k)*0.3f);

            for(int j = 0; j < 10; j++)
            {
                Instantiate(LEPrefab, _posOwn, rot).ShotEXP(Random.Range(0, 360), 0, 0.07f);
                yield return new WaitForSeconds(0.03f);
            }
        }

        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);

        _animAttackEffect.SetBool("Beam", false);

        for (int j = 0; j < 20; j++)
        {
            _audioGO.PlayOneShot(magicS);

            for (int i = 0; i < 3; i++)
            {

                float SettedAngle =angle+ Random.Range(-10,10);
                for (int k = 10; k < 40; k += 2)
                {
                    EMissile1C shot = Instantiate(EMissile1Prefab, _posOwn, rot);
                    shot.ShotMissile(SettedAngle, 0, k + 1);
                }
            }
            yield return new WaitForSeconds(0.06f);
        }

        GetAttackCount();
    }

    //追尾突進
    private IEnumerator Horming()
    {
        //攻撃アニメーションスタート
        _animAttackEffect.SetBool("Attack", true);
        _audioGO.PlayOneShot(chargeS);
        yield return new WaitForSeconds(0.3f);

        for (int j = 0; j < 150; j++)
        {
            if (_posOwn.x > _posPlayer.x - 64)
            {
                if (_xMove > -7) _xMove -= 0.4f;
            }
            if (_posOwn.x < _posPlayer.x + 64)
            {
                if (_xMove < 7) _xMove += 0.4f;
            }
            if (_posOwn.y > _posPlayer.y - 32)
            {
                if (_yMove > -7) _yMove -= 0.4f;
            }
            if (_posOwn.y < _posPlayer.y + 32)
            {
                if (_yMove < 7) _yMove += 0.4f;
            }
            transform.localPosition += new Vector3(_xMove, _yMove, 0);

            yield return new WaitForSeconds(0.03f);
        }
        _animAttackEffect.SetBool("Attack", false);
        GetAttackCount();
    }

    //攻撃何回目？１ならもっかい２なら終わり
    private void GetAttackCount()
    {
        if (!_isFirstAttack)
        {
            _isFirstAttack = true;
            _movingCoroutine = StartCoroutine(ActionBranch());
        }
        else
        {
            _isFirstAttack = false;
            _movingCoroutine = StartCoroutine(Moving());
        }
    }

    protected override IEnumerator ArrivalAction()
    {
        yield return new WaitForFixedUpdate();
        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(Moving());
    }

    protected override IEnumerator DeadAction()
    {
        while (_posOwn.y > -64)
        {

            _gameManaC._bossNowHp = 0;
            transform.localPosition += new Vector3(Random.Range(-10, 10), -5, 0);
            _tfOwnBody.eulerAngles += new Vector3(0, 0, 10);

            yield return new WaitForFixedUpdate();
        }
        DoCollapuse();
    }

    protected override IEnumerator LeaveAction()
    {
        _xPosMove = _posPlayer.x;
        _yPosMove = _posPlayer.y+64;
        for (int j = 0; j < 3; j++)
        {
            float angle = Random.Range(0, 360);
            Quaternion rot = transform.localRotation;
            Instantiate(LEPrefab, new Vector3(_xPosMove, _yPosMove, 0), rot).ShotEXP(angle, 0, 0.05f);
            yield return new WaitForSeconds(0.03f);
        }
        movexx = (_xPosMove - _posOwn.x) / 10;
        moveyy = (_yPosMove - _posOwn.y) / 10;
        _audioGO.PlayOneShot(moveS);

        for (int j = 0; j < 10; j++)
        {
            transform.localPosition += new Vector3(movexx, moveyy, 0);
            yield return new WaitForSeconds(0.03f);
        }


        for (int i = 0; i <= 720; i+=30)
        {
            transform.position =
                _posPlayer += new Vector3(Mathf.Sin(i * Mathf.Deg2Rad), Mathf.Cos(i * Mathf.Deg2Rad), 0) * 128;
            yield return new WaitForSeconds(0.03f);
        }
        
        for (int j = 0; j < 5; j++)
        {
            transform.position -= Vector3.up*6;
            yield return new WaitForSeconds(0.03f);
        }
        _audioGO.PlayOneShot(magicS);
        for (int j = 0; j < 128; j++)
        {
            transform.position += Vector3.up * 32;
            yield return new WaitForSeconds(0.03f);
        }

    }
}
