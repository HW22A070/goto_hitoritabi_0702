using EnumDic.Enemy;
using EnumDic.Stage;
using EnumDic.Player;
using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeonDragonC : BossCoreC
{
    /// <summary>
    /// 重力の影響を受けないモード
    /// </summary>
    private bool _isflying;

    /// <summary>
    /// 空中か
    /// </summary>
    private bool _isGround;

    /// <summary>
    /// プレイヤーの方を向くか
    /// </summary>
    private bool _isAbleRotate = true;

    /// <summary>
    /// 最大重力
    /// </summary>
    private int _gravityMax = 10;

    /// <summary>
    /// 重力加速度
    /// </summary>
    private int _gravityDelta = 4;

    /// <summary>
    /// 現在重力
    /// </summary>
    private int _gravityNow = 0;

    private float _valueHSV = 0;

    /// <summary>
    /// 足の位置と足の広さ
    /// </summary>
    //[SerializeField,Header("足の位置と足の広さ")]
    private Vector2 _posFoot = new Vector3(128, 96);

    /// <summary>
    /// プレイヤーと床の判定
    /// </summary>
    private RaycastHit2D _hitEnemyToFloor;

    [SerializeField]
    private AudioClip _shautS;

    [SerializeField]
    private AudioClip _seEXP;

    [SerializeField]
    private RaserC _prfbRaserThin,_prfbRaserFat;

    [SerializeField]
    private ExpC _prfRaserDust;

    [SerializeField]
    private CristalC _prfbCristal;

    #region 体パーツ
    [SerializeField,Header("足")]
    private SpriteRenderer _srFoot;

    [SerializeField]
    private Sprite _spFootStand, _spFootFlying;

    [SerializeField, Header("翼")]
    private GameObject _goWing;
    
    private SpriteRenderer _srWing;

    private Animator _animWing;

    [SerializeField, Header("目")]
    private GameObject _goEye;

    private Animator _animEye;

    #endregion

    #region 発射口
    [SerializeField]
    private Transform[] _tsNozzles;
    #endregion



    private MODE_ATTACK1 _attackVariation;

    private bool _isLightning;

    private bool _isCharging;

    [SerializeField]
    private AudioClip _acCharge, _acShot, _acClose;

    [SerializeField]
    private AudioSource _asRaser;

    private MODE_ATTACK1 _attackBefore { get; set; }

    private short _evolutionMine = 0;

    [SerializeField]
    private ExpC _prfbFlashEffect;

    private new void Start()
    {
        _animWing = _goWing.GetComponent<Animator>();
        _animEye = _goEye.GetComponent<Animator>();
        _srWing = _goWing.GetComponent<SpriteRenderer>();
        base.Start();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        if(_isCharging) SetBodysColor(new Color(0,0,0,1));
        else
        {
            switch (_evolutionMine)
            {
                case 0:
                    SetBodysColor(GameData.WeaponColor[0]);
                    break;

                case 1:
                    SetBodysColor(GameData.WeaponColor[1]);
                    break;

                case 2:
                    SetBodysColor(GameData.WeaponColor[2]);
                    break;

                case 3:
                    SetBodysColor(GameData.WeaponColor[3]);
                    break;

                case 4:
                    SetBodysColor(Color.HSVToRGB(_valueHSV,0.99f,0.99f));
                    _valueHSV += Time.deltaTime/5;
                    if (_valueHSV > 1) _valueHSV = 0;
                    break;
            }
        }


        //飛んでいたら地面判定は無視
        if (!_isflying)
        {
            //重力
            Ray2D playerFootRay = new Ray2D(transform.position - new Vector3(_posFoot.x / 2, _posFoot.y * 1.2f + 1.0f, 0), new Vector2(_posFoot.x, 0));
            Debug.DrawRay(playerFootRay.origin, playerFootRay.direction, Color.gray);

            _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);

            //足元に地面がなければ落下
            if (_hitEnemyToFloor)
            {
                if (!_isGround)
                {
                    _isGround = true;
                    _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(7, 6);
                    _audioGO.PlayOneShot(_seEXP);
                }
            }
            else _isGround = false;

            //地面
            if (_isGround)
            {
                GameObject floor = _hitEnemyToFloor.collider.gameObject;

                transform.position = new Vector3(_posOwn.x
                    ,16+ floor.transform.position.y + (floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y - GetComponent<BoxCollider2D>().offset.y) / 2), 0);

                //落下力をゼロにする
                _gravityNow = 0;

                //炎上！
                #region OnFire!
                _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
                if (_hitEnemyToFloor)
                {
                    ECoreC eCore = GetComponent<ECoreC>();
                    if (_hitEnemyToFloor.collider.gameObject.GetComponent<FloorC>().GetFloorMode() == MODE_FLOOR.Burning)
                    {
                        //もし炎弱点であればダメージくらう
                        if (eCore.CheckIsCritical(MODE_GUN.Heat) && eCore.TotalHp > 1) eCore.DoGetDamage(1, MODE_GUN.Heat, _hitEnemyToFloor.collider.gameObject.transform.position);
                    }
                }
                #endregion

                _srFoot.sprite = _spFootStand;
            }

            //落下
            else
            {
                //重力加速
                if (_gravityNow >= _gravityMax) _gravityNow = _gravityMax;
                else _gravityNow += _gravityDelta;

                _srFoot.sprite = _spFootFlying;
            }
            transform.position -= Vector3.up * _gravityNow;

            _animWing.SetBool("IsFlying", false);
        }
        else
        {
            _isGround = false;
            _srFoot.sprite = _spFootFlying;
            _animWing.SetBool("IsFlying", true);
        }
    }

    protected override void FxUpFight()
    {
        if (_isAbleRotate)
        {
            transform.rotation = Quaternion.Euler(_rotOwn.x,
                _posPlayer.x>_posOwn.x?180:0

                , _rotOwn.z);
        }
    }

    protected override void FxUpLeave()
    {
        base.FxUpLeave(); 
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

    private enum MODE_ATTACK1
    {
        StandSnipeRaser,
        Diffusion2,
    }
    private enum MODE_ATTACK2
    {
        StandSnipeRaser,
        FlySnipeRaser,
        Diffusion4,
    }
    private enum MODE_ATTACK3
    {
        StandSnipeRaser,
        FlySnipeRaser,
        Rolling1,
    }
    private enum MODE_ATTACK4
    {
        StandSnipeRaser,
        FlySnipeRaser,
        Diffusion16,
        Rolling1,
    }
    private enum MODE_ATTACK5
    {
        StandSnipeRaser,
        FlySnipeRaser,
        Diffusion16,
        Rolling2,
    }

    protected override IEnumerator ActionBranch()
    {
        _isAbleRotate = true;
        yield return new WaitForSeconds(0.5f);

        ChangeTarget();

        /*
        do
        {
            _attackVariation = (MODE_ATTACK)Random.Range(0, 4);
        } while (_attackVariation == _attackBefore);
        */
        _attackBefore = _attackVariation;
        switch (_eCoreC.EvoltionMode)
        {
            case 0:

                if (_evolutionMine != _eCoreC.EvoltionMode) _evolutionMine = _eCoreC.EvoltionMode;
                switch ((MODE_ATTACK1)Random.Range(0, 2))
                {
                    case MODE_ATTACK1.StandSnipeRaser:
                        _movingCoroutine = StartCoroutine(StandRaserSnipAlone());
                        break;

                    case MODE_ATTACK1.Diffusion2:
                        _movingCoroutine = StartCoroutine(Diffusion(1));
                        break;
                }
                break;

            case 1:

                if (_evolutionMine != _eCoreC.EvoltionMode) _evolutionMine = _eCoreC.EvoltionMode;
                switch ((MODE_ATTACK2)Random.Range(0, 3))
                {
                    case MODE_ATTACK2.StandSnipeRaser:
                        _movingCoroutine = StartCoroutine(StandRaserSnipAlone());
                        break;

                    case MODE_ATTACK2.FlySnipeRaser:
                        _movingCoroutine = StartCoroutine(FlyingRaserSnipAlone());
                        break;

                    case MODE_ATTACK2.Diffusion4:
                        _movingCoroutine = StartCoroutine(Diffusion(1));
                        break;
                }
                break;

            case 2:

                if (_evolutionMine != _eCoreC.EvoltionMode)
                {
                    _evolutionMine = _eCoreC.EvoltionMode;
                    _movingCoroutine = StartCoroutine(Charging(2));
                }
                else
                {
                    switch ((MODE_ATTACK3)Random.Range(0, 3))
                    {
                        case MODE_ATTACK3.StandSnipeRaser:
                            _movingCoroutine = StartCoroutine(StandRaserSnipAlone());
                            break;

                        case MODE_ATTACK3.FlySnipeRaser:
                            _movingCoroutine = StartCoroutine(FlyingRaserSnipAlone());
                            break;

                        case MODE_ATTACK3.Rolling1:
                            _movingCoroutine = StartCoroutine(Rolling(1));
                            break;
                    }
                }
                break;

            case 3:

                if (_evolutionMine != _eCoreC.EvoltionMode) _evolutionMine = _eCoreC.EvoltionMode;
                switch ((MODE_ATTACK4)Random.Range(0, 4))
                {
                    case MODE_ATTACK4.StandSnipeRaser:
                        _movingCoroutine = StartCoroutine(StandRaserSnipAlone());
                        break;

                    case MODE_ATTACK4.FlySnipeRaser:
                        _movingCoroutine = StartCoroutine(FlyingRaserSnipAlone());
                        break;

                    case MODE_ATTACK4.Rolling1:
                        _movingCoroutine = StartCoroutine(Rolling(1));
                        break;

                    case MODE_ATTACK4.Diffusion16:
                        _movingCoroutine = StartCoroutine(Diffusion(4));
                        break;
                }
                break;

            case 4:
                if (_evolutionMine != _eCoreC.EvoltionMode)
                {
                    _evolutionMine = _eCoreC.EvoltionMode;
                    _movingCoroutine = StartCoroutine(Charging(4));
                }
                else
                {
                    switch ((MODE_ATTACK5)Random.Range(0, 4))
                    {
                        case MODE_ATTACK5.StandSnipeRaser:
                            _movingCoroutine = StartCoroutine(StandRaserSnipAlone());
                            break;

                        case MODE_ATTACK5.FlySnipeRaser:
                            _movingCoroutine = StartCoroutine(FlyingRaserSnipAlone());
                            break;

                        case MODE_ATTACK5.Rolling2:
                            _movingCoroutine = StartCoroutine(Rolling(2));
                            break;


                        case MODE_ATTACK5.Diffusion16:
                            _movingCoroutine = StartCoroutine(Diffusion(4));
                            break;
                    }
                }

                break;
        }
    }

    //発射準備
    private IEnumerator ChargeRaser(List<float>[] angles, Transform[] nozzles,int chargeTimeTick)
    {
        List<Vector3>[] vecAngles = new List<Vector3>[4]{
            new List<Vector3>{ },
            new List<Vector3>{ },
            new List<Vector3>{ },
            new List<Vector3>{ },
        };
        for(int i = 0; i < angles.Length; i++)
        {
            foreach (float angle in angles[i])
            {
                vecAngles[i].Add(Moving2DSystems.GetDirection(angle).normalized);
            }
        }


        for (int j = 0; j < chargeTimeTick; j++)
        {
            for (int nozzle = 0; nozzle < angles.Length; nozzle++)
            {
                for (int i = 0; i < angles[nozzle].Count; i++)
                {
                    Vector3 posEnergySummon = nozzles[nozzle].position;
                    for (int k = 0; k < 512; k++)
                    {
                        posEnergySummon += vecAngles[nozzle][i] * 64;

                        Instantiate(_prfRaserDust, posEnergySummon, transform.rotation)
                            .ShotEXP(angles[nozzle][i] + Random.Range(0, 2) * 180, 8, 0.1f);

                        if (posEnergySummon.y < 0 || posEnergySummon.x < 0
                            || posEnergySummon.y > GameData.WindowSize.y || posEnergySummon.x > GameData.WindowSize.x) break;
                    }
                }
            }

            _isLightning = Random.Range(0, 50 - j) <= 2;

            yield return new WaitForFixedUpdate();

        }
        _isLightning = true;
    }

    //レーザー狙い撃ち一人
    private IEnumerator StandRaserSnipAlone()
    {
        _isAbleRotate = false;
        List<float>[] angles = new List<float>[4]{
            new List<float>{ },
            new List<float>{ },
            new List<float>{ },
            new List<float>{ }
        };


        GameObject player = _scPlsM.GetRandomAlivePlayer();

        for (int i = 0; i < _tsNozzles.Length; i++)
        {
            angles[i].Add(Moving2DSystems.GetAngle(_tsNozzles[i].position, player.transform.position) + Random.Range(-10, 10));
        }

        _audioGO.PlayOneShot(_acCharge);
        yield return StartCoroutine(ChargeRaser(angles, _tsNozzles,20));
        _audioGO.PlayOneShot(_acShot);

        _asRaser.Play();

        for (int i = 0; i < 30; i++)
        {
            for (int j=0;j< _tsNozzles.Length;j++)
            {
                Instantiate(_prfbRaserThin, _tsNozzles[j].position, Quaternion.Euler(0,0,0)).ShotRaser(angles[j][0],480);
            }
            yield return new WaitForFixedUpdate();
        }
        _asRaser.Stop();

        _audioGO.PlayOneShot(_acClose);
        
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    //空中レーザー狙い撃ち一人
    private IEnumerator FlyingRaserSnipAlone()
    {
        _isflying = true;
        yield return new WaitForSeconds(1.0f);
        while (transform.position.y < 400)
        {
            transform.position += Vector3.up * 4;
            yield return new WaitForFixedUpdate();
        }

        _isAbleRotate = false;
        List<float>[] angles = new List<float>[4]{
            new List<float>{ },
            new List<float>{ },
            new List<float>{ },
            new List<float>{ }
        };


        GameObject player = _scPlsM.GetRandomAlivePlayer();

        for (int i = 0; i < _tsNozzles.Length; i++)
        {
            angles[i].Add(Moving2DSystems.GetAngle(_tsNozzles[i].position, player.transform.position) + Random.Range(-10, 10));
        }

        _audioGO.PlayOneShot(_acCharge);
        yield return StartCoroutine(ChargeRaser(angles, _tsNozzles, 20));
        _audioGO.PlayOneShot(_acShot);

        _asRaser.Play();

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < _tsNozzles.Length; j++)
            {
                Instantiate(_prfbRaserThin, _tsNozzles[j].position, Quaternion.Euler(0, 0, 0)).ShotRaser(angles[j][0],480);
            }
            yield return new WaitForFixedUpdate();
        }
        _asRaser.Stop();

        _audioGO.PlayOneShot(_acClose);
        _isflying =false;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 拡散レーザー
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    private IEnumerator Diffusion(int count)
    {
        _isflying = true;
        yield return new WaitForSeconds(1.0f);

        Vector3 moving = Moving2DSystems.GetSneaking(_posOwn, new Vector3(320, 240, 0), 30);
        for(int i = 0; i < 30; i++)
        {
            transform.position += moving;
            yield return new WaitForFixedUpdate();
        }

        _isAbleRotate = false;
        List<float>[] angles =new List<float>[4]{
            new List<float>{ },
            new List<float>{ },
            new List<float>{ },
            new List<float>{ }
        };

        GameObject player = _scPlsM.GetRandomAlivePlayer();

        float rollValue = 0;
        for(int i = 0; i < _tsNozzles.Length; i++)
        {
            for(int j = 0; j < count; j++)
            {
                angles[i].Add(rollValue);
                rollValue += 360/(count * 4);
            }
        }

        _audioGO.PlayOneShot(_acCharge);
        yield return StartCoroutine(ChargeRaser(angles, _tsNozzles, 40));
        _audioGO.PlayOneShot(_acShot);

        _asRaser.Play();
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 4);
        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < _tsNozzles.Length; j++)
            {
                foreach(float angle in angles[j])
                {
                    Instantiate(_prfbRaserFat, _tsNozzles[j].position, Quaternion.Euler(0, 0, 0)).ShotRaser(angle,480);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        _asRaser.Stop();

        _audioGO.PlayOneShot(_acClose);
        _isflying = false;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 回転レーザー
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    private IEnumerator Rolling(int count)
    {
        _isflying = true;
        yield return new WaitForSeconds(1.0f);

        Vector3 moving = Moving2DSystems.GetSneaking(_posOwn, new Vector3(320, 240, 0), 30);
        for (int i = 0; i < 30; i++)
        {
            transform.position += moving;
            yield return new WaitForFixedUpdate();
        }

        _isAbleRotate = false;
        List<float>[] angles = new List<float>[4]{
            new List<float>{ },
            new List<float>{ },
            new List<float>{ },
            new List<float>{ }
        };

        GameObject player = _scPlsM.GetRandomAlivePlayer();

        float rollValue = 0;
        for (int i = 0; i < _tsNozzles.Length; i++)
        {
            for (int j = 0; j < count; j++)
            {
                angles[i].Add(rollValue);
                rollValue += 360 / (count * 4);
            }
        }

        _audioGO.PlayOneShot(_acCharge);
        yield return StartCoroutine(ChargeRaser(angles, _tsNozzles, 40));
        _audioGO.PlayOneShot(_acShot);

        _asRaser.Play();
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 4);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < _tsNozzles.Length; j++)
            {
                foreach (float angle in angles[j])
                {
                    Instantiate(_prfbRaserFat, _tsNozzles[j].position, Quaternion.Euler(0, 0, 0)).ShotRaser(angle,480);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 90; i++)
        {
            for (int j = 0; j < _tsNozzles.Length; j++)
            {
                foreach (float angle in angles[j])
                {
                    Instantiate(_prfbRaserFat, _tsNozzles[j].position, Quaternion.Euler(0, 0, 0)).ShotRaser(angle+i,480);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        _asRaser.Stop();

        _audioGO.PlayOneShot(_acClose);
        _isflying = false;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// チャージ
    /// </summary>
    /// <returns></returns>
    private IEnumerator Charging(int count)
    {
        _isflying = true;
        yield return new WaitForSeconds(1.0f);

        Vector3 moving = Moving2DSystems.GetSneaking(_posOwn, new Vector3(320, 360, 0), 30);
        for (int i = 0; i < 30; i++)
        {
            transform.position += moving;
            yield return new WaitForFixedUpdate();
        }
        List<CristalC> listCri = new List<CristalC> { };

        if (count == 2)
        {
            listCri.Add(Instantiate(_prfbCristal, new Vector3(160, 240, 0), Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(0.03f);
            listCri.Add(Instantiate(_prfbCristal, new Vector3(480, 240, 0), Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(0.03f);
            MODE_GUN mode1 = (MODE_GUN)Random.Range(0, 4);
            listCri[0].SetColor(mode1);
            MODE_GUN mode2 = (MODE_GUN)Random.Range(0, 4);
            while (mode1 == mode2)
            {
                mode2 = (MODE_GUN)Random.Range(0, 4);
            }
            listCri[1].SetColor(mode2);
            _isCharging = true;
        }

        if (count == 4)
        {
            listCri.Add(Instantiate(_prfbCristal, new Vector3(160, 100, 0), Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(0.03f);
            listCri.Add(Instantiate(_prfbCristal, new Vector3(480, 100, 0), Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(0.03f);
            listCri.Add(Instantiate(_prfbCristal, new Vector3(160, 340, 0), Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(0.03f);
            listCri.Add(Instantiate(_prfbCristal, new Vector3(480, 340, 0), Quaternion.Euler(0, 0, 0)));
            yield return new WaitForSeconds(0.03f);
            listCri[0].SetColor(MODE_GUN.Heat);
            listCri[1].SetColor(MODE_GUN.Crash);
            listCri[2].SetColor(MODE_GUN.Shining);
            listCri[3].SetColor(MODE_GUN.Physical);
            _isCharging = true;
        }



        _eCoreC.SetInvisible(true);

        bool isSuccess = true;
        for(int charge = 0; charge < 20*33; charge++)
        {
            yield return new WaitForSeconds(0.03f);
            bool isBroken = true;
            foreach(CristalC cristal in listCri)
            {
                if (cristal!=null)
                {
                    isBroken = false;
                    break;
                }
            }
            if (isBroken)
            {
                isSuccess = false;
                Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, Quaternion.Euler(0,0,0)).ShotEXP(0, 0, 0.3f);
                float timespeed = Time.timeScale;
                TimeManager.ChangeTimeValue(0.1f);
                yield return new WaitForSeconds(0.3f / 10);
                TimeManager.ChangeTimeValue(timespeed);
                _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 30);
                break;
            }

        }

        _isCharging = false;
        _eCoreC.SetInvisible(false);

        foreach (CristalC cristal in listCri)
        {
            if (cristal != null)
            {
                Destroy(cristal.gameObject);
            }
        }

        if (isSuccess)
        {

            _isAbleRotate = false;
            List<float>[] angles = new List<float>[4]{
                new List<float>{ },
                new List<float>{ },
                new List<float>{ },
                new List<float>{ }
            };

            GameObject player = _scPlsM.GetRandomAlivePlayer();

            float rollValue = 0;
            for (int i = 0; i < _tsNozzles.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    angles[i].Add(rollValue);
                    rollValue += 360 / (8 * 4);
                }
            }

            _audioGO.PlayOneShot(_acCharge);
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 80);
            yield return StartCoroutine(ChargeRaser(angles, _tsNozzles, 80));
            _audioGO.PlayOneShot(_acShot);

            _asRaser.Play();
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 5);
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < _tsNozzles.Length; j++)
                {
                    foreach (float angle in angles[j])
                    {
                        Instantiate(_prfbRaserFat, _tsNozzles[j].position, Quaternion.Euler(0, 0, 0)).ShotRaser(angle,480);
                    }
                }
                yield return new WaitForFixedUpdate();
            }
            _asRaser.Stop();

            _audioGO.PlayOneShot(_acClose);
        }
        else
        {
            _animEye.SetBool("IsDown", true);
            _isflying = false;
            yield return new WaitForSeconds(10.0f);
            _animEye.SetBool("IsDown", false);
        }

        _isflying = false;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    protected override IEnumerator ArrivalAction()
    {
        SetBodysColor(GameData.WeaponColor[0]);

        _isflying = true;
        while (_posOwn.y > 250)
        {
            transform.position -= Vector3.up * 3;
            yield return new WaitForFixedUpdate();
        }

        _isflying = false;
        while (!_isGround)yield return new WaitForFixedUpdate();

        yield return new WaitForSeconds(1.0f);
        _audioGO.PlayOneShot(_shautS);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(4, 10);
        yield return new WaitForSeconds(0.3f);

        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    private void SetBodysColor(Color color)
    {
        _srOwnBody.color = color;
        _srWing.color = color;
    }
}
