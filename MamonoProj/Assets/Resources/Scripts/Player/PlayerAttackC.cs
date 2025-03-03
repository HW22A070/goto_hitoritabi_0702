using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Player;
using System;
using Random = UnityEngine.Random;
using EnumDic.System;

public class PlayerAttackC : MonoBehaviour
{
    private Vector3 _posOwn;
    private Quaternion _rotOwn;

    /// <summary>
    /// プレイヤーのグラフィックかちょっとでかいぶん銃の場所を下げる値
    /// </summary>
    private Vector3 _posShot;


    [SerializeField, Tooltip("武器ステータス")]
    private GunStates[] _statesWeapon = new GunStates[4]{
        new GunStates{
            mode=MODE_GUN.Shining,
            energyChargeTick=1.66f,
            cooltimeDefault=0.1f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=2f,
            isLoaded=true
        },
        new GunStates{
            mode=MODE_GUN.Physical,
            energyChargeTick=1.5f,
            cooltimeDefault=0.08f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=2f,
            isLoaded=true
        },
        new GunStates{
            mode=MODE_GUN.Crash,
            energyChargeTick=2.0f,
            cooltimeDefault=0.6f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=8f,
            isLoaded=true
        },
        new GunStates{
            mode=MODE_GUN.Heat,
            energyChargeTick=0.75f,
            cooltimeDefault=0.03f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=1f,
            isLoaded=true
        },
    };

    private float _enegyConsumptionChargeShot = 90f;

    /// <summary>
    /// 0=ビーム
    /// 1=バレット
    /// 2=ボム
    /// 3=バーン
    /// </summary>
    private MODE_GUN _modeGun;
    //private int _playerNumber = 0;

    /// <summary>
    /// 変形中
    /// </summary>
    private bool _isChanging;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile _prhbMissile;

    [SerializeField, Tooltip("弾アタッチ、")]
    private PBombC PBombP, PMinePutP, PMineP, PMinecristalP;

    [SerializeField, Tooltip("弾アタッチ")]
    private PExpC PFireP, PSlashP;

    [SerializeField, Tooltip("弾アタッチ")]
    private EMissile1C TPwazaPrefab;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

    [SerializeField, Tooltip("ビーム必殺")]
    private PSpecialBeamC BeamMP;

    [SerializeField, Tooltip("バレット必殺")]
    private PMachineGunC PMachinegunP;

    [SerializeField, Tooltip("バーン必殺")]
    private PMeteorC PMeteorP;

    [SerializeField, Tooltip("照準エフェクトアタッチ")]
    private ExpC _prfbTarget;

    [SerializeField, Header("ビームチャイルド")]
    private GameObject _prhbBeamChild;

    [SerializeField, Header("バレットチャイルド")]
    private GameObject _prhbBulletChild;

    [SerializeField, Header("ボムチャイルド")]
    private GameObject _prhbBombChild;

    [SerializeField, Header("バ―ンチャイルド")]
    private GameObject _prhbBurnChild;

    /// <summary>
    /// ロードされたチャイルド管理用
    /// </summary>
    private List<GameObject> _goChild = new List<GameObject> { };

    [SerializeField, Tooltip("サウンド")]
    private AudioClip shotS,magicuseS, exprosionS, bulletS, putS, fireS, ChangeS, SlashS;

    [SerializeField, Tooltip("ロードサウンド")]
    private AudioClip[] _loadS;


    private PlayerC _scPlayer;
    private GameObject _goCamera;
    private AudioSource _goAudio;



