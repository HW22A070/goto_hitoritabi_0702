﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusC : MonoBehaviour
{
    private int i = 0;
    private int j = 0;
    private int k = 0;
    private float fi = 0, f6 = 0;
    private int _attackVariation = 0;
    private int changetime = 0;
    private int count1 = 0, count2 = 0, count3 = 0, count4 = 0, count5 = 0, count6 = 0;
    private int sct = 0;

    private int mae = 0;

    /// <summary>
    /// 0=第一形態クール
    /// 1=第二形態クール
    /// 2=第３形態
    /// </summary>
    private int virusmode = 0;
    /// <summary>
    /// 0=第一形態クール
    /// 1=第二形態クール
    /// 2=第３形態
    /// </summary>
    public static int VirusMode = 0;
    private int looks;
    private float angle, angle2;
    private float down = 2;
    private Vector3 pos, ppos, muki, velocity, shutu, Aa, gai, nerai;
    public SpriteRenderer mine;
    public Sprite a, b, c, d, e, f, g, h, aa, bb, cc, dd, ee, ff;

    /// <summary>
    /// 隙晒しまでのカウンター
    /// </summary>
    private int _countBlood = 0;

    public HealC HealP, StarP;
    public ExpC Virus2E, VirusE, BeamE;
    public GuardC tamaP;
    public EMissile1C MissileP, CubeP;
    public FireworkC FireworkP;

    [SerializeField]
    [Tooltip("爆弾")]
    private BombC _meteorP;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip damageS, deadS, heavyS, beamS, magicS, shineS, expS, chargeS;
    public short[] BeamD , BulletD , FireD , BombD, ExpD;

    private Vector3 hitPos;
    private PMCoreC playerMissileP;

    [SerializeField]
    [Tooltip("クリティカル発生")]
    private bool[] _isBeamCritical, _isBulletCritical, _isFireCritical, _isBombCritical;

    private PlayerC playerP;
    private GameObject PlayerGO;

    private Quaternion rot;

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
        pos = transform.position;
        PlayerGO = GameObject.Find("Player");
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        _gameManaC._bossNowHp = _eCoreC.hp[1];
        _gameManaC._bossMaxHp = _eCoreC.hp[1];
        _eCoreC.IsBoss = true;
    }


    public void Summon(int judge)
    {

    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        ppos = PlayerGO.transform.position;
        rot = transform.localRotation;
        if (_eCoreC.BossLifeMode != 2) _gameManaC._bossNowHp = _eCoreC.hp[_eCoreC.EvoltionMode];

    }


    void FixedUpdate()
    {

        VirusMode = virusmode;

        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            _bgmManager.ChangeAudio(101, false, 0.8f);
            for (int i = 0; i < 200; i++)
            {
                do
                {
                    Aa = new Vector3(Random.Range(0, 640), Random.Range(0, 530), 0);
                }
                while ((ppos.x + 64 > Aa.x && Aa.x > ppos.x - 64) && (ppos.y + 64 > Aa.y && Aa.y > ppos.y - 64));

                float sho = Random.Range(0, 360);
                ExpC shot2 = Instantiate(Virus2E, Aa, rot);
                shot2.EShot1(sho, Random.Range(-0.2f, 0.2f), 1);
            }
            GameData.VirusBugEffectLevel = 1;
            _eCoreC.BossLifeMode = 1;
            _movingCoroutine = StartCoroutine(ActionBranch());
        }
        
        //形態変化1->2
        if (virusmode==0&&_eCoreC.hp[1]<=0)
        {
            AllCoroutineStop();
            _movingCoroutine = StartCoroutine(Evoltion1());
        }

        //形態変化2->3
        if (virusmode == 1 && _eCoreC.hp[2] <= 0)
        {
            AllCoroutineStop();
            _movingCoroutine = StartCoroutine(Evoltion2());
        }

        if(GameData.VirusBugEffectLevel>0)
        {
            TextBug();
        }

        //DeathAction
        if (_eCoreC.BossLifeMode == 2)
        {
            
            GameData.Star = true;
            GameData.TimerMoving = false;
            _gameManaC._bossNowHp=0;
            AllCoroutineStop();
            GameData.VirusBugEffectLevel = 0;
            if (k == 1)
            {
                TimeManager.ChangeTimeValue(1.0f);
            }
            k++;
            if (k <= 100)
            {
                angle = Random.Range(0, 360);
                ExpC cosgun = Instantiate(Virus2E, pos, rot);
                cosgun.EShot1(angle, 20, 0.3f);
            }
            if (k > 100)
            {
                angle = Random.Range(0, 360);
                ExpC cosgun = Instantiate(Virus2E, pos, rot);
                cosgun.EShot1(angle, 20, 0.3f);
                gameObject.transform.localScale -= new Vector3(0.01f, 0.01f, 0);
            }
            if (k > 200)
            {
                GameData.Point += 100000;
                CrearC.BossBonus += 10;
                Destroy(gameObject);
            }

        }

        //LooksChange
        if (virusmode<=1)
        {
            looks = Random.Range(0, 8);
            if (looks == 0) mine.sprite = a;
            else if (looks == 1) mine.sprite = b;
            else if (looks == 2) mine.sprite = c;
            else if (looks == 3) mine.sprite = d;
            else if (looks == 4) mine.sprite = e;
            else if (looks == 5) mine.sprite = f;
            else if (looks == 6) mine.sprite = g;
            else if (looks == 7) mine.sprite = h;
        }
        else if (virusmode==2)
        {
            looks = Random.Range(0, 6);
            if (looks == 0) mine.sprite = aa;
            else if (looks == 1) mine.sprite = bb;
            else if (looks == 2) mine.sprite = cc;
            else if (looks == 3) mine.sprite = dd;
            else if (looks == 4) mine.sprite = ee;
            else if (looks == 5) mine.sprite = ff;
        }
        if (changetime > 100)
        {
            looks = Random.Range(0, 6);
            if (looks == 0) mine.sprite = aa;
            else if (looks == 1) mine.sprite = bb;
            else if (looks == 2) mine.sprite = cc;
            else if (looks == 3) mine.sprite = dd;
            else if (looks == 4) mine.sprite = ee;
            else if (looks == 5) mine.sprite = ff;
        }


        if (_eCoreC.EvoltionMode == 0)
        {
            //Barrier
            i = Random.Range(0, 12) * 30;
            float angle = GameData.GetAngle(transform.position,ppos);
            shutu = pos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(i * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, rot);
            barrier.EShot1(angle, 1, 0.9f);

            //Effect
            angle = Random.Range(0, 360);
            shutu = new Vector3(Random.Range(-160, 800), Random.Range(0, 530),0) ;
            ExpC effect = Instantiate(VirusE, shutu, rot);
            effect.EShot1(angle, 0.3f, 4);
        }

        //hpbug
        if (_eCoreC.EvoltionMode == 0)
        {
            if (virusmode == 0) _eCoreC.hp[0] = Random.Range(0, 42000);
            else if (virusmode == 1) _eCoreC.hp[0] = Random.Range(0, 42000);
        }
        else _eCoreC.hp[0] = 1;




        //Roll
        if (_eCoreC.BossLifeMode < 2)transform.localEulerAngles += new Vector3(0, 0, -_eCoreC.BossLifeMode + 4);
        else transform.localEulerAngles += new Vector3(0, 0, 0);

        
    }

    /// <summary>
    /// 行動分岐
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionBranch()
    {

        if (virusmode == 0)
        {
            if(GameData.Difficulty>=2)RandomScreenWipe(2,5);
            //Screen.SetResolution(480, 270, true);
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
                do
                {
                    _attackVariation = Random.Range(0, 3);
                } while (_attackVariation == mae);
                mae = _attackVariation;
                if (_attackVariation == 0) _movingCoroutine = StartCoroutine(SCT(10));
                else if (_attackVariation == 1) _movingCoroutine = StartCoroutine(UnderBeam());
                else if (_attackVariation == 2) _movingCoroutine = StartCoroutine(SCT(13));
            }


        }
        else if (virusmode == 1)
        {
            if (GameData.Difficulty >= 2) RandomScreenWipe(3,7);
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
                do
                {
                    _attackVariation = Random.Range(0, 5);
                } while (_attackVariation == mae);
                mae = _attackVariation;
                if (_attackVariation == 0) _movingCoroutine = StartCoroutine(SCT(9));
                else if (_attackVariation == 1) _movingCoroutine = StartCoroutine(SCT(9));
                else if (_attackVariation == 2) _movingCoroutine = StartCoroutine(FireWork());
                else if (_attackVariation == 3) _movingCoroutine = StartCoroutine(Cube1());
                else if (_attackVariation == 4) _movingCoroutine = StartCoroutine(Meteor());
            }
        }
        else if (virusmode == 2)
        {

            if (GameData.Difficulty >= 2) Screen.SetResolution(GameData.FirstWidth, GameData.FirstHeight, true);
            GameData.PlayerMoveAble = 6;
            yield return new WaitForSeconds(0.03f);
            _eCoreC.EvoltionMode = 3;
            do
            {
                _attackVariation = Random.Range(0, 6);
            } while (_attackVariation == mae);
            mae = _attackVariation;
            if (_attackVariation == 0) _movingCoroutine = StartCoroutine(FireWork());
            else if (_attackVariation == 1) _movingCoroutine = StartCoroutine(Meteor());
            else if (_attackVariation == 2) _movingCoroutine = StartCoroutine(UnderBeam());
            else if (_attackVariation == 3) _movingCoroutine = StartCoroutine(Cube2());
            else if (_attackVariation == 4) _movingCoroutine = StartCoroutine(SCT(7));
            else if (_attackVariation == 5) _movingCoroutine = StartCoroutine(BloodBeam3());
        }

    }    
    
    /// <summary>
    /// 衛生
    /// </summary>
    /// <returns></returns>
    private IEnumerator Guardian()
    {
        for(int j=0;j<6;j++)
        {
            transform.localPosition = ppos + new Vector3(0, 100, 0);
            for (i = 0; i < 10; i++)
            {
                GuardC shot = Instantiate(tamaP, pos, rot);
                shot.EShot1(10, i * 36, 1, ((i % 2) + 1) * 3, pos);
            }
            _audioGO.PlayOneShot(heavyS);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.15f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 地下ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator UnderBeam()
    {

        gai = new Vector3(Random.Range(20, 620), 0, 0);
        angle2 = Random.Range(45, 135);

        for (int j = 0; j < 30; j++)
        {
            if (Random.Range(0, 2) == 0)
            {
                ExpC shot = Instantiate(BeamE, gai, rot);
                shot.EShot1(angle2, 100, 1);
                _audioGO.PlayOneShot(beamS);
                _audioGO.PlayOneShot(expS);
                yield return new WaitForSeconds(0.03f);
            }
        }
        yield return new WaitForSeconds(0.15f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// サインコサインタンジェント
    /// </summary>
    /// <returns></returns>
    private IEnumerator SCT(int speed)
    {
        sct = Random.Range(0, 4);
        _audioGO.PlayOneShot(magicS);
        for (float f6 = 0; f6 < 100; f6++)
        {
            if (sct == 0)
            {

                for (fi = 1; fi < f6; fi++)
                {
                    shutu = new Vector3(-200+(fi * 16), 240 + Mathf.Sin(fi / (3 + ((100 - f6) / speed))) * 220, 0);
                    ExpC singun = Instantiate(Virus2E, shutu, rot);
                    singun.EShot1(0, 0, 0.1f);
                    shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Sin(-fi / (3 + ((100 - f6) / speed))) * 220, 0);
                    ExpC cosgun = Instantiate(Virus2E, shutu, rot);
                    cosgun.EShot1(0, 0, 0.1f);
                }
            }
            else if (sct == 1)
            {
                for (fi = 1; fi < f6; fi++)
                {
                    shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Cos(fi / (3 + ((100 - f6) / speed))) * 220, 0);
                    ExpC singun = Instantiate(Virus2E, shutu, rot);
                    singun.EShot1(0, 0, 0.1f);
                    shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Cos(-fi / (3 + ((100 - f6) / speed))) * 220, 0);
                    ExpC cosgun = Instantiate(Virus2E, shutu, rot);
                    cosgun.EShot1(0, 0, 0.05f);
                }
            }
            else if (sct == 2)
            {
                for (fi = 1; fi < f6; fi++)
                {
                    shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Tan(fi / (3 + ((100 - f6) / (speed * 1.5f)))) * 220, 0);
                    ExpC singun = Instantiate(Virus2E, shutu, rot);
                    singun.EShot1(0, 0, 0.1f);
                    shutu = new Vector3(-200 + (fi * 16), 240 + Mathf.Tan(-fi / (3 + ((100 - f6) / (speed * 1.5f)))) * 220, 0);
                    ExpC cosgun = Instantiate(Virus2E, shutu, rot);
                    cosgun.EShot1(0, 0, 0.05f);
                }
            }
            else
            {
                for (fi = 1; fi < f6; fi++)
                {
                    shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 5) + (f6 / speed)) * 220, 0);
                    ExpC singun = Instantiate(Virus2E, shutu, rot);
                    singun.EShot1(0, 0, 0.05f);
                    shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 5) - (f6 / speed)) * 220, 0);
                    ExpC cosgun = Instantiate(Virus2E, shutu, rot);
                    cosgun.EShot1(0, 0, 0.05f);
                }
                
            }
            _audioGO.PlayOneShot(shineS);
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
        if (GameData.Difficulty >= 2) Screen.SetResolution(384, 216, true);
        GameData.PlayerMoveAble = 6;
        _eCoreC.EvoltionMode = 1;
        transform.localPosition = new Vector3(Random.Range(50, 590), 120, 0);
        GameData.VirusBugEffectLevel = 100;
        pos = transform.position;
        angle2 = GameData.GetAngle(pos, ppos);
        TimeManager.ChangeTimeValue(0.1f);
        for (int j = 0; j < 29; j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = pos + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                ExpC shot2 = Instantiate(VirusE, tar, rot);
                shot2.EShot1(GameData.GetAngle(tar, pos), 5, 0.1f);
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
                    
                    rot.z = Random.Range(0, 360);
                    ExpC shot2 = Instantiate(BeamE, pos, rot);
                    shot2.EShot1(angle2 + Random.Range(-20, 20), Random.Range(50, 135), 0.4f);
                }
                _audioGO.PlayOneShot(damageS);
                _audioGO.PlayOneShot(heavyS);
                playerP = PlayerGO.GetComponent<PlayerC>();
                playerP.CriticalVibration();
                _goCamera.GetComponent<CameraC>().StartShakeVertical(5, 1);
                _eCoreC.hp[1] -= 50;
                yield return new WaitForSeconds(0.03f);
            }
        }
        GameData.VirusBugEffectLevel = 0;
        for (int j = 0; j < 400; j++)
        {
            if (Random.Range(0, 100) == 0)
            {
                HealSummon();
            }
            
            if (j > 300) GameData.VirusBugEffectLevel = 1;
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
        virusmode = 1;
        yield return new WaitForSeconds(0.03f);
        for (int j = 0; j < 2; j++)
        {
            gai = new Vector3(Random.Range(20, 620), 0, 0);
            angle = Random.Range(70, 110);
            Instantiate(FireworkP, gai, rot).EShot1(angle, Random.Range(18, 25), -1.0f, Random.Range(10, 16), 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.5f);
        }
        for (int j = 0; j < 9; j++)
        {
            gai = new Vector3(320, 0, 0);
            angle = 45 + (j * 10);
            Instantiate(FireworkP, gai, rot).EShot1(angle, 22, -1.0f, 20, 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.09f);
        }
        yield return new WaitForSeconds(0.03f);
        _eCoreC.EvoltionMode = 0;
        Vector3 direction = new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0);
        Instantiate(StarP, direction, rot);
        for (i = 0; i < 3; i++)
        {
            HealSummon();
        }
        _bgmManager.ChangeAudio(102, false, 0.8f);
        GameData.VirusBugEffectLevel = 2;
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
                Vector3 tar = pos + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                float angle2 = GameData.GetAngle(tar,pos);
                Instantiate(VirusE, tar, rot).EShot1(angle2, 10, 0.1f);
            }
            yield return new WaitForSeconds(0.03f);
        }
        for(int j = 0; j < 2; j++)
        {
            gai = new Vector3(Random.Range(20, 620), 0, 0);
            angle = Random.Range(70, 110);
            Instantiate(FireworkP, gai, rot).EShot1(angle, Random.Range(18, 25), -1.0f, Random.Range(18, 25), 10, 0.3f);
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.5f);
        }
        for (int j = 0; j < 9; j++)
        {
            gai = new Vector3(320, 0, 0);
            angle = 45 + (j * 10);
            Instantiate(FireworkP, gai, rot).EShot1(angle, 22, -1.0f, 20, 10, 0.3f);
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
            angle2 = 180;

            j = Random.Range(0, 5);
            gai = new Vector3(1000, (j * 90) + 6 + 48, 0);
            EMissile1C shot = Instantiate(CubeP, gai, rot);
            shot.EShot1(angle2, (j % 2 + 1) * 7, 0);
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
        _audioGO.PlayOneShot(shineS);
        float angle = Random.Range(260, 280);
        BombC shot = Instantiate(_meteorP, new Vector3(360, 650, 0), rot);
        shot.EShot1(angle, 0, 0.1f, 100, 30, 3.0f);
        yield return new WaitForSeconds(1.5f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 大血ビーム２
    /// </summary>
    /// <returns></returns>
    private IEnumerator BloodBeam2()
    {
        if (GameData.Difficulty >= 2) Screen.SetResolution(96, 54, true);
        GameData.PlayerMoveAble = 6;
        _eCoreC.EvoltionMode = 2;
        transform.localPosition = new Vector3(Random.Range(50, 590), 210, 0);
        GameData.VirusBugEffectLevel = 200;
        yield return new WaitForSeconds(0.6f);
        TimeManager.ChangeTimeValue(0.1f);
        for (int j = 0; j < 30; j++)
        {
            TimeManager.ChangeTimeValue(0.1f+(j*0.03f));
            _audioGO.PlayOneShot(chargeS);
            yield return new WaitForSeconds(0.03f);
        }
        TimeManager.ChangeTimeValue(1.0f);
        angle2 = GameData.GetAngle(pos, ppos);
        for (int j = 0; j < 200; j++)
        {
            if (_eCoreC.hp[2] > 50)
            {
                for (i = 0; i < 30; i++)
                {
                    rot.z = Random.Range(0, 360);
                    ExpC shot2 = Instantiate(BeamE, pos, rot);
                    shot2.EShot1(angle2 + Random.Range(30, 330), Random.Range(50, 135), 0.4f);
                }
                angle2 += Random.Range(-2, 2);
                _audioGO.PlayOneShot(damageS);
                _audioGO.PlayOneShot(heavyS);
                playerP = PlayerGO.GetComponent<PlayerC>();
                playerP.CriticalVibration();
                _goCamera.GetComponent<CameraC>().StartShakeVertical(5, 1);
                _eCoreC.hp[2] -= 1;
            }
            yield return new WaitForSeconds(0.03f);
        }
        GameData.VirusBugEffectLevel = 0;
        for (int j = 0; j < 340; j++)
        {
            if (Random.Range(0, 100) == 0)
            {
                HealSummon();
            }
            if (j > 240) GameData.VirusBugEffectLevel = 2;
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
        virusmode = 2;
        _gameManaC._bossMaxHp = _eCoreC.hp[3];
        GameData.VirusBugEffectLevel = 0;
        for (int j = 0; j < 100; j++)
        {
            i = Random.Range(0, 12) * 30;
            float angle = GameData.GetAngle(transform.position,ppos);
            shutu = pos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(i * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, rot);
            barrier.EShot1(angle, 1, 0.9f);
            yield return new WaitForSeconds(0.03f);
        }
        _bgmManager.ChangeAudio(103, false, 0.8f);
        for (j = 0; j < 30; j++)
        {
            i = Random.Range(0, 12) * 30;
            float angle = Random.Range(0, 360);
            shutu = pos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 8 * 5, Mathf.Sin(i * Mathf.Deg2Rad) * 8 * 5, 0);
            ExpC barrier = Instantiate(Virus2E, shutu, rot);
            barrier.EShot1(angle, 10, 0.9f);
        }

        yield return new WaitForSeconds(3.00f);
        _eCoreC.EvoltionMode = 2;
        Vector3 direction = new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0);
        HealC shot = Instantiate(StarP, direction, rot);
        shot.EShot1();
        for (i = 0; i < 3; i++)
        {
            HealSummon();
        }
        GameData.VirusBugEffectLevel = 2;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// すれ違いキューブ
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cube2()
    {
        TimeManager.ChangeTimeValue(1.0f);
        _audioGO.PlayOneShot(magicS);
        for(int k=0;k<30;k++)
        {
            angle2 = 180;
            j = Random.Range(0, 5);
            gai = new Vector3((j % 2 * 1200)-200, (j * 90) + 6 + 48, 0);
            Instantiate(CubeP, gai, rot).EShot1(angle2, ((j % 2 * 2) - 1) * 9, 0);
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
        if(GameData.Difficulty>=2) Screen.SetResolution(384, 216, true);
        GameData.VirusBugEffectLevel = 100;
        transform.localPosition = new Vector3(Random.Range(50, 590), 120, 0);
        angle2 = GameData.GetAngle(transform.position, ppos);
        TimeManager.ChangeTimeValue(0.4f);
        for (int j=0;j<30;j++)
        {
            for (i = 0; i < 3; i++)
            {
                Vector3 tar = pos + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
                ExpC shot2 = Instantiate(VirusE, tar, rot);
                shot2.EShot1(GameData.GetAngle(tar, pos), 10, 0.1f);
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
               
                rot.z = Random.Range(0, 360);
                ExpC shot2 = Instantiate(BeamE, pos, rot);
                shot2.EShot1(angle2 + Random.Range(-20, 20), Random.Range(50, 135), 0.4f);
            }
            _audioGO.PlayOneShot(heavyS);
            _audioGO.PlayOneShot(beamS);
            playerP = PlayerGO.GetComponent<PlayerC>();
            playerP.CriticalVibration();
            _goCamera.GetComponent<CameraC>().StartShakeVertical(5, 1);
            yield return new WaitForSeconds(0.03f);
        }
        GameData.VirusBugEffectLevel = 2;
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

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

    private void RandomScreenWipe(int min,int max)
    {
        int hoge = Random.Range(min, max+1);
        Screen.SetResolution(GameData.FirstWidth / hoge, GameData.FirstHeight / hoge, true);
    }

    private void HealSummon()
    {
        Instantiate(HealP, new Vector3(Random.Range(10, 630), 32 + Random.Range(1, 5) * 90, 0), transform.localRotation).EShot1();
    }

    private void AllCoroutineStop()
    {
        StopAllCoroutines();
        _movingCoroutine = null;
    }
}
