using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Floor;
using EnumDic.Enemy;
using EnumDic.Player;

public class MechaZombieC : MonoBehaviour
{

    private bool _isDontDown = false;

    /// <summary>
    /// 下降中か
    /// </summary>
    private bool _isflying;

    /// <summary>
    /// 空中か
    /// </summary>
    private bool _isGround;

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

    /// <summary>
    /// 上昇力
    /// </summary>
    private int _powerJet = 0;

    /// <summary>
    /// 現在の上昇力
    /// </summary>
    private int _powerJetDelta=0;

    /// <summary>
    /// プレイヤーの場所を見るか
    /// </summary>
    private bool _isLookAble = true;

    /// <summary>
    /// 足の位置と足の広さ
    /// </summary>
    //[SerializeField,Header("足の位置と足の広さ")]
    private Vector2 _posFoot = new Vector3(128, 72);

    /// <summary>
    /// プレイヤーと床の判定
    /// </summary>
    private RaycastHit2D _hitEnemyToFloor;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    private GameObject _goCamera;

    /// <summary>
    /// 変形モード
    /// </summary>
    private MODE_GUN _modeTransform;

    private MODE_GUN[] _transformVariations = new MODE_GUN[4]
    {
        MODE_GUN.Shining,
        MODE_GUN.Physical,
        MODE_GUN.Heat,
        MODE_GUN.Crash
    };

    /// <summary>
    /// HPの割合
    /// </summary>
    private float _probabilityHP = 100;

    private int _hpDefault = 0;

    private float _angle, _moveX, _moveY;
    private GameObject _scGameManager;
    private Vector3 _posOwn, _posPlayer, _posShot;
    private float _posyFoot;

    private Quaternion rot;
    private SpriteRenderer _srOwn;

    [SerializeField]
    private SpriteRenderer _srPlant;

    [SerializeField]
    private Sprite _beam, _bullet, _bomb,_fire,_transfomation,_beam2,_bullet2,_bomb2,_fire2;

    [SerializeField]
    private EMissile1C Ballet1P, _prfbRaserFat,_prfbRailGunMissile;

    [SerializeField]
    private HomingC RocketPrefab;

    [SerializeField]
    private ExpC jet, SwingP,_prhbTargetEffect,_prfbFlashEffect,_pefbVirusEF;

    [SerializeField]
    private FireworkC FireworkP;

    [SerializeField]
    private UnitC _goUnitP;

    private ExhaustC _scOwnExh;

    private bool _isPositionLeft;

    private MODE_GUN _gunMode;

    /// <summary>
    /// 全形態
    /// </summary>
    private bool _isFullThrottle;

    [SerializeField]
    private ClearEffectC StaffPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip  _seMissile, _seGun, _seFire,_seEXP;

    [SerializeField]
    [Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    /// <summary>
    /// 動作中のコルーチン 
    /// </summary>
    private Coroutine _movingCoroutine;

    /// <summary>
    /// ECoreCのコンポーネント
    /// </summary>
    private ECoreC _eCoreC;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent();
        for (int j = 0; j < _eCoreC.hp.Length; j++)
        {
            _hpDefault += _eCoreC.hp[j];
        }
        _eCoreC.IsBoss = true;
        _posOwn = transform.position;
        _scGameManager.GetComponent<GameManagement>()._bossNowHp = _hpDefault;
        _scGameManager.GetComponent<GameManagement>()._bossMaxHp = _hpDefault;
    }

    private void GetComponent()
    {
        _scOwnExh = GetComponent<ExhaustC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");
        playerGO = GameObject.Find("Player");
        _srOwn = GetComponent<SpriteRenderer>();
        _eCoreC = GetComponent<ECoreC>();
        _scGameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    private void Update()
    {
        _posOwn = transform.position;
        _posPlayer = playerGO.transform.position;

        _posyFoot = _posOwn.y - 72;
        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead) _scGameManager.GetComponent<GameManagement>()._bossNowHp = _eCoreC.TotalHp;
        _probabilityHP = _eCoreC.TotalHp * 100 / _hpDefault;

        if (_isLookAble)
        {
            if (_posOwn.x < _posPlayer.x)
            {
                _srOwn.flipX = true;
                _posShot = _posOwn + transform.right * 70;
            }
            else
            {
                _srOwn.flipX = false;
                _posShot = _posOwn - transform.right * 70;
            }

            _srPlant.flipX = _srOwn.flipX;
        }


    }

    

