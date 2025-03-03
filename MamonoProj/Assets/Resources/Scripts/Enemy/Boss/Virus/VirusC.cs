using EnumDic.Enemy;
using EnumDic.Enemy.Virus;
using EnumDic.System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VirusC : MonoBehaviour
{
    private int i = 0;
    private int j = 0;
    private int _deathActionTime = 0;
    
    private ATTACK_VIRUS1 _beforeAction1;
    
    private ATTACK_VIRUS2 _beforeAction2;
    
    private ATTACK_VIRUS3 _beforeAction3;

    /// <summary>
    /// クールタイムのチャージ時間
    /// </summary>
    private int changetime = 0;


    /// <summary>
    /// 0=第一形態クール
    /// 1=第二形態クール
    /// 2=第３形態
    /// </summary>
    private int _virusmode = 0;

    private float _angle;
    private Vector3 _posOwn, _posPlayer, shutu, muzzle;

    [SerializeField]
    private SpriteRenderer mine;

    [SerializeField]
    private Sprite[] _spritesType1, _spritesType2, _spritesLast;

    /// <summary>
    /// 隙晒しまでのカウンター
    /// </summary>
    private int _countBlood = 0;


    [SerializeField]
    private HealC HealP, StarP;

    [SerializeField]
    private ExpC Virus2E, VirusE, BeamE;

    [SerializeField]
    private GuardC tamaP;

    [SerializeField]
    private EMissile1C MissileP, CubeP;

    [SerializeField]
    private FireworkC FireworkP;

    [SerializeField]
    [Tooltip("爆弾")]
    private BombC _meteorP;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;


    [SerializeField]
    private AudioClip damageS, deadS, heavyS, beamS, magicS, shineS, expS, chargeS;

    [SerializeField]
    private short[] BeamD , BulletD , FireD , BombD, ExpD;

    private PMCoreC playerMissileP;

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool[] _isBeamCritical, _isBulletCritical, _isFireCritical, _isBombCritical;

    private PlayerC _scPlayer;
    private GameObject PlayerGO;

    private Quaternion _rotOwn;

    /// <summary>
    /// 動作中のコルーチン
    /// </summary>
    private Coroutine _movingCoroutine;

    /// <summary>
    /// ECoreCのコンポーネント
    /// </summary>
    private ECoreC _eCoreC;
    private GameManagement _gameManaC;

    /// <summary>
    /// カメラオブジェクト
    /// </summary>
    private GameObject _goCamera;

    private AudioControlC _bgmManager;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(480, 270, true);

        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();

        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");
        GameData.PlayerMoveAble = 3;

        _eCoreC = GetComponent<ECoreC>();
        _posOwn = transform.position;
        PlayerGO = GameObject.Find("Player");
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        _gameManaC._bossNowHp = _eCoreC.hp[1];
        _gameManaC._bossMaxHp = _eCoreC.hp[1];
        _eCoreC.IsBoss = true;
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;
        _posPlayer = PlayerGO.transform.position;
        _rotOwn = transform.localRotation;
        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead) _gameManaC._bossNowHp = _eCoreC.hp[_eCoreC.EvoltionMode];

    }

    void FixedUpdate()
    {
        
        //出現行動
        if (_eCoreC.BossLifeMode == 0)
        {
            _bgmManager.ChangeAudio(101, false, 0.8f);

            //牽制
            for (int i = 0; i < 200; i++)
            {
                Vector3 posVirusDust;
                do
                {
                    posVirusDust = new Vector3(Random.Range(0, 640), Random.Range(0, 530), 0);
                }
                while ((_posPlayer.x + 64 > posVirusDust.x && posVirusDust.x > _posPlayer.x - 64) && (_posPlayer.y + 64 > posVirusDust.y && posVirusDust.y > _posPlayer.y - 64));

                float sho = Random.Range(0, 360);
                ExpC shot2 = Instantiate(Virus2E, posVirusDust, _rotOwn);
                shot2.EShot1(sho, Random.Range(-0.2f, 0.2f), 1);
            }

            GameData.VirusBugEffectLevel = MODE_VIRUS.Little;
            _eCoreC.BossLifeMode = MODE_LIFE.Fight;
            _movingCoroutine = StartCoroutine(ActionBranch());
        }
        
        //形態変化1->2
        if (_virusmode==0&&_eCoreC.hp[1]<=0)
        {
            AllCoroutineStop();
            _movingCoroutine = StartCoroutine(Evoltion1());
        }

        //形態変化2->3
        if (_virusmode == 1 && _eCoreC.hp[2] <= 0)
        {
            AllCoroutineStop();
            _movingCoroutine = StartCoroutine(Evoltion2());
        }


        if(GameData.VirusBugEffectLevel != MODE_VIRUS.None)
        {
            TextBug();
        }

        //DeathAction
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;
            _gameManaC._bossNowHp=0;
            AllCoroutineStop();
            GameData.VirusBugEffectLevel = MODE_VIRUS.None;
            if (_deathActionTime == 1)
            {
                TimeManager.ChangeTimeValue(1.0f);
            }
            _deathActionTime++;
            if (_deathActionTime <= 100)
            {
                _angle = Random.Range(0, 360);
                ExpC cosgun = Instantiate(Virus2E, _posOwn, _rotOwn);
                cosgun.EShot1(_angle, 20, 0.3f);
            }
            if (_deathActionTime > 100)
            {
                _angle = Random.Range(0, 360);
                ExpC cosgun = Instantiate(Virus2E, _posOwn, _rotOwn);
                cosgun.EShot1(_angle, 20, 0.3f);
                gameObject.transform.localScale -= new Vector3(0.01f, 0.01f, 0);
            }
            if (_deathActionTime > 200)
            {
                GameData.Point += 100000;
                ClearC.BossBonus += 10;
                Destroy(gameObject);
            }

        }

        //見た目変化
        if (_virusmode<=1)
        {
            int looks = Random.Range(0, _spritesType1.Length);
            mine.sprite = _spritesType1[looks];
        }
        else if (_virusmode==2)
        {
            int looks = Random.Range(0, _spritesLast.Length);
            mine.sprite = _spritesLast[looks];
        }
        if (changetime > 100)
        {
            int looks = Random.Range(0, _spritesType1.Length);
            mine.sprite = _spritesType1[looks];
        }


        if (_eCoreC.EvoltionMode == 0)
        {
            //Barrier
            i = Random.Range(0, 12) * 30;
            float angle = GameData.GetAngle(transform.position,_posPlayer);
            shutu = _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(i * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, _rotOwn);
            barrier.EShot1(angle, 1, 0.9f);

            //Effect
            angle = Random.Range(0, 360);
            shutu = new Vector3(Random.Range(-160, 800), Random.Range(0, 530),0) ;
            ExpC effect = Instantiate(VirusE, shutu, _rotOwn);
            effect.EShot1(angle, 0.3f, 4);
        }

        //hpbug
        if (_eCoreC.EvoltionMode == 0)
        {
            if (_virusmode == 0) _eCoreC.hp[0] = Random.Range(0, 42000);
            else if (_virusmode == 1) _eCoreC.hp[0] = Random.Range(0, 42000);
        }
        else _eCoreC.hp[0] = 1;




        //Roll
        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead)transform.localEulerAngles += new Vector3(0, 0, 4);
        else transform.localEulerAngles += new Vector3(0, 0, 0);

        
    }

    private enum ATTACK_VIRUS1
    {
        MathVirus,
        EruptionBeam,
        Cube
    }

    private enum ATTACK_VIRUS2
    {
        MathVirus,
        FireWork,
        Cube,
        Meteor
    }

    private enum ATTACK_VIRUS3
    {
        MathVirus,
        EruptionBeam,
        FireWork,
        Cube2,
        Meteor,
        Bloodbeam
    }



    /// <summary>
    /// 行動分岐
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionBranch()
    {

        switch (_virusmode)
        {
            case 0:
                if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) RandomScreenWipe(2, 5);

                yield return new WaitForSeconds(1.0f);
                GameData.PlayerMoveAble = 3;
                _eCoreC.EvoltionMode = 0;
                if (_countBlood >= 4)
                {
                    _countBlood = 0;
                    _movingCoroutine = StartCoroutine(BloodBeam1());
                }
                else
                {
                    _countBlood++;

                    ATTACK_VIRUS1 attackVariation1;
                    do
                    {
                        attackVariation1 = (ATTACK_VIRUS1)Random.Range(0, System.Enum.GetNames(typeof( ATTACK_VIRUS1)).Length);
                    } while (attackVariation1 == _beforeAction1);

                    _beforeAction1=attackVariation1;

                    switch (_beforeAction1)
                    {
                        case ATTACK_VIRUS1.MathVirus:
                            _movingCoroutine = StartCoroutine(MathBeam(10));
                            break;

                        case ATTACK_VIRUS1.EruptionBeam:
                            _movingCoroutine = StartCoroutine(EruptionBeam());
                            break;

                        case ATTACK_VIRUS1.Cube:
                            _movingCoroutine = StartCoroutine(Cube1());
                            break;
                    }
                }
                break;

            case 1:
                if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) RandomScreenWipe(3, 7);
                yield return new WaitForSeconds(0.7f);
                GameData.PlayerMoveAble = 3;
                _eCoreC.EvoltionMode = 0;

                if (_countBlood >= 6)
                {
                    _countBlood = 0;
                    _movingCoroutine = StartCoroutine(BloodBeam2());
                }
                else
                {
                    _countBlood++;

                    ATTACK_VIRUS2 attackVariation2;
                    do
                    {
                        attackVariation2 = (ATTACK_VIRUS2)Random.Range(0, System.Enum.GetNames(typeof(ATTACK_VIRUS2)).Length);
                    } while (attackVariation2 == _beforeAction2);

                    _beforeAction2 = attackVariation2;

                    switch (_beforeAction2)
                    {
                        case ATTACK_VIRUS2.MathVirus:
                            _movingCoroutine = StartCoroutine(MathBeam(9));
                            break;

                        case ATTACK_VIRUS2.FireWork:
                            _movingCoroutine = StartCoroutine(FireWork());
                            break;

                        case ATTACK_VIRUS2.Cube:
                            _movingCoroutine = StartCoroutine(Cube1());
                            break;

                        case ATTACK_VIRUS2.Meteor:
                            _movingCoroutine = StartCoroutine(Meteor());
                            break;
                    }
                }
                break;

            case 2:
                if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(GameData.FirstWidth, GameData.FirstHeight, true);
                GameData.PlayerMoveAble = 6;
                yield return new WaitForSeconds(0.03f);

                ATTACK_VIRUS3 attackVariation3;
                do
                {
                    attackVariation3 = (ATTACK_VIRUS3)Random.Range(0, System.Enum.GetNames(typeof(ATTACK_VIRUS3)).Length);
                } while (attackVariation3 == _beforeAction3);

                _beforeAction3 = attackVariation3;

                switch (_beforeAction3)
                {
                    case ATTACK_VIRUS3.MathVirus:
                        _movingCoroutine = StartCoroutine(MathBeam(7));
                        break;

                    case ATTACK_VIRUS3.FireWork:
                        _movingCoroutine = StartCoroutine(FireWork());
                        break;

                    case ATTACK_VIRUS3.Cube2:
                        _movingCoroutine = StartCoroutine(CubeSpeedUp());
                        break;

                    case ATTACK_VIRUS3.Meteor:
                        _movingCoroutine = StartCoroutine(Meteor());
                        break;

                    case ATTACK_VIRUS3.Bloodbeam:
                        _movingCoroutine = StartCoroutine(BloodBeam3());
                        break;

                    case ATTACK_VIRUS3.EruptionBeam:
                        _movingCoroutine = StartCoroutine(EruptionBeam());
                        break;
                }
                break;
        }
    }    

    /// <summary>
    /// 噴火ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator EruptionBeam()
    {

        muzzle = new Vector3(Random.Range(20, 620), 0, 0);
        float angle = Random.Range(45, 135);

        for (int j = 0; j < 30; j++)
        {
            if (Random.Range(0, 2) == 0)
            {
                ExpC shot = Instantiate(BeamE, muzzle, _rotOwn);
                shot.EShot1(angle, 100, 1);
                _audioGO.PlayOneShot(beamS);
                _audioGO.PlayOneShot(expS);
                yield return new WaitForSeconds(0.03f);
            }
        }
        yield return new WaitForSeconds(0.15f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    private enum TYPE_MATHBEAM
    {
        Sin,
        Sin2,
        Cos,
        Tan,
    }

    /// <summary>
    /// サインコサインタンジェント
    /// </summary>
    /// <returns></returns>
    private IEnumerator MathBeam(int speed)
    {
        TYPE_MATHBEAM sct = (TYPE_MATHBEAM)Random.Range(0, 4);
        _audioGO.PlayOneShot(magicS);
        for (float f6 = 0; f6 < 100; f6++)
        {
            switch (sct)
            {
                case TYPE_MATHBEAM.Sin:
                    for (float fi = 1; fi < f6; fi++)
                    {
                        shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Sin(fi / (3 + ((100 - f6) / speed))) * 220, 0);
                        ExpC singun = Instantiate(Virus2E, shutu, _rotOwn);
                        singun.EShot1(0, 0, 0.1f);
                        shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Sin(-fi / (3 + ((100 - f6) / speed))) * 220, 0);
                        ExpC cosgun = Instantiate(Virus2E, shutu, _rotOwn);
                        cosgun.EShot1(0, 0, 0.1f);
                    }
                    break;

                case TYPE_MATHBEAM.Sin2:
                    for (float fi = 1; fi < f6; fi++)
                    {
                        shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Cos(fi / (3 + ((100 - f6) / speed))) * 220, 0);
                        ExpC singun = Instantiate(Virus2E, shutu, _rotOwn);
                        singun.EShot1(0, 0, 0.1f);
                        shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Cos(-fi / (3 + ((100 - f6) / speed))) * 220, 0);
                        ExpC cosgun = Instantiate(Virus2E, shutu, _rotOwn);
                        cosgun.EShot1(0, 0, 0.05f);
                    }
                    break;

                case TYPE_MATHBEAM.Cos:
                    for (float fi = 1; fi < f6; fi++)
                    {
                        shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Tan(fi / (3 + ((100 - f6) / (speed * 1.5f)))) * 220, 0);
                        ExpC singun = Instantiate(Virus2E, shutu, _rotOwn);
                        singun.EShot1(0, 0, 0.1f);
                        shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Tan(-fi / (3 + ((100 - f6) / (speed * 1.5f)))) * 220, 0);
                        ExpC cosgun = Instantiate(Virus2E, shutu, _rotOwn);
                        cosgun.EShot1(0, 0, 0.05f);
                    }
                    break;

                case TYPE_MATHBEAM.Tan:
                    for (float fi = 1; fi < f6; fi++)
                    {
                        shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 5) + (f6 / speed)) * 220, 0);
                        ExpC singun = Instantiate(Virus2E, shutu, _rotOwn);
                        singun.EShot1(0, 0, 0.05f);
                        shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 5) - (f6 / speed)) * 220, 0);
                        ExpC cosgun = Instantiate(Virus2E, shutu, _rotOwn);
                        cosgun.EShot1(0, 0, 0.05f);
                    }
                    break;
            }
            if(Random.Range(0,2)==0)_audioGO.PlayOneShot(shineS);
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 大血ビーム１
    /// </summary>
    /// <returns></returns>
    private IEnumerator BloodBeam1()
    {
        if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(384, 216, true);
        GameData.PlayerMoveAble = 6;
        _eCoreC.EvoltionMode = 1;
        transform.localPosition = new Vector3(Random.Range(50, 590), 120, 0);
        GameData.VirusBugEffectLevel = MODE_VIRUS.FullThrottle1;
        _posOwn = transform.position;
        float angle = GameData.GetAngle(_posOwn, _posPlayer);
        TimeManager.ChangeTimeValue(0.1f);
        for (int j = 0; j < 29; j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                ExpC shot2 = Instantiate(VirusE, tar, _rotOwn);
                shot2.EShot1(GameData.GetAngle(tar, _posOwn), 5, 0.1f);
            }
            _audioGO.PlayOneShot(chargeS);
            TimeManager.ChangeTimeValue(0.1f+(j*0.03f));
            yield return new WaitForSeconds(0.03f);
        }
        TimeManager.ChangeTimeValue(1.0f);
        for (int j = 0; j < 170; j++)
        {
            if (_eCoreC.hp[1] > 50)
            {
                for (i = 0; i < 10; i++)
                {
                    _rotOwn.z = Random.Range(0, 360);
                    Instantiate(BeamE, _posOwn, _rotOwn).EShot1(angle + Random.Range(-20, 20), Random.Range(50, 135), 0.4f);
                }
                _audioGO.PlayOneShot(damageS);
                _audioGO.PlayOneShot(heavyS);
                PlayerGO.GetComponent<PlayerC>().VibrationCritical();
                _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 1);
                _eCoreC.hp[1] -= 50;
                yield return new WaitForSeconds(0.03f);
            }
        }
        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        for (int j = 0; j < 400; j++)
        {
            if (Random.Range(0, 100) == 0)
            {
                HealSummon();
            }
            
            if (j > 300) GameData.VirusBugEffectLevel = MODE_VIRUS.Little;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.03f);
        _eCoreC.EvoltionMode = 0;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 形態変化1->2
    /// </summary>
    /// <returns></returns>
    private IEnumerator Evoltion1()
    {
        _bgmManager.VolumefeedInOut(1.8f, 0);
        _virusmode = 1;
        yield return new WaitForSeconds(0.03f);
        for (int j = 0; j < 2; j++)
        {
            muzzle = new Vector3(Random.Range(20, 620), 0, 0);
            _angle = Random.Range(70, 110);
            Instantiate(FireworkP, muzzle, _rotOwn).EShot1(_angle, Random.Range(18, 25), -1.0f, Random.Range(10, 16), 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.5f);
        }
        for (int j = 0; j < 9; j++)
        {
            muzzle = new Vector3(320, 0, 0);
            _angle = 45 + (j * 10);
            Instantiate(FireworkP, muzzle, _rotOwn).EShot1(_angle, 22, -1.0f, 20, 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.09f);
        }
        yield return new WaitForSeconds(0.03f);
        _eCoreC.EvoltionMode = 0;
        Vector3 direction = new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0);
        Instantiate(StarP, direction, _rotOwn);
        for (i = 0; i < 3; i++)
        {
            HealSummon();
        }
        _bgmManager.ChangeAudio(102, false, 0.8f);
        GameData.VirusBugEffectLevel = MODE_VIRUS.Medium;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 花火
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWork()
    {
        for(int j=0;j<30;j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                float angle2 = GameData.GetAngle(tar,_posOwn);
                Instantiate(VirusE, tar, _rotOwn).EShot1(angle2, 10, 0.1f);
            }
            yield return new WaitForSeconds(0.03f);
        }
        for(int j = 0; j < 2; j++)
        {
            muzzle = new Vector3(Random.Range(20, 620), 0, 0);
            _angle = Random.Range(70, 110);
            Instantiate(FireworkP, muzzle, _rotOwn).EShot1(_angle, Random.Range(18, 25), -1.0f, Random.Range(18, 25), 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.5f);
        }
        for (int j = 0; j < 9; j++)
        {
            muzzle = new Vector3(320, 0, 0);
            _angle = 45 + (j * 10);
            Instantiate(FireworkP, muzzle, _rotOwn).EShot1(_angle, 22, -1.0f, 20, 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.09f);
        }
        yield return new WaitForSeconds(0.03f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 並走キューブ
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cube1()
    {
        _audioGO.PlayOneShot(magicS);
        for(int k=0;k<10;k++)
        {
            float angle = 180;

            j = Random.Range(0, 5);
            muzzle = new Vector3(1000, (j * 90) + 6 + 48, 0);
            EMissile1C shot = Instantiate(CubeP, muzzle, _rotOwn);
            shot.EShot1(angle, (j % 2 + 1) * 7, 0);
            yield return new WaitForSeconds(0.5f);

        }
        yield return new WaitForSeconds(0.03f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 隕石
    /// </summary>
    /// <returns></returns>
    private IEnumerator Meteor()
    {
        float angle = Random.Range(260, 280);
        Instantiate(_meteorP, new Vector3(360, 650, 0), _rotOwn).EShot1(angle, 0, 0.1f, 100, 30, 3.0f);

        for(int i = 0; i < 5; i++)
        {
            angle = Random.Range(260, 280);
            _audioGO.PlayOneShot(shineS);
            Instantiate(_meteorP, new Vector3(GameData.GetRandomWindowPosition().x, GameData.WindowSize.y+64, 0), _rotOwn).EShot1(angle, 30, 0.1f, 100, 5, 0.6f);
            yield return new WaitForSeconds(0.3f);
        }
        
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 大血ビーム２
    /// </summary>
    /// <returns></returns>
    private IEnumerator BloodBeam2()
    {
        if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(96, 54, true);
        GameData.PlayerMoveAble = 6;
        _eCoreC.EvoltionMode = 2;
        transform.localPosition = new Vector3(Random.Range(50, 590), 210, 0);
        GameData.VirusBugEffectLevel = MODE_VIRUS.FullThrottle2;

        yield return new WaitForSeconds(0.6f);
        TimeManager.ChangeTimeValue(0.1f);

        for (int j = 0; j < 30; j++)
        {
            TimeManager.ChangeTimeValue(0.1f+(j*0.03f));
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.03f);
        }

        TimeManager.ChangeTimeValue(1.0f);
        float angle = GameData.GetAngle(_posOwn, _posPlayer);
        for (int j = 0; j < 200; j++)
        {
            if (_eCoreC.hp[2] > 50)
            {
                for (i = 0; i < 30; i++)
                {
                    _rotOwn.z = Random.Range(0, 360);
                    ExpC shot2 = Instantiate(BeamE, _posOwn, _rotOwn);
                    shot2.EShot1(angle + Random.Range(30, 330), Random.Range(50, 135), 0.4f);
                }
                angle += Random.Range(-2, 2);
                _audioGO.PlayOneShot(damageS);
                _audioGO.PlayOneShot(heavyS);
                _scPlayer = PlayerGO.GetComponent<PlayerC>();
                _scPlayer.VibrationCritical();
                _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 1);
                _eCoreC.hp[2] -= 1;
            }
            yield return new WaitForSeconds(0.03f);
        }

        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        for (int j = 0; j < 340; j++)
        {
            if (Random.Range(0, 100) == 0)
            {
                HealSummon();
            }
            if (j > 240) GameData.VirusBugEffectLevel = MODE_VIRUS.FullThrottle2;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.03f);
        _eCoreC.EvoltionMode = 0;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 形態変化2->3
    /// </summary>
    /// <returns></returns>
    private IEnumerator Evoltion2()
    {
        _bgmManager.VolumefeedInOut(2.5f, 0);
        _virusmode = 2;
        _gameManaC._bossMaxHp = _eCoreC.hp[3];
        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        for (int j = 0; j < 100; j++)
        {
            i = Random.Range(0, 12) * 30;
            float angle = GameData.GetAngle(transform.position,_posPlayer);
            shutu = _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(i * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, _rotOwn);
            barrier.EShot1(angle, 1, 0.9f);
            yield return new WaitForSeconds(0.03f);
        }
        _bgmManager.ChangeAudio(103, false, 0.8f);
        for (j = 0; j < 30; j++)
        {
            i = Random.Range(0, 12) * 30;
            float angle = Random.Range(0, 360);
            shutu = _posOwn + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(i * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, _rotOwn);
            barrier.EShot1(angle, 10, 0.9f);
        }

        yield return new WaitForSeconds(3.00f);
        _eCoreC.EvoltionMode = 3;
        Vector3 direction = new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0);
        Instantiate(StarP, direction, _rotOwn);
        for (i = 0; i < 3; i++)
        {
            HealSummon();
        }
        GameData.VirusBugEffectLevel = MODE_VIRUS.Large;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// すれ違いキューブ
    /// </summary>
    /// <returns></returns>
    private IEnumerator CubeSpeedUp()
    {
        TimeManager.ChangeTimeValue(1.0f);
        _audioGO.PlayOneShot(magicS);
        for(int k=0;k<30;k++)
        {
            float angle = 180;
            j = Random.Range(0, 5);
            muzzle = new Vector3((j % 2 * 1200)-200, (j * 90) + 6 + 48, 0);
            Instantiate(CubeP, muzzle, _rotOwn).EShot1(angle, ((j % 2 * 2) - 1) * 9, 0);
            TimeManager.ChangeTimeValue(1.0f+(k*0.03f));
            yield return new WaitForSeconds(0.21f);
        }
        TimeManager.ChangeTimeValue(1.0f);
        yield return new WaitForSeconds(0.03f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 即席大血ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator BloodBeam3()
    {
        if(GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(384, 216, true);
        GameData.VirusBugEffectLevel = MODE_VIRUS.Medium;
        transform.localPosition = new Vector3(Random.Range(50, 590), 120, 0);
        float angle = GameData.GetAngle(transform.position, _posPlayer);
        TimeManager.ChangeTimeValue(0.4f);

        for (int j=0;j<30;j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                ExpC shot2 = Instantiate(VirusE, tar, _rotOwn);
                shot2.EShot1(GameData.GetAngle(tar, _posOwn), 10, 0.1f);
            }
            TimeManager.ChangeTimeValue(0.4f+(j*0.02f));
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.03f);
        }

        TimeManager.ChangeTimeValue(1.0f);
        for (int j=0;j<70;j++)
        {
            for (i = 0; i < 10; i++)
            {
               
                _rotOwn.z = Random.Range(0, 360);
                ExpC shot2 = Instantiate(BeamE, _posOwn, _rotOwn);
                shot2.EShot1(angle + Random.Range(-20, 20), Random.Range(50, 135), 0.4f);
            }
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(beamS);
            _scPlayer = PlayerGO.GetComponent<PlayerC>();
            _scPlayer.VibrationCritical();
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 1);
            yield return new WaitForSeconds(0.03f);
        }

        GameData.VirusBugEffectLevel = MODE_VIRUS.Large;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// テキストのランダム生成
    /// </summary>
    private void TextBug()
    {
        string base_string = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ%&!$#";
        int length=Random.Range(5,20);
        char[] random_chars = new char[length];



        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");
        for(int k = 0; k < texts.Length; k++)
        {
            for (int i = 0; i < length; i++)
            {
                int char_index = Random.Range(0, base_string.Length);

                random_chars[i] = base_string[char_index];
            }

            string result = new string(random_chars);
            texts[k].GetComponent<Text>().text = result;
        }
    }

    /// <summary>
    /// 画面のサイズをむりやり変更
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    private void RandomScreenWipe(int min,int max)
    {
        int hoge = Random.Range(min, max+1);
        Screen.SetResolution(GameData.FirstWidth / hoge, GameData.FirstHeight / hoge, true);
    }

    /// <summary>
    /// 回復を生成
    /// </summary>
    private void HealSummon()
    {
        Instantiate(HealP, new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0), transform.localRotation);
    }

    private void AllCoroutineStop()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }

    }

    public int GetVirusMode()
    {
        return _virusmode;
    }
}