    // Start is called before the first frame update
    void Start()
    {
        _scPlayer = GetComponent<PlayerC>();
        _goAudio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");

        _posOwn = transform.position;
        Summon_Child();

        
        //さつりくマシンの武器強化
        if (GameData.Difficulty ==MODE_DIFFICULTY.Berserker)
        {
            for(int i = 0; i < _statesWeapon.Length; i++)
            {
                _statesWeapon[i].energyChargeTick *= 2f;
                _statesWeapon[i].cooltimeDefault *= 0.5f;
                _statesWeapon[i].enegyConsumption *= 0.5f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _posOwn = transform.position;
        _rotOwn = transform.localRotation;
        _posShot = _posOwn - (transform.up * 4);

        for (int mode = 0; mode < _statesWeapon.Length; mode++)
        {
            //短距離武器クールタイムの回復
            if (_statesWeapon[mode].cooltimeNow > 0)
            {
                _statesWeapon[mode].cooltimeNow -= Time.deltaTime;
            }
        }

    }

    private void FixedUpdate()
    {
        
        switch (_modeGun)
        {
            case MODE_GUN.Physical:

                //銃モード中は照準出す
                GameObject flontObj = _scPlayer.GetFlontEnemy();
                if (flontObj != null)
                {
                    Vector3 flontObjPos = GameData.FixPosition(flontObj.transform.position, 32, 32);
                    Instantiate(_prfbTarget, flontObjPos, _rotOwn).EShot1(0, 0, 0.03f);
                }
                break;
        }

        //エネルギー回復
        for (int mode = 0; mode < _statesWeapon.Length; mode++)
        {
            if (_statesWeapon[mode].energy < 100)
            {
                if (_modeGun != (MODE_GUN)mode) _statesWeapon[mode].energy += _statesWeapon[mode].energyChargeTick * 2;
                else
                {
                    if(_statesWeapon[mode].cooltimeNow <= 0) _statesWeapon[mode].energy += _statesWeapon[mode].energyChargeTick;
                }

                if (_statesWeapon[mode].energy >= 100)
                {
                    _goAudio.PlayOneShot(_loadS[mode]);

                    if (!_statesWeapon[mode].isLoaded)
                    {

                        _statesWeapon[mode].isLoaded = true;
                    }

                    _statesWeapon[mode].energy = 100;
                }
            }

        }
    }

    private void Summon_Child()
    {
        switch (_modeGun)
        {
            case MODE_GUN.Shining:
                Change_Beam();
                break;
            case MODE_GUN.Physical:
                Change_Bullet();
                break;
            case MODE_GUN.Crash:
                Change_Bomb();
                break;
            case MODE_GUN.Heat:
                Change_Burn();
                break;
        }
    }


    /// <summary>
    /// 変形
    /// </summary>
    /// <param name="modevalue"></param>
    public void SetGunModeChange(MODE_GUN modevalue)
    {

        if (modevalue != _modeGun)
        {
            _isChanging = true;
            StartCoroutine(ChangingAnim(modevalue));
        }
    }

    /// <summary>
    /// 変形ディレイ
    /// </summary>
    /// <param name="modevalue"></param>
    /// <returns></returns>
    private IEnumerator ChangingAnim(MODE_GUN modevalue)
    {
        _goAudio.PlayOneShot(ChangeS);
        yield return new WaitForSeconds(0.21f);
        _modeGun = modevalue;
        if (CheckIsLoad(modevalue)) _goAudio.PlayOneShot(_loadS[(int)_modeGun]);
        
        DeleteAllChild();
        Summon_Child();
        _scPlayer.ResetPlayerAnim();

        _isChanging = false;
    }



    /// <summary>
    /// ビームチャイルド呼び出し
    /// </summary>
    private void Change_Beam()
    {
        for (int hoge = 0; hoge < 3; hoge++)_goChild.Add(Instantiate(_prhbBeamChild, _posOwn, _rotOwn));
        for(int hoge=0;hoge< _goChild.Count;hoge++)_goChild[hoge].GetComponent<BeamChildC>().SetOfset(hoge * (360/_goChild.Count));
    }

    /// <summary>
    /// バレットチャイルド呼び出し
    /// </summary>
    private void Change_Bullet()
    {
        for (int hoge = 0; hoge < 2; hoge++)_goChild.Add(Instantiate(_prhbBulletChild, _posOwn, _rotOwn));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BulletChildC>().SetOfset(new Vector3(0, hoge * 32, 0));

    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Bomb()
    {
        for (int hoge = 0; hoge < 1; hoge++) _goChild.Add(Instantiate(_prhbBombChild, _posOwn, _rotOwn));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BombChildC>().SetOfset(new Vector3(0, (hoge+1)*48, 0));
    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Burn()
    {
        for (int hoge = 0; hoge < 1; hoge++)_goChild.Add(Instantiate(_prhbBurnChild, _posOwn, _rotOwn));
        //for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<FireChildC>();
    }

    /// <summary>
    /// 遠距離攻撃
    /// </summary>
    public void Fire()
    {
        if (!_isChanging)
        {
            if (GetWeaponState(_modeGun).energy >= _enegyConsumptionChargeShot && GetWeaponState(_modeGun).isLoaded)
            {
                switch (_modeGun)
                {
                    case MODE_GUN.Shining:
                        Shot_Raser();
                        break;

                    case MODE_GUN.Physical:
                        Shot_Bullet();
                        break;

                    case MODE_GUN.Crash:
                        Shot_Drop();
                        break;

                    case MODE_GUN.Heat:
                        Shot_Rocket();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 近距離攻撃
    /// </summary>
    public void Fire2()
    {
        if (!_isChanging)
        {
            if (GetWeaponState(_modeGun).cooltimeNow <= 0 && GetWeaponState(_modeGun).isLoaded)
            {
                switch (_modeGun)
                {
                    case MODE_GUN.Shining:
                        Shot_Slash();
                        break;

                    case MODE_GUN.Physical:
                        Shot_ShotGun();
                        break;

                    case MODE_GUN.Crash:
                        Shot_MineSield();
                        break;

                    case MODE_GUN.Heat:
                        Shot_Fire();
                        break;
                }
            }
        }
    }



    /// <summary>
    /// 0Beam_Raser
    /// </summary>
    private void Shot_Raser()
    {

        foreach(GameObject child in _goChild)
        {
            child.GetComponent<BeamChildC>().DoAttackRaser();
        }
            
        _goAudio.PlayOneShot(shotS);

        DoUseEnegry(MODE_GUN.Shining, true);
    }

    /// <summary>
    /// 1Beam_Slash
    /// </summary>
    private void Shot_Slash()
    {
        foreach (GameObject child in _goChild)
        {
            child.GetComponent<BeamChildC>().DoAttackSlash();
        }

        PExpC prefab = Instantiate(PSlashP, _posShot, _rotOwn);
        prefab.EShot1(_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180, 0, 0.08f);
        prefab.transform.position += prefab.transform.up * 64;
        _goAudio.PlayOneShot(SlashS);

        DoUseEnegry(MODE_GUN.Shining, false);

    }

    /// <summary>
    /// 2Bullet_Rifle
    /// </summary>
    private void Shot_Bullet()
    {
        foreach (GameObject child in _goChild)
        {
            child.GetComponent<BulletChildC>().DoAttackSniper();
        }

        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(4, 5);
        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _goAudio.PlayOneShot(bulletS);

        DoUseEnegry(MODE_GUN.Physical, true);

    }

    /// <summary>
    /// 3Bullet_MachineGun
    /// </summary>
    private void Shot_ShotGun()
    {
        PMissile prefab = Instantiate(_prhbMissile, _posShot, _rotOwn);
        prefab.Shot((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) + Random.Range(-4, 4), 70, 0);
        prefab.transform.position += prefab.transform.up * 16;
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(2, 4);
        PlayBulletEffect();

        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _goAudio.PlayOneShot(bulletS);

        DoUseEnegry(MODE_GUN.Physical, false);
    }

    /// <summary>
    /// 4Bomb_drop
    /// </summary>
    private void Shot_Drop()
    {

        foreach (GameObject child in _goChild)
        {
            child.GetComponent<BombChildC>().DoAttackDrop();
        }

        _goAudio.PlayOneShot(putS);

        SetCoolTimeToDefault(MODE_GUN.Crash);
        
        DoUseEnegry(MODE_GUN.Crash, true);
    }

    /// <summary>
    /// 5Bomb_Sield
    /// </summary>
    private void Shot_MineSield()
    {
        PBombC shot = Instantiate(PMinePutP, new Vector3(_posShot.x + (_scPlayer.CheckPlayerAngleIsRight() ? 48 : -48), _posShot.y, 0), _rotOwn);
        shot.EShot1(270, 0, 0, 100, 3, 0.5f);
        _goAudio.PlayOneShot(putS);
        
        DoUseEnegry(MODE_GUN.Crash, false);
    }

    /// <summary>
    /// 6rocket
    /// </summary>
    private void Shot_Rocket()
    {

        foreach (GameObject child in _goChild)
        {
            child.GetComponent<FireChildC>().DoAttackRocket();
        }

        _goAudio.PlayOneShot(bulletS);
        DoUseEnegry(MODE_GUN.Heat, true);
    }

    /// <summary>
    /// 7Fire
    /// </summary>
    private void Shot_Fire()
    {
        PExpC shot4 = Instantiate(PFireP, _posShot, _rotOwn);
        shot4.EShot1((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) + Random.Range(-20, 20), Random.Range(2, 20), 0.2f);
        _goAudio.PlayOneShot(fireS);

        foreach (GameObject child in _goChild)
        {
            child.GetComponent<FireChildC>().DoAttackBress();
        }

        DoUseEnegry(MODE_GUN.Heat, false);
    }

    /// <summary>
    /// エネルギーを消費させる
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="isChargeShot"></param>
    private void DoUseEnegry(MODE_GUN mode,bool isChargeShot)
    {
        SetUseEnergy(mode,isChargeShot);

        if (!isChargeShot) SetCoolTimeToDefault(mode);

        if (GetWeaponState(mode).energy <= 0)SetIsLoad(mode,false);
    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void PlayBulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _posShot, _rotOwn);
        bulletEf.transform.parent = transform;
        bulletEf.EShot1(_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180, 0, 0.06f);
        bulletEf.transform.position += bulletEf.transform.up * 24;
    }

    /// <summary>
    /// 必殺発動
    /// </summary>
    public void PlayMagic()
    {
        _goAudio.PlayOneShot(magicuseS);
        _goAudio.PlayOneShot(exprosionS);
        GameData.TP -= 1;

        switch (_modeGun)
        {
            case MODE_GUN.Shining:
                PlayMagic_SuperRaser();
                break;

            case MODE_GUN.Physical:
                PlayMagic_BackMachinegun();
                break;

            case MODE_GUN.Crash:
                PlayMagic_MineCristal();
                break;

            case MODE_GUN.Heat:
                PlayMagic_Meteor();
                break;
        }
        
    }


    /// <summary>
    /// 必殺ビーム
    /// </summary>
    private void PlayMagic_SuperRaser()
    {
        for (short i = 0; i < 2; i++)
        {
            /*BeamMC shot = */
            Instantiate(BeamMP, _posOwn, _rotOwn).SetPos(i);
            //我が子にする
            //shot.transform.parent = transform;
            //shot.EShot1(i*120);
        }
    }

    /// <summary>
    /// 必殺マシンガン
    /// </summary>
    private void PlayMagic_BackMachinegun()
    {
        PMachineGunC machine = Instantiate(PMachinegunP, _posShot, _rotOwn);
        //我が子にする
        machine.transform.parent = transform;
    }

    /// <summary>
    /// 必殺ボム
    /// </summary>
    private void PlayMagic_MineCristal()
    {
        Quaternion rot = transform.localRotation;

        for (int i = 10; i < 640; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(i, _posShot.y, 0), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 300 - (i / 10), 10, 1.0f);
        }
        for (int i = 10; i < 480; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(_posShot.x, i, 0), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 300 - (i / 10), 10, 1.0f);
        }
        for (int i = 10; i < 360; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, _posShot + (new Vector3(Mathf.Sin(i * Mathf.Deg2Rad), Mathf.Cos(i * Mathf.Deg2Rad), 0) * 100), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 350, 10, 1.0f);
        }
    }

    /// <summary>
    /// 必殺ファイヤ
    /// </summary>
    private void PlayMagic_Meteor()
    {

        foreach (GameObject child in _goChild)
        {
            child.GetComponent<FireChildC>().DoAttackSpecial();
        }

    }


    /// <summary>
    /// チャイルド全消し
    /// </summary>
    /// <returns></returns>
    private void DeleteAllChild()
    {
        foreach (GameObject child in _goChild)
        {
            Destroy(child);
        }

        _goChild.Clear();
    }


    public MODE_GUN GetGunMode()
    {
        return _modeGun;
    }

    public bool CheckIsChanging()
    {
        return _isChanging;
    }

    /// <summary>
    /// 特定の武器のエナジー割合を返す
    /// </summary>
    public float CheckEnergy(MODE_GUN mode)
    {
        return 1.0f- GetWeaponState(mode).energy / 100.0f;
    }

    public bool CheckIsAbleChargeShot(MODE_GUN mode)
    {
        return GetWeaponState(mode).energy >= _enegyConsumptionChargeShot;
    }

    public void SetGunMode(MODE_GUN value)
    {
        _modeGun = value;
        _goAudio.PlayOneShot(ChangeS);
    }

    /// <summary>
    /// 特定の武器の現在のステータスを返す
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public GunStates GetWeaponState(MODE_GUN mode)
    {
        foreach(GunStates state in _statesWeapon)
        {
            if (state.mode == mode) return state;
        }
        return _statesWeapon[0];

    }

    /// <summary>
    /// 特定の武器のクールタイムをデフォルトに設定し発動
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private void SetCoolTimeToDefault(MODE_GUN mode)
    {
        for(int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].mode == mode)
            {
                _statesWeapon[i].cooltimeNow =_statesWeapon[i].cooltimeDefault;
                return;
            }
        }
    }

    /// <summary>
    /// 特定の武器のエネルギーを消費
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private void SetUseEnergy(MODE_GUN mode,bool isChargeShot)
    {
        for (int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].mode == mode)
            {

                _statesWeapon[i].energy -= isChargeShot ? _enegyConsumptionChargeShot : _statesWeapon[i].enegyConsumption;
                return;
            }
        }
    }

    /// <summary>
    /// 特定の武器の武器ロード状態を確認
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private bool CheckIsLoad(MODE_GUN mode)
    {
        for (int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].mode == mode)
            {
                return _statesWeapon[i].isLoaded;
            }
        }
        return false;
    }

    /// <summary>
    /// 特定の武器の武器ロード状態を変更
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private void SetIsLoad(MODE_GUN mode, bool isLoad)
    {
        for (int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].mode == mode)
            {
                _statesWeapon[i].isLoaded = isLoad;
                return;
            }
        }
    }

    public void SetAllEnergyHeal()
    {
        for (int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].energy < 90)
            {
                _statesWeapon[i].energy +=10f;
                
            }
            _statesWeapon[i].isLoaded = true;

        }
    }
}