    void FixedUpdate()
    {

        //重力
        Ray2D playerFootRay = new Ray2D(transform.position - new Vector3(_posFoot.x / 2, _posFoot.y * 1.2f + 1.0f, 0), new Vector2(_posFoot.x, 0));
        Debug.DrawRay(playerFootRay.origin, playerFootRay.direction, Color.gray);

        //飛んでいたら地面判定は無視
        if (!_isflying)
        {
            _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
            //足元に地面がなければ空中
            _isGround = _hitEnemyToFloor;
        }
        else _isGround = false;

        //地面
        if (_isGround)
        {
            GameObject floor = _hitEnemyToFloor.collider.gameObject;
            _posOwn = transform.position;

            transform.position = new Vector3(_posOwn.x
                , floor.transform.position.y + (floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y - GetComponent<BoxCollider2D>().offset.y) / 2), 0);

            //重力ゼロ
            _gravityNow = 0;

            //炎上！
            _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
            if (_hitEnemyToFloor)
            {
                ECoreC eCore = GetComponent<ECoreC>();
                if (_hitEnemyToFloor.collider.gameObject.GetComponent<FloorC>().GetFloorMode()==MODE_FLOOR.Burning)
                {
                    //もし炎弱点であればダメージくらう
                    if (eCore.CheckIsCritical(3) && eCore.TotalHp > 1) eCore.DoGetDamage(1, 3, _hitEnemyToFloor.collider.gameObject.transform.position);
                }
            }
        }
        //空中
        else
        {
            //重力加速
            if (_gravityNow >= _gravityMax) _gravityNow = _gravityMax;
            else _gravityNow += _gravityDelta;
        }
        transform.position -= new Vector3(0, _gravityNow, 0);

        transform.position += transform.up * _powerJetDelta*2 ;
        if (_powerJetDelta < _powerJet) _powerJetDelta++;
        else if (_powerJetDelta > _powerJet) _powerJetDelta--;
        if (_powerJet > 0)
        {
            JetPush();
        }

