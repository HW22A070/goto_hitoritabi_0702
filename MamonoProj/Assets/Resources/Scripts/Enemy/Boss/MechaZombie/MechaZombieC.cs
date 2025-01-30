using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaZombieC : MonoBehaviour
{
    //重力関係
    protected int judge;
    protected float down = 0;
    protected int pull, tate;

    protected bool _isDontDown = false;

    /// <summary>
    /// 下降中なう
    /// </summary>
    private bool _isflying;

    /// <summary>
    /// 空中
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
    /// 
    /// </summary>
    private int _powerJetDelta=0;

    private bool _isKaitenAble = true;

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


    private int i = 0;
    private int j = 0;
    private int _attackMode = 0;
    private float _damagePar = 100;
    private int firstHP = 0;

    private float angle, movex, movey;
    private GameObject GM;
    private Vector3 pos, ppos,_posShot, muki, velocity;
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
    private HenchmanC GCP;

    [SerializeField]
    private FireworkC FireworkP;

    private float kaber, kabel;

    [SerializeField]
    private UnitC _goUnitP;

    private ExhaustC _scOwnExh;

    private bool _isPositionLeft;

    /// <summary>
    /// 0=beam
    /// 1=bullet
    /// 2=fire
    /// 3=bomb
    /// </summary>
    private int _gunMode = 0;

    /// <summary>
    /// ガチモード
    /// </summary>
    private bool _isGatch;

    [SerializeField]
    private StaffRollC StaffPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip  _seMissile, _seGun, _seFire,_seEXP;

    private Vector3 hitPos;

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
        _scOwnExh = GetComponent<ExhaustC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");
        playerGO = GameObject.Find("Player");
        _srOwn = GetComponent<SpriteRenderer>();
        _eCoreC = GetComponent<ECoreC>();
        for (int j = 0; j < _eCoreC.hp.Length; j++)
        {
            firstHP += _eCoreC.hp[j];
        }
        _eCoreC.IsBoss = true;
        pos = transform.position;
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = firstHP;
        GM.GetComponent<GameManagement>()._bossMaxHp = firstHP;
    }

    // Update is called once per frame
    private void Update()
    {
        pos = transform.position;
        ppos = playerGO.transform.position;

        _posyFoot = pos.y - 72;
        if (_eCoreC.BossLifeMode != 2) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.TotalHp;
        _damagePar = _eCoreC.TotalHp * 100 / firstHP;

        if (_isKaitenAble)
        {
            if (pos.x < ppos.x)
            {
                _srOwn.flipX = true;
                _posShot = pos + transform.right * 70;
            }
            else
            {
                _srOwn.flipX = false;
                _posShot = pos - transform.right * 70;
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
            pos = transform.position;

            transform.position = new Vector3(pos.x
                , floor.transform.position.y + (floor.GetComponent<BoxCollider2D>().size.y / 2) + ((GetComponent<BoxCollider2D>().size.y - GetComponent<BoxCollider2D>().offset.y) / 2), 0);

            //重力ゼロ
            _gravityNow = 0;

            //炎上！
            _hitEnemyToFloor = Physics2D.Raycast(playerFootRay.origin, playerFootRay.direction, _posFoot.y, 8);
            if (_hitEnemyToFloor)
            {
                ECoreC eCore = GetComponent<ECoreC>();
                if (_hitEnemyToFloor.collider.gameObject.GetComponent<FloorC>()._floorMode == 3)
                {
                    //もし炎弱点であればダメージくらう
                    if (eCore.GetIsCritical(3) && eCore.TotalHp > 1) eCore.Damage(1, 3, _hitEnemyToFloor.collider.gameObject.transform.position);
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

        if (_isGatch)
        {
            Instantiate(_pefbVirusEF, GameData.RandomWindowPosition(), rot).EShot1(Random.Range(0,360),0.2f,2.0f);
        }



        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            transform.localPosition += new Vector3(-1, 0, 0);
            if (pos.x < 550)
            {
                _movingCoroutine = StartCoroutine(ActionBranch());
                _eCoreC.BossLifeMode = 4;
                for(int i=0;i<5;i++)SummonUnit();
            }
        }

        //DeathAction
        if (_eCoreC.BossLifeMode == 2)
        {
            DestroyAllUnit();
            FloorManagerC.StageGimic(100, 0);
            GameData.Star = true;
            GameData.TimerMoving = false;
            GM.GetComponent<GameManagement>()._bossNowHp = 0;
            FloorManagerC.StageGimic(100,0);
            if (GameData.Round == GameData.GoalRound)
            {
                Instantiate(StaffPrefab, new Vector3(320, -100, 0), transform.localRotation).Summon(0);
            }
            else
            {
                playerGO.GetComponent<PlayerC>().StageMoveAction();
            }
            Destroy(gameObject);
        }

        
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        _isKaitenAble = true;
        ResetUnitPosition();

        //ガチモード切替判定
        if (_eCoreC.EvoltionMode == 4)
        {
            if (!_isGatch)
            {
                _scOwnExh.enabled = true;
                _isGatch = true;
                //AllCoroutineStop();
                FloorManagerC.SetGimicBedRock(4);
                //_movingCoroutine = StartCoroutine(ActionBranch());
            }
        }

        if (!_isGatch)
        {
            StartCoroutine(Transformation(_eCoreC.EvoltionMode));
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
            

            switch (_eCoreC.EvoltionMode)
            {
                //ビームモード
                case 0:
                    _attackMode = Random.Range(0, 4);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(DoSummonUnit(2));
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(Move(true));
                    else if (_attackMode == 2) _movingCoroutine = StartCoroutine(Raser());
                    else if (_attackMode == 3) _movingCoroutine = StartCoroutine(IceBeam());
                    break;
                //バレットモード
                case 1:
                    _attackMode = Random.Range(0, 5);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(DoSummonUnit(2));
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(MachineGun());
                    else if (_attackMode == 2) _movingCoroutine = StartCoroutine(Move(true));
                    else if (_attackMode == 3) _movingCoroutine = StartCoroutine(ShotGun());
                    else if (_attackMode == 4) _movingCoroutine = StartCoroutine(RailGun());
                    break;
                //バーン
                case 2:
                    _attackMode = Random.Range(0, 4);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(DoSummonUnit(2));
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(Move(true));
                    else if (_attackMode == 2) _movingCoroutine = StartCoroutine(DropFire());
                    else if (_attackMode == 3) _movingCoroutine = StartCoroutine(Rolling());
                    break;
                //ボム
                case 3:
                    _attackMode = Random.Range(0, 4);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(DoSummonUnit(2));
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(Move(true));
                    else if (_attackMode == 2) _movingCoroutine = StartCoroutine(Rocket());
                    else if (_attackMode == 3) _movingCoroutine = StartCoroutine(RocketSniper());
                    break;
                default:
                    _movingCoroutine = StartCoroutine(Move(true));
                    break;
            }
        }
        //ガチ
        else
        {
            //ユニットいなかったらめちゃ召喚
            if (GameObject.FindGameObjectsWithTag("MechaUnit").Length <= 0)
            {
                for (int i = 0; i < Random.Range(6, 11); i++)
                {
                    SummonUnit();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                SummonUnit();
            }

            _attackMode = Random.Range(0, 5);
            switch (_attackMode)
            {
                //ビームモード
                case 0:
                    StartCoroutine(Transformation(_attackMode));
                    yield return new WaitForSeconds(0.7f);
                    _attackMode = Random.Range(0, 2);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(Raser());
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(IceBeam());
                    break;
                //バレットモード
                case 1:
                    StartCoroutine(Transformation(_attackMode));
                    yield return new WaitForSeconds(0.7f);
                    _attackMode = Random.Range(0, 3);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(MachineGun());
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(ShotGun());
                    else if (_attackMode == 2) _movingCoroutine = StartCoroutine(RailGun());
                    break;
                //バーン
                case 2:
                    StartCoroutine(Transformation(_attackMode));
                    yield return new WaitForSeconds(0.7f);
                    _attackMode = Random.Range(0, 2);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(Rolling());
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(DropFire());
                    break;
                //ボム
                case 3:
                    StartCoroutine(Transformation(_attackMode));
                    yield return new WaitForSeconds(0.7f);
                    _attackMode = Random.Range(0, 2);
                    if (_attackMode == 0) _movingCoroutine = StartCoroutine(Rocket());
                    else if (_attackMode == 1) _movingCoroutine = StartCoroutine(RocketSniper());
                    break;
                //召喚
                case 4:
                    _movingCoroutine = StartCoroutine(DoSummonUnit(Random.Range(1,4)));
                    break;
                //移動
                case 5:
                    _movingCoroutine = StartCoroutine(Move(true));
                    break;
                default:
                    _movingCoroutine = StartCoroutine(Move(true));
                    break;
            }
        }
        Debug.Log(_attackMode);
    }

    /// <summary>
    /// 衛生
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoSummonUnit(int value)
    {
        _isKaitenAble = true;
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
        _isKaitenAble = true;
        _isflying = true;
        _powerJet = 10;
        Vector3 target = ppos;
        //if (_isGatch) Instantiate(_prhbTargetEffect, target, rot).EShot1(0, 0, 1.7f);


        while (_posyFoot < 480)
        {
            yield return new WaitForSeconds(0.03f);
        }

        //if (_damagePar < 50) SkyFireWorkDown(target);

        _powerJet = 0;

        transform.position = new Vector3(ppos.x, pos.y, 0);

        //プレイヤーより下に行くまで噴射
        while (_posyFoot > ppos.y || _posyFoot > GameData.WindowSize.y - 192)
        {
            yield return new WaitForSeconds(0.03f);
        }
        _goCamera.GetComponent<CameraC>().StartShakeVertical(7, 6);
        _audioGO.PlayOneShot(_seEXP);
        
        _isPositionLeft = true;
        _isflying = false;
        if(isSeedActionBranch) _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// レーザー
    /// </summary>
    /// <returns></returns>
    private IEnumerator Raser()
    {
        _isKaitenAble = true;
        yield return new WaitForSeconds(0.3f);
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.RandomWindowPosition();
            targetpos = GameData.FixPosition(targetpos, 32, 32);
            unit[j].GetComponent<UnitC>().AttackRaser(GameData.RandomWindowPosition(), j * 0.09f, 0.6f+(unit.Length-j)*0.09f);
        }

        Instantiate(_prhbTargetEffect, ppos, Quaternion.Euler(0,0,0)).EShot1(0, 0, 1.3f);
        float angle = GameData.GetAngle(_posShot, ppos) + Random.Range(-1, 1);

        yield return new WaitForSeconds(0.9f+(unit.Length * 0.09f));

        Quaternion rot = transform.localRotation;
        _audioGO.PlayOneShot(_seGun);
        EMissile1C shot = Instantiate(_prfbRaserFat, _posShot, transform.rotation);
        shot.EShot1(angle, 0, 1000);
        shot.transform.position += shot.transform.up * 320;
        yield return new WaitForSeconds(1.5f);
        
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// 上からレーザー
    /// </summary>
    /// <returns></returns>
    private IEnumerator IceBeam()
    {
        _isKaitenAble = true;
        yield return new WaitForSeconds(0.3f);
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.RandomWindowPosition();
            targetpos = GameData.FixPosition(targetpos, 32, 32);
            unit[j].GetComponent<UnitC>().AttackIceBeam(0.6f);
        }


        yield return new WaitForSeconds(1.6f);

        StartCoroutine(Move(true));
        
        //_movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// マシンガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator MachineGun()
    {
        _isKaitenAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3  targetpos = GameData.FixPosition(GameData.RandomWindowPosition(), 32, 32);
            unit[j].GetComponent<UnitC>().AttackMachineGun(targetpos, j * 0.3f);
        }
        Instantiate(_prhbTargetEffect, ppos, rot).EShot1(0, 0, 1.3f);
        yield return new WaitForSeconds(0.3f + unit.Length * 0.3f);
        float angle = GameData.GetAngle(_posShot, ppos) + Random.Range(-1, 1);

        yield return new WaitForSeconds(0.6f);

        for (j = 0; j < 10; j++)
        {
            Quaternion rot = transform.localRotation;
            _audioGO.PlayOneShot(_seGun);
            Instantiate(Ballet1P, _posShot, rot).EShot1(angle, 30, 0);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(1.5f);
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// ショットガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShotGun()
    {
        _isKaitenAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            unit[j].GetComponent<UnitC>().AttackShotGun(0.6f, _srOwn.flipX);
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Move(true));
        
        //_movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// レールガン
    /// </summary>
    /// <returns></returns>
    private IEnumerator RailGun()
    {
        _isKaitenAble = false;
        yield return new WaitForSeconds(0.3f);
        Quaternion rot = transform.localRotation;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                Vector3 movePos = _posShot + ((ppos - _posShot) / 15 * j / 2);
                unit[j].GetComponent<UnitC>().AttackRailGun(movePos, 1.5f, _srOwn.flipX);
            }
        }

        Vector3 target = ppos;
        float angle = GameData.GetAngle(_posShot, target) + Random.Range(-1, 1);

        for(int k = 0; k < 18; k++)
        {
            Instantiate(_prhbTargetEffect, target, rot).EShot1(0, 0, 0.3f);
            yield return new WaitForSeconds(0.1f);
        }
        
        _audioGO.PlayOneShot(_seGun);
        _audioGO.PlayOneShot(_seEXP);
        _goCamera.GetComponent<CameraC>().StartShakeVertical(10, 10);
        for (int k = 0; k < unit.Length; k++)
        {
            _audioGO.PlayOneShot(_seGun);
            Instantiate(_prfbRailGunMissile, _posShot, rot).EShot1(angle, 100, 0);
        }
        Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, rot).EShot1(0, 0, 0.1f);

        yield return new WaitForSeconds(3.0f);

        _movingCoroutine = StartCoroutine("ActionBranch");
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
            StartCoroutine(Move(false));
            yield return new WaitForSeconds(7.0f);

        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    /// <summary>
    /// 落下炎
    /// </summary>
    /// <returns></returns>
    private IEnumerator DropFire()
    {
        _isKaitenAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            Vector3 targetpos = GameData.FixPosition(GameData.RandomWindowPosition(), 32, 32);
            unit[j].GetComponent<UnitC>().AttackDropFire(targetpos, j * 0.3f);
        }
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Move(true));

    }


    /// <summary>
    /// ロケット狙撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator RocketSniper()
    {
        Quaternion rot = transform.localRotation;
        _isKaitenAble = false;
        Vector3 target = ppos;
        float angle = GameData.GetAngle(_posShot, target) + Random.Range(-1, 1);
        Instantiate(_prhbTargetEffect, ppos, rot).EShot1(0, 0, 1.7f);


        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        if (unit.Length > 0)
        {
            for (int j = 0; j < unit.Length; j++)
            {
                unit[j].GetComponent<UnitC>().SetLeader(j == 0);
                Vector3 movePos = _posShot + ((ppos - _posShot) / 15 * j / 2);
                unit[j].GetComponent<UnitC>().AttackRockerSniper(0.9f, _srOwn.flipX,angle);
            }
        }

        yield return new WaitForSeconds(1.2f);
        StartCoroutine(Move(true));
    }




    /// <summary>
    /// ロケット
    /// </summary>
    /// <returns></returns>
    private IEnumerator Rocket()
    {
        _isKaitenAble = true;
        GameObject[] unit = GameObject.FindGameObjectsWithTag("MechaUnit");
        for (int j = 0; j < unit.Length; j++)
        {
            unit[j].GetComponent<UnitC>().SetLeader(j == 0);
            float ofset = (600 / unit.Length) * j;
            unit[j].GetComponent<UnitC>().AttackRocket(ofset, j * 0.21f);
        }
        yield return new WaitForSeconds(1.0f + unit.Length * 0.21f);

        kaber = GameData.GetAngle(_posShot, new Vector3(50, 460, 0));
        kabel = GameData.GetAngle(_posShot, new Vector3(590, 460, 0));

        for (j = 0; j < 20; j++)
        {
            float angle = Random.Range(kabel, kaber);
            Quaternion rot = transform.localRotation;
            _audioGO.PlayOneShot(_seMissile);
            Instantiate(RocketPrefab, _posShot, rot).EShot1(angle, 15, 300, 5, 0.5f, Random.Range(20, 30));
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(1.5f);
        _movingCoroutine = StartCoroutine("ActionBranch");
    }


    /// <summary>
    /// ユニット召喚
    /// </summary>
    private void SummonUnit()
    {
        Instantiate(_goUnitP, pos, Quaternion.Euler(0, 0, 0)).SetParent(gameObject);
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
        for (i = 0; i < 3; i++)
        {
            float angle = Random.Range(250, 290);
            Quaternion rot = transform.localRotation;
            ExpC shot = Instantiate(jet, new Vector3(pos.x + Random.Range(-32, 32), pos.y - 50, 0), rot);
            shot.EShot1(angle, 10, 0.3f);
        }
    }

    /// <summary>
    /// 移動時の急襲花火攻撃
    /// </summary>
    private void SkyFireWorkDown(Vector3 target)
    {

        for (i = 0; i < 3; i++)
        {
            float angle = GameData.GetAngle(pos, target) + Random.Range(-10, 10);
            Quaternion rot = transform.localRotation;
            Instantiate(FireworkP, pos, rot).EShot1(angle, 36, -0.2f, Random.Range(18, 25), 20, 0.5f);
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
    private IEnumerator Transformation(int mode)
    {
        if (_gunMode != mode)
        {
            _srOwn.sprite = _transfomation;
            yield return new WaitForSeconds(0.7f);
            if (!_isGatch) {
                switch (mode)
                {
                    case 0:
                        _srOwn.sprite = _beam;
                        break;
                    case 1:
                        _srOwn.sprite = _bullet;
                        break;
                    case 2:
                        _srOwn.sprite = _fire;
                        break;
                    case 3:
                        _srOwn.sprite = _bomb;
                        break;
                }
            }
            else
            {
                switch (mode)
                {
                    case 0:
                        _srOwn.sprite = _beam2;
                        break;
                    case 1:
                        _srOwn.sprite = _bullet2;
                        break;
                    case 2:
                        _srOwn.sprite = _fire2;
                        break;
                    case 3:
                        _srOwn.sprite = _bomb2;
                        break;
                }
            }

            _gunMode = mode;
        }
    }

}
