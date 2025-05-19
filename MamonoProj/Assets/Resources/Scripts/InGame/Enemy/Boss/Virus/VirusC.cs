using EnumDic.Enemy;
using EnumDic.Enemy.Virus;
using EnumDic.System;
using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusC : BossCoreC
{
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
    private Vector3 shutu, muzzle;

    [SerializeField]
    private Sprite[] _spritesType1, _spritesType2, _spritesLast;
    [SerializeField]
    private Sprite _sp5100;

    /// <summary>
    /// 隙晒しまでのカウンター
    /// </summary>
    private int _countBlood = 0;


    [SerializeField]
    private HealC HealP, StarP;

    [SerializeField]
    private ExpC Virus2E, VirusE,_prfbVirusPink, BeamE;

    [SerializeField]
    private EMissile1C MissileP, CubeP;

    [SerializeField]
    private FireworkC FireworkP;

    [SerializeField]
    [Tooltip("爆弾")]
    private HowitzerC _meteorP;


    [SerializeField]
    private AudioClip damageS, deadS, heavyS, beamS, magicS, shineS, expS, chargeS;

    [SerializeField]
    private short[] BeamD , BulletD , FireD , BombD, ExpD;

    private PMCoreC playerMissileP;

    [SerializeField]
    private VirusTextsC _textsVirus;

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool[] _isBeamCritical, _isBulletCritical, _isFireCritical, _isBombCritical;

    private AudioControlC _bgmManager;

    // Start is called before the first frame update
    new void Start()
    {
        _bgmManager = GameObject.Find("BGMManager").GetComponent<AudioControlC>();
        base.Start();
        Screen.SetResolution(480, 270, true);
        _textsVirus = Instantiate(_textsVirus, Vector3.zero, Quaternion.Euler(0, 0, 0));
    }

    protected override void FxUpDead()
    {
        if (_eCoreC.CheckIsAlive())
        {
            _eCoreC.SetIsAlive(false);
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;
            _gameManaC._bossNowHp = 0;
            AllCoroutineStop();
            GameData.VirusBugEffectLevel = MODE_VIRUS.None;
            StartCoroutine(DeadAction());
        }

    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        //形態変化1->2
        if (_virusmode==0&&_eCoreC.hp[1]<=0)
        {
            _srOwnBody.enabled = true;
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

        //見た目変化
        int numberSprite = 0;

        switch (_virusmode)
        {
            case 0:
                //エフェクトで隠れる
                int barrier = Random.Range(0, 360);
                shutu = _posOwn + new Vector3(Mathf.Cos(barrier * Mathf.Deg2Rad) * Random.Range(0,32), Mathf.Sin(barrier * Mathf.Deg2Rad) * Random.Range(0, 32), 0);
                ExpC effect = Instantiate(_prfbVirusPink, shutu, _rotOwn);
                effect.ShotEXP(Random.Range(0,360), 0.3f, 0.9f);
                float scales= Random.Range(0.5f, 1.5f);
                effect.transform.localScale = new Vector3(scales, scales, scales);
                break;

            case 1:
                if (Random.Range(0, 10000) == 0 && Random.Range(0, 50000) == 0) _srOwnBody.sprite = _sp5100;
                else
                {
                    numberSprite = Random.Range(0, _spritesType1.Length);
                    _srOwnBody.sprite = _spritesType1[numberSprite];
                }

                break;

            case 2:
                if (Random.Range(0, 10000) == 0 && Random.Range(0, 50000) == 0) _srOwnBody.sprite = _sp5100;
                else {
                    numberSprite = Random.Range(0, _spritesLast.Length);
                    _srOwnBody.sprite = _spritesLast[numberSprite];
                }
                break;
        }


        if (changetime > 100)
        {
            numberSprite = Random.Range(0, _spritesType1.Length);
            _srOwnBody.sprite = _spritesType1[numberSprite];
        }


        if (_eCoreC.EvoltionMode == 0)
        {
            //Barrier
            int barrier = Random.Range(0, 12) * 30;
            float angle = Moving2DSystems.GetAngle(transform.position,_posPlayer);
            shutu = _posOwn + new Vector3(Mathf.Cos(barrier * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(barrier * Mathf.Deg2Rad) * 8 * 5, 0);
            Instantiate(Virus2E, shutu, _rotOwn)
                .ShotEXP(angle, 1, 0.9f);

            //Effect
            angle = Random.Range(0, 360);
            shutu = new Vector3(Random.Range(-160, 800), Random.Range(0, 530),0) ;
            Instantiate(VirusE, shutu, _rotOwn).ShotEXP(angle, 0.3f, 4);
        }

        //hpbug
        if (_eCoreC.EvoltionMode == 0)
        {
            if (_virusmode == 0) _eCoreC.hp[0] = Random.Range(0, 42000);
            else if (_virusmode == 1) _eCoreC.hp[0] = Random.Range(0, 42000);
        }
        else _eCoreC.hp[0] = 1;




        //Roll
        if (_eCoreC.GetModeBossLife() != MODE_LIFE.Dead)_tfOwnBody.eulerAngles += new Vector3(0, 0, 4);
        else _tfOwnBody.eulerAngles += new Vector3(0, 0, 0);

        
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
    protected override IEnumerator ActionBranch()
    {
        ChangeTarget();
        switch (_virusmode)
        {
            //第1形態
            case 0:
                if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) RandomScreenWipe(2, 5);

                yield return new WaitForSeconds(1.0f);
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
                            _movingCoroutine = StartCoroutine(MathBeam(20));
                            break;

                        case ATTACK_VIRUS1.EruptionBeam:
                            _movingCoroutine = StartCoroutine(EruptionBeam(2));
                            break;

                        case ATTACK_VIRUS1.Cube:
                            _movingCoroutine = StartCoroutine(ParallelCube(1));
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
                            _movingCoroutine = StartCoroutine(MathBeam(20));
                            break;

                        case ATTACK_VIRUS2.FireWork:
                            _movingCoroutine = StartCoroutine(FireWork());
                            break;

                        case ATTACK_VIRUS2.Cube:
                            _movingCoroutine = StartCoroutine(ParallelCube(2));
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

                Vector3 sneak = Moving2DSystems.GetSneaking(_posOwn, new Vector3(Random.Range(50, 590), 120, 0), 11);
                for (int i = 0; i < 11; i++)
                {
                    transform.position += sneak;
                    yield return new WaitForFixedUpdate();
                }

                ATTACK_VIRUS3 attackVariation3;
                do
                {
                    attackVariation3 = (ATTACK_VIRUS3)Random.Range(0, System.Enum.GetNames(typeof(ATTACK_VIRUS3)).Length);
                } while (attackVariation3 == _beforeAction3);

                _beforeAction3 = attackVariation3;

                switch (_beforeAction3)
                {
                    case ATTACK_VIRUS3.MathVirus:
                        _movingCoroutine = StartCoroutine(MathBeam(10));
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
                        _movingCoroutine = StartCoroutine(EruptionBeam(5));
                        break;
                }
                break;
        }
    }    

    /// <summary>
    /// 噴火ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator EruptionBeam(int number)
    {
        _textsVirus.SetTextSystem("Action_Attack_Beam");

        _textsVirus.SetTextDoingAction("Beam_Ready");
        List<Vector3> muzzles = new List<Vector3> {};
        for(int i = 0; i < number; i++)
        {
            muzzles.Add(new Vector3(Random.Range(20, 620), 0, 0));
        }


        List<float> angles = new List<float> { }; Random.Range(45, 135);
        for (int i = 0; i < number; i++)
        {
            angles.Add(Random.Range(45, 135));
        }

        for (int i = 0; i < number; i++)
        {
            Instantiate(BeamE, muzzles[i], _rotOwn).ShotEXP(angles[i], 13, 1);
        }
        
        _audioGO.PlayOneShot(beamS);
        yield return new WaitForSeconds(2.0f);

        _textsVirus.SetTextDoingAction("Beam_Fire");
        for (int j = 0; j < 30; j++)
        {
            for (int i = 0; i < number; i++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Instantiate(BeamE, muzzles[i], _rotOwn).ShotEXP(angles[i], 100, 1);
                    _audioGO.PlayOneShot(beamS);
                    _audioGO.PlayOneShot(expS);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.15f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    private enum TYPE_MATHBEAM
    {
        Sin,
        Cos,
    }

    /// <summary>
    /// サインコサインタンジェント
    /// </summary>
    /// <returns></returns>
    private IEnumerator MathBeam(int speed)
    {
        _textsVirus.SetTextSystem("Action_Attack_MathematicalWaves");
        TYPE_MATHBEAM sct = (TYPE_MATHBEAM)Random.Range(0, 2);
        switch (sct)
        {
            case TYPE_MATHBEAM.Sin:
                _textsVirus.SetTextDoingAction("Sin_Wave");
                break;

            case TYPE_MATHBEAM.Cos:
                _textsVirus.SetTextDoingAction("Cos_Wave");
                break;
        }


        _audioGO.PlayOneShot(magicS);
        for (float f6 = 0; f6 < 100; f6++)
        {
            switch (sct)
            {
                case TYPE_MATHBEAM.Sin:
                    for (float fi = 1; fi < f6; fi++)
                    {
                        shutu = new Vector3(-200+fi * 16, 240 + Mathf.Sin((fi / 5) + (f6 / speed)) * 220, 0);
                        Instantiate(Virus2E, shutu, _rotOwn).ShotEXP(0, 0, 0.06f);

                        shutu = new Vector3(-200+fi * 16, 240 + Mathf.Sin(-(fi / 5) - (f6 / speed)) * 220, 0);
                        Instantiate(Virus2E, shutu, _rotOwn).ShotEXP(0, 0, 0.06f);
                    }
                    break;

                case TYPE_MATHBEAM.Cos:
                    for (float fi = 1; fi < f6; fi++)
                    {
                        shutu = new Vector3(-200 + fi * 16, 240 + Mathf.Cos((fi / 5) + (f6 / speed)) * 220, 0);
                        Instantiate(Virus2E, shutu, _rotOwn).ShotEXP(0, 0, 0.06f);

                        shutu = new Vector3(-200 + fi * 16, 240 + Mathf.Cos(-(fi / 5) - (f6 / speed)) * 220, 0);
                        Instantiate(Virus2E, shutu, _rotOwn).ShotEXP(0, 0, 0.06f);
                    }
                    break;
            }
            if(Random.Range(0,5)!=0)_audioGO.PlayOneShot(shineS);
            yield return new WaitForFixedUpdate();
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 大血ビーム１
    /// </summary>
    /// <returns></returns>
    private IEnumerator BloodBeam1()
    {
        _textsVirus.SetTextSystem("Action_Attack_KILL");

        if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(384, 216, true);
        _eCoreC.EvoltionMode = 1;

        Vector3 sneak = Moving2DSystems.GetSneaking(_posOwn, new Vector3(Random.Range(50, 590), 120, 0), 33);
        for (int i = 0; i < 33; i++)
        {
            transform.position += sneak;
            yield return new WaitForFixedUpdate();
        }

        GameData.VirusBugEffectLevel = MODE_VIRUS.FullThrottle1;
        _posOwn = transform.position;
        List<float> angles = new List<float> { };
        foreach (GameObject player in _scPlsM.GetAllPlayers())
        {
            angles.Add(Moving2DSystems.GetAngle(_posOwn, player.transform.position));
        }
        TimeManager.ChangeTimeValue(0.1f);
        for (int j = 0; j < 29; j++)
        {
            _textsVirus.SetTextDoingAction("KILL_Ready..."+((float)(j * 100) / 30).ToString("N3")+"%");
            for (int i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                ExpC shot2 = Instantiate(VirusE, tar, _rotOwn);
                shot2.ShotEXP(Moving2DSystems.GetAngle(tar, _posOwn), 5, 0.1f);
            }
            _audioGO.PlayOneShot(chargeS);
            TimeManager.ChangeTimeValue(0.1f+(j*0.03f));
            yield return new WaitForFixedUpdate();
        }
        _textsVirus.SetTextDoingAction("KILL_Ready...Complete");

        TimeManager.ChangeTimeValue(1.0f);
        for (int j = 0; j < 170; j++)
        {
            if (_eCoreC.hp[1] > 50)
            {
                for (int i = 0; i < 10; i++)
                {
                    foreach (float angle in angles)
                    {
                        _rotOwn.z = Random.Range(0, 360);
                        Instantiate(BeamE, _posOwn, _rotOwn).ShotEXP(angle + Random.Range(-20, 20), Random.Range(50, 135), 0.4f);
                    }
                }
                _audioGO.PlayOneShot(damageS);
                _audioGO.PlayOneShot(heavyS);
                _goPlayer.GetComponent<PlayerC>().VibrationCritical();
                _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 1);
                _eCoreC.hp[1] -= 50;
                yield return new WaitForFixedUpdate();
            }
        }
        _textsVirus.SetTextDoingAction("Format...");
        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        for (int j = 0; j < 400; j++)
        {
            if (Random.Range(0, 100) == 0)
            {
                HealSummon();
            }
            
            if (j > 300) GameData.VirusBugEffectLevel = MODE_VIRUS.Little;
            yield return new WaitForFixedUpdate();
        }
        _textsVirus.SetTextDoingAction("Format...Complete");
        yield return new WaitForFixedUpdate();
        _eCoreC.EvoltionMode = 0;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 形態変化1->2
    /// </summary>
    /// <returns></returns>
    private IEnumerator Evoltion1()
    {
        TimeManager.ChangeTimeValue(1.0f);
        _textsVirus.SetTextDamage("Discover_Ary_Damages");

        _bgmManager.VolumefeedInOut(1.8f, 0);
        _virusmode = 1;

        Vector3 sneak = Moving2DSystems.GetSneaking(_posOwn,new Vector3(GameData.WindowSize.x / 2, 400, 0), 33);
        for (int i = 0; i < 33; i++)
        {
            _textsVirus.SetTextSystem("Search_For_Damage..." + ((float)(i * 100) / 33).ToString("N3") + "%");
            transform.position += sneak;
            yield return new WaitForFixedUpdate();
        }

        _textsVirus.SetTextSystem("Search_For_Damage...Complete");
        yield return new WaitForFixedUpdate();
        _textsVirus.SetTextDamage("Damage_Confirmed_To_OverAttack_Stopper");

        _audioGO.PlayOneShot(chargeS);
        for (int j = 0; j < 90; j++)
        {
            _textsVirus.SetTextSystem("Switch_To_Annihilation_Mode..." + ((float)(j * 100) / 90).ToString("N3") + "%");

            for (int i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                float angle2 = Moving2DSystems.GetAngle(tar, _posOwn);
                Instantiate(VirusE, tar, _rotOwn).ShotEXP(angle2, 10, 0.1f);
            }
            yield return new WaitForFixedUpdate();
        }
        _textsVirus.SetTextSystem("Switch_To_Annihilation_Mode...Complete");

        for (int j = 0; j < 20; j++)
        {
            muzzle = new Vector3(Random.Range(20, 620), 0, 0);
            _angle = Random.Range(70, 110);
            Instantiate(FireworkP, muzzle, _rotOwn)
                .ShotHowitzer(_angle, Random.Range(18, 25), -1.0f, Random.Range(13, 18), 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.09f);
        }
        _eCoreC.EvoltionMode = 0;
        Vector3 direction = new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0);
        Instantiate(StarP, direction, _rotOwn);
        for (int i = 0; i < 3; i++)
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
        _textsVirus.SetTextSystem("Action_Attack_FireWork");

        _audioGO.PlayOneShot(chargeS);
        for (int j=0;j<30;j++)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                float angle2 = Moving2DSystems.GetAngle(tar,_posOwn);
                Instantiate(VirusE, tar, _rotOwn).ShotEXP(angle2, 10, 0.1f);
            }
            yield return new WaitForFixedUpdate();
        }
        for(int j = 0; j < 13; j++)
        {
            muzzle = new Vector3(Random.Range(20, 620), 0, 0);
            _angle = Random.Range(70, 110);
            Instantiate(FireworkP, muzzle, _rotOwn).ShotHowitzer(_angle, Random.Range(18, 25), -1.0f, Random.Range(18, 25), 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.09f);
        }
        yield return new WaitForFixedUpdate();
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 並走キューブ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ParallelCube(int number)
    {
        _textsVirus.SetTextSystem("Action_Attack_ParallelCube");
        for(int k=0;k<7;k++)
        {
            _audioGO.PlayOneShot(magicS);
            float angle = 180;

            for(int i = 0; i < number; i++)
            {
                int cube = Random.Range(0, 5);
                muzzle = new Vector3(1000, (cube * 90) + 6 + 48, 0);
                EMissile1C shot = Instantiate(CubeP, muzzle, _rotOwn);
                shot.ShotMissile(angle, (cube % 2 + 1) * 7, 0);

                _textsVirus.SetTextDoingAction("ParallelCube_Summon_"+cube.ToString());
            }
            yield return new WaitForSeconds(0.5f);

        }
        yield return new WaitForFixedUpdate();
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 隕石
    /// </summary>
    /// <returns></returns>
    private IEnumerator Meteor()
    {
        _textsVirus.SetTextSystem("Action_Attack_Meteor");
        _textsVirus.SetTextDoingAction("Seach_Meteors...");
        _textsVirus.SetTextDoingAction("Seach_Meteors...Complete");

        float angle = Random.Range(260, 280);
        Instantiate(_meteorP, new Vector3(360, 650, 0), _rotOwn).ShotHowitzer(angle, 0, 0.1f, 100, 30, 3.0f);

        for(int i = 0; i < 5; i++)
        {
            angle = Random.Range(260, 280);
            _audioGO.PlayOneShot(shineS);
            Instantiate(_meteorP, new Vector3(GameData.GetRandomWindowPosition().x, GameData.WindowSize.y+64, 0), _rotOwn).ShotHowitzer(angle, 30, 0.1f, 100, 5, 0.6f);
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
        _textsVirus.SetTextSystem("Action_Attack_KILL_Beta");

        if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(96, 54, true);
        GameData.PlayerMoveAble = 6;
        _eCoreC.EvoltionMode = 2;
        GameData.VirusBugEffectLevel = MODE_VIRUS.FullThrottle2;

        Vector3 sneak = Moving2DSystems.GetSneaking(_posOwn, GameData.WindowSize / 2 - Vector2.up * 32, 33);
        for (int i = 0; i < 33; i++)
        {
            transform.position += sneak;
            yield return new WaitForFixedUpdate();
        }

        TimeManager.ChangeTimeValue(0.1f);

        for (int j = 0; j < 30; j++)
        {
            _textsVirus.SetTextDoingAction("KILL_Beta_Ready..." + ((float)(j * 100) / 30).ToString("N3") + "%");

            TimeManager.ChangeTimeValue(0.1f+(j*0.03f));
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForFixedUpdate();
        }
        _textsVirus.SetTextDoingAction("KILL_Beta_Ready...Complete");

        TimeManager.ChangeTimeValue(1.0f);
        List<float> angles = new List<float> { };
        foreach (GameObject player in _scPlsM.GetAllPlayers())
        {
            angles.Add(Moving2DSystems.GetAngle(_posOwn, player.transform.position));
        }
        for (int j = 0; j < 200; j++)
        {
            if (_eCoreC.hp[2] > 50)
            {
                for (int i = 0; i < 30 / _scPlsM.GetAllPlayers().Count + 1; i++)
                {
                    foreach (float angle in angles)
                    {
                        _rotOwn.z = Random.Range(0, 360);
                        ExpC shot2 = Instantiate(BeamE, _posOwn, _rotOwn);
                        shot2.ShotEXP(angle + Random.Range(30, 330), Random.Range(50, 135), 0.4f);
                    }
                }

                float randomMove = Random.Range(-2, 2);
                for (int i=0;i<angles.Count;i++)
                {
                    angles[i] += randomMove;
                }
                _audioGO.PlayOneShot(damageS);
                _audioGO.PlayOneShot(heavyS);
                _goPlayer.GetComponent<PlayerC>().VibrationCritical();
                _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 1);
                _eCoreC.hp[2] -= 1;
            }
            yield return new WaitForFixedUpdate();
        }

        _textsVirus.SetTextDoingAction("Format...");
        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        for (int j = 0; j < 340; j++)
        {
            if (Random.Range(0, 100) == 0)
            {
                HealSummon();
            }
            if (j > 240) GameData.VirusBugEffectLevel = MODE_VIRUS.FullThrottle2;
            yield return new WaitForFixedUpdate();
        }
        _textsVirus.SetTextDoingAction("Format...Complete");
        yield return new WaitForFixedUpdate();
        _eCoreC.EvoltionMode = 0;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 形態変化2->3
    /// </summary>
    /// <returns></returns>
    private IEnumerator Evoltion2()
    {
        TimeManager.ChangeTimeValue(1.0f);
        _textsVirus.SetTextDamage("Discover_Ary_Damages");


        _bgmManager.VolumefeedInOut(2.5f, 0);
        _virusmode = 2;
        _gameManaC._bossMaxHp = _eCoreC.hp[3];
        GameData.VirusBugEffectLevel = MODE_VIRUS.None;
        for (int j = 0; j < 100; j++)
        {
            _textsVirus.SetTextSystem("Search_For_Damage..." + ((float)(j * 100) / 100).ToString("N3") + "%");
            int math = Random.Range(0, 12) * 30;
            float angle = Moving2DSystems.GetAngle(transform.position,_posPlayer);
            shutu = _posOwn + new Vector3(Mathf.Cos(math * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(math * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, _rotOwn);
            barrier.ShotEXP(angle, 1, 0.9f);
            yield return new WaitForFixedUpdate();
        }

        _textsVirus.SetTextSystem("Search_For_Damage...Complete");
        yield return new WaitForFixedUpdate();
        _textsVirus.SetTextDamage("Destroyed_Confirmed_To_OverAttack_Stopper");
        _textsVirus.SetTextDamage("Destroyed_Access_Cntrol_System");

        _bgmManager.ChangeAudio(103, false, 0.8f);
        for (int j = 0; j < 30; j++)
        {
            _textsVirus.SetTextSystem("Restored_Access_Control_System..." + ((float)(j * 100) / 30).ToString("N3") + "%");

            int math = Random.Range(0, 12) * 30;
            float angle = Random.Range(0, 360);
            shutu = _posOwn + new Vector3(Mathf.Cos(math * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(math * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, _rotOwn);
            barrier.ShotEXP(angle, 10, 0.9f);
        }
        _textsVirus.SetTextDamage("Restored_Access_Control_System...Faild");

        yield return new WaitForSeconds(3.00f);
        _eCoreC.EvoltionMode = 3;
        Vector3 direction = new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0);
        Instantiate(StarP, direction, _rotOwn);
        for (int i = 0; i < 3; i++)
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
        _textsVirus.SetTextSystem("Action_Attack_PassingCube");

        TimeManager.ChangeTimeValue(1.0f);
        _audioGO.PlayOneShot(magicS);
        for(int k=0;k<30;k++)
        {
            _audioGO.PlayOneShot(magicS);
            float angle = 180;
            int cube = Random.Range(0, 5);
            muzzle = new Vector3((cube % 2 * 1200)-200, (cube * 90) + 6 + 48, 0);
            Instantiate(CubeP, muzzle, _rotOwn).ShotMissile(angle, ((cube % 2 * 2) - 1) * 9, 0);
            TimeManager.ChangeTimeValue(1.0f+(k*0.03f));

            _textsVirus.SetTextDoingAction("ParallelCube_Summon_" + cube.ToString());

            yield return new WaitForSeconds(0.21f);
        }
        TimeManager.ChangeTimeValue(1.0f);
        yield return new WaitForFixedUpdate();
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 即席大血ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator BloodBeam3()
    {
        _textsVirus.SetTextSystem("Action_Attack_KILL_Instant");

        if (GameData.CheckIsUpperDifficulty(MODE_DIFFICULTY.Assault)) Screen.SetResolution(384, 216, true);
        GameData.VirusBugEffectLevel = MODE_VIRUS.Medium;

        List<float> angles = new List<float> { };
        foreach (GameObject player in _scPlsM.GetAllPlayers())
        {
            angles.Add(Moving2DSystems.GetAngle(_posOwn, player.transform.position));
        }

        TimeManager.ChangeTimeValue(0.4f);

        for (int j=0;j<30;j++)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                ExpC shot2 = Instantiate(VirusE, tar, _rotOwn);
                shot2.ShotEXP(Moving2DSystems.GetAngle(tar, _posOwn), 10, 0.1f);
            }
            TimeManager.ChangeTimeValue(0.4f+(j*0.02f));
            _audioGO.PlayOneShot(chargeS);

            _textsVirus.SetTextDoingAction("KILL_Instant_Ready..." + ((float)(j * 100) / 30).ToString("N3") + "%");

            yield return new WaitForFixedUpdate();
        }
        _textsVirus.SetTextDoingAction("KILL_Instant_Ready...Complete");

        TimeManager.ChangeTimeValue(1.0f);
        for (int j=0;j<70;j++)
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (float angle in angles)
                {
                    _rotOwn.z = Random.Range(0, 360);
                    Instantiate(BeamE, _posOwn, _rotOwn).ShotEXP(angle + Random.Range(-20, 20), Random.Range(50, 135), 0.4f);
                }
            }
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(beamS);
            _goPlayer.GetComponent<PlayerC>().VibrationCritical();
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(5, 1);
            yield return new WaitForFixedUpdate();
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

    public int GetVirusMode()
    {
        return _virusmode;
    }

    public override void GetDamageValue(int damage)
    {
        if (damage > 0)
        {
            _textsVirus.SetTextDamage("Find_Damage_To_System <value:" + damage.ToString() + ">");
        }
        else
        {
            _textsVirus.SetTextDamage("Blocked_Unauthorized_Access");
        }
    }

    protected override IEnumerator ArrivalAction()
    {
        _textsVirus.SetTextSystem("Boot");
        _textsVirus.SetTextSystem("DMS_Boot...");
        _textsVirus.SetTextSystem("DMS_Boot...Complete");


        _bgmManager.ChangeAudio(101, false, 0.8f);

        //牽制
        for (int i = 0; i < 50; i++)
        {
            _textsVirus.SetTextSystem("Ready..." + ((float)(i * 100) / 50).ToString("N3") + "%");
            for (int j = 0; j < 4; j++)
            {
                Vector3 posVirusDust;
                do
                {
                    posVirusDust = new Vector3(Random.Range(0, 640), Random.Range(0, 530), 0);
                }
                while ((_posPlayer.x + 64 > posVirusDust.x && posVirusDust.x > _posPlayer.x - 64) && (_posPlayer.y + 64 > posVirusDust.y && posVirusDust.y > _posPlayer.y - 64));

                float sho = Random.Range(0, 360);
                Instantiate(Virus2E, posVirusDust, _rotOwn).ShotEXP(sho, Random.Range(-0.2f, 0.2f), 1);
            }
            yield return new WaitForFixedUpdate();
            
        }
        _textsVirus.SetTextSystem("Ready...Complete");
        _textsVirus.SetTextSystem("Hello World!");

        GameData.VirusBugEffectLevel = MODE_VIRUS.Little;
        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    protected override IEnumerator DeadAction()
    {
        TimeManager.ChangeTimeValue(1.0f);

        _textsVirus.SetTextSystem("Confirmed Several_Serious_Damages");
        for (int i = 0; i < 100; i++)
        {
            _textsVirus.SetTextSystem("Repair...");

            _angle = Random.Range(0, 360);
            ExpC cosgun = Instantiate(Virus2E, _posOwn, _rotOwn);
            cosgun.ShotEXP(_angle, 20, 0.3f);
            yield return new WaitForFixedUpdate();
            _textsVirus.SetTextSystem("Repair...Faild");
        }

        for (int i = 0; i < 100; i++)
        {
            _angle = Random.Range(0, 360);
            ExpC cosgun = Instantiate(Virus2E, _posOwn, _rotOwn);
            cosgun.ShotEXP(_angle, 20, 0.3f);
            _tfOwnBody.localScale -= new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForFixedUpdate();
        }

        GameData.Point += 100000;
        ClearC.BossBonus += 10;
        Destroy(gameObject);
    }

    protected override IEnumerator LeaveAction()
    {
        RandomScreenWipe(192, 108);
        while (true)
        {
            float distance = Moving2DSystems.GetDistance(_posOwn,_posPlayer);
            Vector3 angles = Moving2DSystems.GetDirection(Moving2DSystems.GetAngle(_posOwn, _posPlayer)).normalized;
            for (int j = 0; j < 20; j++)
            {
                Instantiate(Virus2E, _posPlayer, _rotOwn).ShotEXP(Random.Range(0, 360), 10, 0.3f);
                Instantiate(Virus2E, _posOwn + angles * Random.Range(0, distance), _rotOwn).ShotEXP(Random.Range(0, 360), 1, 0.3f);
            }
            _audioGO.PlayOneShot(heavyS);
            yield return new WaitForFixedUpdate();
        }
    }
}