        if (_isFullThrottle)
        {
            Instantiate(_pefbVirusEF, GameData.GetRandomWindowPosition(), rot).EShot1(Random.Range(0,360),0.2f,2.0f);
        }



        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            transform.localPosition += new Vector3(-1, 0, 0);
            if (_posOwn.x < 550)
            {
                _movingCoroutine = StartCoroutine(ActionBranch());
                _eCoreC.BossLifeMode = MODE_LIFE.Fight;
                for(int i=0;i<5;i++)SummonUnit();
            }
        }

        //DeathAction
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            DestroyAllUnit();
            FloorManagerC.SetStageGimic(100, 0);
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;
            _scGameManager.GetComponent<GameManagement>()._bossNowHp = 0;
            FloorManagerC.SetStageGimic(100,0);
            if (GameData.Round == GameData.GoalRound)
            {
                Instantiate(StaffPrefab, new Vector3(320, -100, 0), transform.localRotation);
            }
            else
            {
                playerGO.GetComponent<PlayerC>().StageMoveAction();
            }
            Destroy(gameObject);
        }

        
    }

    private enum MODE_ATTACK
    {
        Attack,
        Move,
        SummonUnit
    }

    private enum MODE_BEAM
    {
        Raser,
        VerticalBeam
    }

    private enum MODE_BULLET
    {
        MachineGun,
        ShotGun,
        RailGun
    }

    private enum MODE_FIRE
    {
        Drop,
        Rolling
    }

    private enum MODE_BOMB
    {
        RocketFall,
        RocketSnipe
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        _isLookAble = true;
        ResetUnitPosition();

        //ガチモード切替判定
        if (_eCoreC.EvoltionMode == 4)
        {
            if (!_isFullThrottle)
            {
                _scOwnExh.enabled = true;
                _isFullThrottle = true;
                FloorManagerC.SetGimicBedRock(MODE_FLOOR.PreNeedle);

                for (int i = 0; i < Random.Range(2, 6); i++)
                {
                    SummonUnit();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        if (!_isFullThrottle)
        {
            _modeTransform = _transformVariations[_eCoreC.EvoltionMode];
            StartCoroutine(Transformation(_modeTransform));
            yield return new WaitForSeconds(0.7f);

            //ユニットいなかったら召喚
            if (GameObject.FindGameObjectsWithTag("MechaUnit").Length <= 0)
            {
                for(int i = 0; i < Random.Range(2,6); i++)
                {
                    SummonUnit();
                    yield return new WaitForSeconds(0.1f);
                }
            }

            switch ((MODE_ATTACK)Random.Range(0, System.Enum.GetNames(typeof(MODE_ATTACK)).Length)) {
                case MODE_ATTACK.Attack:
                    switch (_modeTransform)
                    {
                        //ビームモード
                        case MODE_GUN.Shining:
                            switch((MODE_BEAM)Random.Range(0, System.Enum.GetNames(typeof(MODE_BEAM)).Length))
                            {
                                case MODE_BEAM.Raser:
                                    _movingCoroutine = StartCoroutine(Raser());
                                    break;

                                case MODE_BEAM.VerticalBeam:
                                    _movingCoroutine = StartCoroutine(IceBeam());
                                    break;
                            }
                            break;

                        //バレットモード
                        case MODE_GUN.Physical:
                            switch((MODE_BULLET)Random.Range(0, System.Enum.GetNames(typeof(MODE_BULLET)).Length))
                            {
                                case MODE_BULLET.MachineGun:
                                    _movingCoroutine = StartCoroutine(MachineGun());
                                    break;

                                case MODE_BULLET.ShotGun:
                                    _movingCoroutine = StartCoroutine(ShotGun());
                                    break;

                                case MODE_BULLET.RailGun:
                                    _movingCoroutine = StartCoroutine(RailGun());
                                    break;
                            }
                            break;

                        //バーン
                        case MODE_GUN.Heat:
                            switch ((MODE_FIRE)Random.Range(0, System.Enum.GetNames(typeof(MODE_FIRE)).Length))
                            {
                                case MODE_FIRE.Drop:
                                    _movingCoroutine = StartCoroutine(DropFire());
                                    break;

                                case MODE_FIRE.Rolling:
                                    _movingCoroutine = StartCoroutine(Rolling());
                                    break;
                            }
                            break;

                        //ボム
                        case MODE_GUN.Crash:

                            switch ((MODE_BOMB)Random.Range(0, System.Enum.GetNames(typeof(MODE_BOMB)).Length))
                            {
                                case MODE_BOMB.RocketFall:
                                    _movingCoroutine = StartCoroutine(RocketFall());
                                    break;

                                case MODE_BOMB.RocketSnipe:
                                    _movingCoroutine = StartCoroutine(RocketSniper());
                                    break;
                            }
                            break;
                    }
                    break;

                case MODE_ATTACK.Move:
                    _movingCoroutine = StartCoroutine(Move(true));
                    break;

                case MODE_ATTACK.SummonUnit:
                    _movingCoroutine = StartCoroutine(DoSummonUnit(2));
                    break;
            }
        }
        //ガチ
        else
        {
            //ユニットいなかったらめちゃ召喚
            if (GameObject.FindGameObjectsWithTag("MechaUnit").Length <= 0)
            {
                for (int i = 0; i < Random.Range(5, 10); i++)
                {
                    SummonUnit();
                    yield return new WaitForSeconds(0.06f);
                }
            }

            SummonUnit();

            switch ((MODE_ATTACK)Random.Range(0, System.Enum.GetNames(typeof(MODE_ATTACK)).Length))
            {
                case MODE_ATTACK.Attack:
                    _modeTransform = (MODE_GUN)Random.Range(0, System.Enum.GetNames(typeof(MODE_GUN)).Length);
                    StartCoroutine(Transformation(_modeTransform));
                    yield return new WaitForSeconds(0.3f);

                    switch (_modeTransform)
                    {
                        //ビームモード
                        case MODE_GUN.Shining:
                            switch ((MODE_BEAM)Random.Range(0, System.Enum.GetNames(typeof(MODE_BEAM)).Length))
                            {
                                case MODE_BEAM.Raser:
                                    _movingCoroutine = StartCoroutine(Raser2());
                                    break;

                                case MODE_BEAM.VerticalBeam:
                                    _movingCoroutine = StartCoroutine(IceBeam());
                                    break;
                            }
                            break;

                        //バレットモード
                        case MODE_GUN.Physical:
                            switch ((MODE_BULLET)Random.Range(0, System.Enum.GetNames(typeof(MODE_BULLET)).Length))
                            {
                                case MODE_BULLET.MachineGun:
                                    _movingCoroutine = StartCoroutine(MachineGun());
                                    break;

                                case MODE_BULLET.ShotGun:
                                    _movingCoroutine = StartCoroutine(ShotGun());
                                    break;

                                case MODE_BULLET.RailGun:
                                    _movingCoroutine = StartCoroutine(RailGun());
                                    break;
                            }
                            break;

                        //バーン
                        case MODE_GUN.Heat:
                            switch ((MODE_FIRE)Random.Range(0, System.Enum.GetNames(typeof(MODE_FIRE)).Length))
                            {
                                case MODE_FIRE.Drop:
                                    _movingCoroutine = StartCoroutine(DropFire());
                                    break;

                                case MODE_FIRE.Rolling:
                                    _movingCoroutine = StartCoroutine(Rolling());
                                    break;
                            }
                            break;

                        //ボム
                        case MODE_GUN.Crash:

                            switch ((MODE_BOMB)Random.Range(0, System.Enum.GetNames(typeof(MODE_BOMB)).Length))
                            {
                                case MODE_BOMB.RocketFall:
                                    _movingCoroutine = StartCoroutine(RocketFall2());
                                    break;

                                case MODE_BOMB.RocketSnipe:
                                    _movingCoroutine = StartCoroutine(RocketSniper2());
                                    break;
                            }
                            break;
                    }
                    break;

                //召喚
                case MODE_ATTACK.Move:
                    _movingCoroutine = StartCoroutine(DoSummonUnit(Random.Range(1, 4)));
                    break;
                //移動
                case MODE_ATTACK.SummonUnit:
                    _movingCoroutine = StartCoroutine(MoveFast(true));
                    break;

            }
        }
    }

    /// <summary>
    /// 衛生
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoSummonUnit(int value)
    {
        _isLookAble = true;
        for (int k = 0; k < value; k++)
        {
            SummonUnit();
            yield return new WaitForSeconds(0.3f);
        }
        
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move(bool isSeedActionBranch)
    {
        _isLookAble = true;
        _isflying = true;
        _powerJet = 10;
        Vector3 target = _posPlayer;
        //if (_isGatch) Instantiate(_prhbTargetEffect, target, rot).EShot1(0, 0, 1.7f);


        while (_posyFoot < 480)
        {
            yield return new WaitForSeconds(0.03f);
        }

        //if (_damagePar < 50) SkyFireWorkDown(target);

        _powerJet = 0;

        transform.position = new Vector3(_posPlayer.x, _posOwn.y, 0);

        //プレイヤーより下に行くまで噴射
        while (_posyFoot > _posPlayer.y || _posyFoot > GameData.WindowSize.y - 192)
        {
            yield return new WaitForSeconds(0.03f);
        }
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(7, 6);
        _audioGO.PlayOneShot(_seEXP);
        
        _isPositionLeft = true;
        _isflying = false;
        if(isSeedActionBranch) _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 高速移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveFast(bool isSeedActionBranch)
    {
        _isLookAble = true;
        _isflying = true;
        _powerJet = 30;
        Vector3 target = _posPlayer;
        //if (_isGatch) Instantiate(_prhbTargetEffect, target, rot).EShot1(0, 0, 1.7f);


        while (_posyFoot < 480)
        {
            yield return new WaitForSeconds(0.03f);
        }

        //if (_damagePar < 50) SkyFireWorkDown(target);

        _powerJetDelta = 10;
        _powerJet = 0;

        transform.position = new Vector3(_posPlayer.x,_posOwn.y, 0);

        //プレイヤーより下に行くまで噴射
        while (_posyFoot > _posPlayer.y || _posyFoot > GameData.WindowSize.y - 192)
        {
            yield return new WaitForSeconds(0.03f);
        }
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(7, 6);
        _audioGO.PlayOneShot(_seEXP);

        _isPositionLeft = true;
        _isflying = false;
        if (isSeedActionBranch) _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// レーザー
    /// </summary>
    /// <returns></returns>
    private IEnumerator Raser()
    {
        _isLookAble = true;
        yield return new WaitForSeconds(0.3f);
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.GetRandomWindowPosition();
            targetpos = GameData.FixPosition(targetpos, 32, 32);
            unit[j].GetComponent<UnitC>().AttackRaser(GameData.GetRandomWindowPosition(), j * 0.09f, 0.6f+(unit.Length-j)*0.09f);
        }

        Instantiate(_prhbTargetEffect, _posPlayer, Quaternion.Euler(0,0,0)).EShot1(0, 0, 1.3f);
        float angle = GameData.GetAngle(_posShot, _posPlayer) + Random.Range(-1, 1);

        yield return new WaitForSeconds(0.9f+(unit.Length * 0.09f));

        Quaternion rot = transform.localRotation;
        _audioGO.PlayOneShot(_seGun);
        EMissile1C shot = Instantiate(_prfbRaserFat, _posShot, transform.rotation);
        shot.EShot1(angle, 0, 1000);
        shot.transform.position += shot.transform.up * 320;
        yield return new WaitForSeconds(1.5f);
        
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// レーザー2
    /// </summary>
    /// <returns></returns>
    private IEnumerator Raser2()
    {
        _isLookAble = true;
        yield return new WaitForSeconds(0.3f);
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.GetRandomWindowPosition();
            targetpos = GameData.FixPosition(targetpos, 32, 32);
            unit[j].GetComponent<UnitC>().AttackRaser(GameData.GetRandomWindowPosition(), 0, 0.6f);
        }

        Instantiate(_prhbTargetEffect, _posPlayer, Quaternion.Euler(0, 0, 0)).EShot1(0, 0, 1.3f);
        float angle = GameData.GetAngle(_posShot, _posPlayer) + Random.Range(-1, 1);

        yield return new WaitForSeconds(0.9f);

        Quaternion rot = transform.localRotation;
        _audioGO.PlayOneShot(_seGun);
        EMissile1C shot = Instantiate(_prfbRaserFat, _posShot, transform.rotation);
        shot.EShot1(angle, 0, 1000);
        shot.transform.position += shot.transform.up * 320;
        yield return new WaitForSeconds(1.0f);

        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 上からレーザー
    /// </summary>
    /// <returns></returns>
    private IEnumerator IceBeam()
    {
        _isLookAble = true;
        yield return new WaitForSeconds(0.3f);
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.GetRandomWindowPosition();
            targetpos = GameData.FixPosition(targetpos, 32, 32);
            unit[j].GetComponent<UnitC>().AttackIceBeam(0.6f);
        }


        yield return new WaitForSeconds(1.6f);

        if(!_isFullThrottle) StartCoroutine(Move(true));
        else StartCoroutine(MoveFast(true));

    }

    /// <summary>
    /// マシンガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun()
    {
        _isLookAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3  targetpos = GameData.FixPosition(GameData.GetRandomWindowPosition(), 32, 32);
            unit[j].GetComponent<UnitC>().AttackMachineGun(targetpos, j * 0.3f);
        }
        Instantiate(_prhbTargetEffect, _posPlayer, rot).EShot1(0, 0, 1.3f);
        yield return new WaitForSeconds(0.3f + unit.Length * 0.3f);
        float angle = GameData.GetAngle(_posShot, _posPlayer) + Random.Range(-1, 1);

        yield return new WaitForSeconds(0.6f);

        for (int j = 0; j < 10; j++)
        {
            Quaternion rot = transform.localRotation;
            _audioGO.PlayOneShot(_seGun);
            Instantiate(Ballet1P, _posShot, rot).EShot1(angle, 30, 0);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(1.5f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// ショットガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotGun()
    {
        _isLookAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            unit[j].GetComponent<UnitC>().AttackShotGun(0.6f, _srOwn.flipX);
        }

        yield return new WaitForSeconds(1.0f);

        if (!_isFullThrottle) StartCoroutine(Move(true));
        else StartCoroutine(MoveFast(true));
    }

    /// <summary>
    /// レールガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator RailGun()
    {
        _isLookAble = false;
        yield return new WaitForSeconds(0.3f);
        Quaternion rot = transform.localRotation;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                Vector3 movePos = _posShot + ((_posPlayer - _posShot) / 15 * j / 2);
                unit[j].GetComponent<UnitC>().AttackRailGun(movePos, 1.5f, _srOwn.flipX);
            }
        }

        Vector3 target = _posPlayer;
        float angle = GameData.GetAngle(_posShot, target) + Random.Range(-1, 1);

        for(int k = 0; k < 18; k++)
        {
            Instantiate(_prhbTargetEffect, target, rot).EShot1(0, 0, 0.3f);
            yield return new WaitForSeconds(0.1f);
        }
        
        _audioGO.PlayOneShot(_seGun);
        _audioGO.PlayOneShot(_seEXP);
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 10);
        for (int k = 0; k < unit.Length; k++)
        {
            _audioGO.PlayOneShot(_seGun);
            Instantiate(_prfbRailGunMissile, _posShot, rot).EShot1(angle, 100, 0);
        }
        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, rot).EShot1(0, 0, 0.1f);

        yield return new WaitForSeconds(3.0f);

        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// ローリング
    /// </summary>
    /// <returns></returns>
    private IEnumerator Rolling()
    {
        Quaternion rot = transform.localRotation;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                unit[j].GetComponent<UnitC>().AttackRolling(1.5f);
            }
        }
        yield return new WaitForSeconds(2.0f);
        if (!_isFullThrottle) StartCoroutine(Move(false));
        else StartCoroutine(MoveFast(false));
        yield return new WaitForSeconds(7.0f);

        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 落下炎
    /// </summary>
    /// <returns></returns>
    private IEnumerator DropFire()
    {
        _isLookAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.FixPosition(GameData.GetRandomWindowPosition(), 32, 32);
            unit[j].GetComponent<UnitC>().AttackDropFire(targetpos, j * 0.3f);
        }
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Move(true));

    }


    /// <summary>
    /// ロケット狙撃1
    /// </summary>
    /// <returns></returns>
    private IEnumerator RocketSniper()
    {
        Quaternion rot = transform.localRotation;
        _isLookAble = false;
        Vector3 target = _posPlayer;
        float angle = GameData.GetAngle(_posShot, target) + Random.Range(-1, 1);
        Instantiate(_prhbTargetEffect, _posPlayer, rot).EShot1(0, 0, 1.7f);


        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                Vector3 movePos = _posShot + ((_posPlayer - _posShot) / 15 * j / 2);
                unit[j].GetComponent<UnitC>().AttackRockerSniper(0.9f, _srOwn.flipX,angle);
            }
        }

        yield return new WaitForSeconds(1.2f);

        if (!_isFullThrottle) StartCoroutine(Move(true));
        else StartCoroutine(MoveFast(true));
    }

    /// <summary>
    /// ロケット狙撃2
    /// </summary>
    /// <returns></returns>
    private IEnumerator RocketSniper2()
    {
        Quaternion rot = transform.localRotation;
        _isLookAble = false;
        Vector3 target = _posPlayer;
        float angle = GameData.GetAngle(_posShot, target) + Random.Range(-20, 20);
        Instantiate(_prhbTargetEffect, _posPlayer, rot).EShot1(0, 0, 1.7f);


        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                Vector3 movePos = _posShot + ((_posPlayer - _posShot) / 15 * j / 2);
                unit[j].GetComponent<UnitC>().AttackRockerSniper(0.6f, _srOwn.flipX, angle);
            }
        }

        yield return new WaitForSeconds(1.2f);

        if (!_isFullThrottle) StartCoroutine(Move(true));
        else StartCoroutine(MoveFast(true));

    }


    /// <summary>
    /// ロケット
    /// </summary>
    /// <returns></returns>
    private IEnumerator RocketFall ()
    {
        _isLookAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            float ofset = (600 / unit.Length) * j;
            unit[j].GetComponent<UnitC>().AttackRocket(ofset, j * 0.21f);
        }
        yield return new WaitForSeconds(1.0f + unit.Length * 0.21f);

        float kabeR = GameData.GetAngle(_posShot, new Vector3(GameData.WindowSize.x+50, GameData.WindowSize.y-20, 0));
        float kabeL = GameData.GetAngle(_posShot, new Vector3(GameData.WindowSize.x - 50, GameData.WindowSize.y - 20, 0));

        for (int j = 0; j < 20; j++)
        {
            float angle = Random.Range(kabeL, kabeR);
            Quaternion rot = transform.localRotation;
            _audioGO.PlayOneShot(_seMissile);
            Instantiate(RocketPrefab, _posShot, rot).EShot1(angle, 15, 300, 5, 0.5f, Random.Range(20, 30));
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(1.5f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// ロケット2
    /// </summary>
    /// <returns></returns>
    private IEnumerator RocketFall2()
    {
        _isLookAble = true;

        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            float ofset = (600 / unit.Length) * j;
            unit[j].GetComponent<UnitC>().AttackRocket(ofset, 0);
        }
        yield return new WaitForSeconds(3.0f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }


    /// <summary>
    /// ユニット召喚
    /// </summary>
    private void SummonUnit()
    {
        Instantiate(_goUnitP, _posOwn, Quaternion.Euler(0, 0, 0)).SetParent(gameObject);
        _audioGO.PlayOneShot(_seFire);
        ResetUnitPosition();
    }

    /// <summary>
    /// ユニット位置整理
    /// </summary>
    private void ResetUnitPosition()
    {
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            float delay = 360 / unit.Length;
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                unit[j].GetComponent<UnitC>().SetOfset(delay * j);
            }
        }
    }

    /// <summary>
    /// ユニット爆破
    /// </summary>
    private void DestroyAllUnit()
    {
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            float delay = 360 / unit.Length;
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().Apoptosis();
            }
        }

    }

    /// <summary>
    /// 足ジェット噴射
    /// </summary>
    private void JetPush()
    {
        //_audioGO.PlayOneShot(_seGun);
        for (int i = 0; i < 3; i++)
        {
            float angle = Random.Range(250, 290);
            Quaternion rot = transform.localRotation;
            ExpC shot = Instantiate(jet, new Vector3(_posOwn.x + Random.Range(-32, 32), _posOwn.y - 50, 0), rot);
            shot.EShot1(angle, 10, 0.3f);
        }
    }

    /// <summary>
    /// 移動時の急襲花火攻撃
    /// </summary>
    private void SkyFireWorkDown(Vector3 target)
    {

        for (int i = 0; i < 3; i++)
        {
            float angle = GameData.GetAngle(_posOwn, target) + Random.Range(-10, 10);
            Quaternion rot = transform.localRotation;
            Instantiate(FireworkP, _posOwn, rot).EShot1(angle, 36, -0.2f, Random.Range(18, 25), 20, 0.5f);
        }
        _audioGO.PlayOneShot(_seFire);
    }

    private void AllCoroutineStop()
    {
        StopAllCoroutines();
        _movingCoroutine = null;
    }

    /// <summary>
    /// 変形
    /// </summary>
    private IEnumerator Transformation(MODE_GUN mode)
    {
        if (_gunMode != mode)
        {
            _srOwn.sprite = _transfomation;
            yield return new WaitForSeconds(0.7f);
            if (!_isFullThrottle) {
                switch (mode)
                {
                    case MODE_GUN.Shining:
                        _srOwn.sprite = _beam;
                        break;
                    case MODE_GUN.Physical:
                        _srOwn.sprite = _bullet;
                        break;
                    case MODE_GUN.Heat:
                        _srOwn.sprite = _fire;
                        break;
                    case MODE_GUN.Crash:
                        _srOwn.sprite = _bomb;
                        break;
                }
            }
            else
            {
                switch (mode)
                {
                    case MODE_GUN.Shining:
                        _srOwn.sprite = _beam2;
                        break;
                    case MODE_GUN.Physical:
                        _srOwn.sprite = _bullet2;
                        break;
                    case MODE_GUN.Heat:
                        _srOwn.sprite = _fire2;
                        break;
                    case MODE_GUN.Crash:
                        _srOwn.sprite = _bomb2;
                        break;
                }
            }

            _gunMode = mode;
        }
    }

}
