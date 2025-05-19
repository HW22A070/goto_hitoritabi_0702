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

    /// <summary>
    /// 必殺技強化中！
    /// </summary>
    private bool _isSpecial;


    [SerializeField, Tooltip("武器ステータス")]
    private GunStates[] _statesWeapon = new GunStates[4]{
        new GunStates{
            mode=MODE_GUN.Shining,
            energyChargeTick=1.33f,
            cooltimeDefault=0.2f,
            cooltimeSPDefault=0.06f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=4f,
            isLoaded=true
        },
        new GunStates{
            mode=MODE_GUN.Physical,
            energyChargeTick=1.2f,
            cooltimeDefault=0.08f,
            cooltimeSPDefault=0.3f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=1.5f,
            isLoaded=true
        },
        new GunStates{
            mode=MODE_GUN.Crash,
            energyChargeTick=1.8f,
            cooltimeDefault=0.6f,
            cooltimeSPDefault=1.2f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=8f,
            isLoaded=true
        },
        new GunStates{
            mode=MODE_GUN.Heat,
            energyChargeTick=0.6f,
            cooltimeDefault=0.03f,
            cooltimeSPDefault=0.4f,
            cooltimeNow=0f,
            energy=100f,
            enegyConsumption=1f,
            isLoaded=true
        },
    };

    private float _enegyConsumptionChargeShot = 75f;

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
    private PMissile _prhbMissile,_prfbRifle;

    [SerializeField, Tooltip("弾アタッチ、")]
    private PHowitzerC PBombP, _prfbPBeamSP;

    [SerializeField, Tooltip("弾アタッチ、")]
    private PBombC PMinePutP, PMinecristalP;

    [SerializeField, Tooltip("弾アタッチ")]
    private PExpC PFireP, PSlashP;

    [SerializeField, Tooltip("弾アタッチ")]
    private EMissile1C TPwazaPrefab;

    [SerializeField, Tooltip("エフェクトアタッチ")]
    private ExpC _prhbBulletShot;

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

    [SerializeField]
    private ExpC _prfbFlashEffect;

    /// <summary>
    /// ロードされたチャイルド管理用
    /// </summary>
    private List<GameObject> _goChild = new List<GameObject> { };

    [SerializeField, Tooltip("サウンド")]
    private AudioClip shotS, magicuseS, exprosionS, bulletS, putS, fireS, ChangeS, SlashS;

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
        if (GameData.Difficulty == MODE_DIFFICULTY.Berserker)
        {
            for (int i = 0; i < _statesWeapon.Length; i++)
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

                if (_scPlayer.CheckIsAlive())
                {
                    //銃モード中は照準出す
                    GameObject flontObj = _scPlayer.GetFlontEnemy();
                    if (flontObj != null)
                    {
                        Vector3 flontObjPos = GameData.FixPosition(flontObj.transform.position, 32, 32);
                        Instantiate(_prfbTarget, flontObjPos, _rotOwn).ShotEXP(0, 0, 0.03f);
                    }
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
                    if (_statesWeapon[mode].cooltimeNow <= 0) _statesWeapon[mode].energy += _statesWeapon[mode].energyChargeTick;
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

        if (!_scPlayer.CheckIsAlive())
        {
            DeleteAllChild();
        }
    }

    public void Summon_Child()
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
    public void SetGunModeChange(MODE_GUN modevalue,float timeTransformation)
    {
        if (!_isChanging)
        {
            if (modevalue != _modeGun)
            {
                _isChanging = true;
                StartCoroutine(ChangingAnim(modevalue,timeTransformation));
            }
        }

    }

    /// <summary>
    /// 変形ディレイ
    /// </summary>
    /// <param name="modevalue"></param>
    /// <returns></returns>
    private IEnumerator ChangingAnim(MODE_GUN modevalue, float timeTransformation)
    {
        _goAudio.PlayOneShot(ChangeS);
        yield return new WaitForSeconds(timeTransformation);
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
        for (int hoge = 0; hoge < 3; hoge++) _goChild.Add(Instantiate(_prhbBeamChild, _posOwn, _rotOwn));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BeamChildC>().SetOfset(hoge * (360 / _goChild.Count));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<ChildsCoreC>().SetParent(gameObject);
    }

    /// <summary>
    /// バレットチャイルド呼び出し
    /// </summary>
    private void Change_Bullet()
    {
        for (int hoge = 0; hoge < 2; hoge++) _goChild.Add(Instantiate(_prhbBulletChild, _posOwn, _rotOwn));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BulletChildC>().SetOfset(new Vector3(0, hoge * 32, 0));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<ChildsCoreC>().SetParent(gameObject);
    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Bomb()
    {
        for (int hoge = 0; hoge < 1; hoge++) _goChild.Add(Instantiate(_prhbBombChild, _posOwn, _rotOwn));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BombChildC>().SetOfset(new Vector3(0, (hoge + 1) * 48, 0));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<ChildsCoreC>().SetParent(gameObject);
    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Burn()
    {
        for (int hoge = 0; hoge < 1; hoge++) _goChild.Add(Instantiate(_prhbBurnChild, _posOwn, _rotOwn));
        //for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<FireChildC>();
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<ChildsCoreC>().SetParent(gameObject);
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
                        Shot_MachineGun();
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

        if (!_isSpecial)
        {
            foreach (GameObject child in _goChild)
            {
                child.GetComponent<BeamChildC>().DoAttackRaser();
            }
        }
        else
        {
            foreach (GameObject child in _goChild)
            {
                child.GetComponent<BeamChildC>().DoPutBeamCristal();
            }
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

        if (!_isSpecial)
        {
            foreach (GameObject child in _goChild)
            {
                child.GetComponent<BeamChildC>().DoAttackSlash();
            }

            PExpC prefab = Instantiate(PSlashP, _posShot, _rotOwn);
            prefab.ShotEXP(_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180, 0, 0.08f);
            prefab.transform.position += prefab.transform.up * 64;
            _goAudio.PlayOneShot(SlashS);
        }

        else
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(_prfbPBeamSP, new Vector3(_posShot.x + (_scPlayer.CheckPlayerAngleIsRight() ? 24 : -24), _posShot.y+Random.Range(-16,16), 0), _rotOwn)
                    .ShotHowitzer(_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180, 64 + i * 32, 0, 100, 0, 0.1f);
            }

            _goAudio.PlayOneShot(shotS);
        }

        DoUseEnegry(MODE_GUN.Shining, false);

    }

    /// <summary>
    /// 2Bullet_Rifle
    /// </summary>
    private void Shot_Bullet()
    {
        if (!_isSpecial)
        {
            foreach (GameObject child in _goChild)
            {
                child.GetComponent<BulletChildC>().DoAttackSniper();
            }
            _goAudio.PlayOneShot(bulletS);
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(4, 5);
        }

        else
        {
            foreach (GameObject child in _goChild)
            {
                child.GetComponent<BulletChildC>().DoAttackRailGun();
            }

            Instantiate(_prfbFlashEffect, GameData.WindowSize / 2, Quaternion.Euler(0, 0, 0)).ShotEXP(0, 0, 0.1f);
            _goAudio.PlayOneShot(exprosionS);
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 10);
        }

        DoUseEnegry(MODE_GUN.Physical, true);

    }

    /// <summary>
    /// 3Bullet_MachineGun
    /// </summary>
    private void Shot_MachineGun()
    {
        if (!_isSpecial)
        {
            PMissile prefab = Instantiate(_prhbMissile, _posShot, _rotOwn);
            prefab.ShotMissle((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) + Random.Range(-4, 4), 70, 0);
            prefab.transform.position += prefab.transform.up * 16;
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(2, 4);
        }
        else
        {
            for (int i = -10; i<=10; i+=4){
                PMissile prefab = Instantiate(_prfbRifle, _posShot, _rotOwn);
                prefab.ShotMissle((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) +i, 0, 32);
                prefab.transform.position += prefab.transform.up * 128;
            }
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(3, 2);
            _goAudio.PlayOneShot(exprosionS);
        }

        PlayBulletEffect();
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
        if (!_isSpecial)
        {

            Instantiate(PMinePutP, new Vector3(_posShot.x + (_scPlayer.CheckPlayerAngleIsRight() ? 48 : -48), _posShot.y+24, 0), _rotOwn)
                .ShotBomb(270, 0, 0, 100,64);
        }
        else
        {
            for (int i = 1; i < 36; i += 4)
            {
                Instantiate(PMinecristalP, _posShot + (new Vector3(Mathf.Sin(i*10 * Mathf.Deg2Rad), Mathf.Cos(i* 10 * Mathf.Deg2Rad), 0) * 64), _rotOwn)
                    .ShotBomb(90, 0, 0.003f, 100-i,64);
            }
        }
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
        if (!_isSpecial)
        {
            PExpC shot4 = Instantiate(PFireP, _posShot, _rotOwn);
            shot4.ShotEXP((_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180) + Random.Range(-20, 20), Random.Range(2, 20), 0.2f);
        }

        else
        {
            for(int i = 0; i < 20; i++)
            {
                float ofsetX = Random.Range(-128, 128);
                PExpC shot4 = Instantiate(PFireP, _posShot+new Vector3(ofsetX, -32*Mathf.Cos(ofsetX*Mathf.Deg2Rad),0), _rotOwn);
                shot4.ShotEXP(90+ofsetX/16, Random.Range(10, 30), Random.Range(0.2f, 0.5f));
            }
            _goAudio.PlayOneShot(exprosionS);
            _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(3, 2);
        }
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
    private void DoUseEnegry(MODE_GUN mode, bool isChargeShot)
    {
        SetUseEnergy(mode, isChargeShot);

        if (!isChargeShot) SetCoolTimeToDefault(mode);

        if (GetWeaponState(mode).energy <= 0) SetIsLoad(mode, false);
    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void PlayBulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _posShot, _rotOwn);
        bulletEf.transform.parent = transform;
        bulletEf.ShotEXP(_scPlayer.CheckPlayerAngleIsRight() ? 0 : 180, 0, 0.06f);
        bulletEf.transform.position += bulletEf.transform.up * 24;
    }

    /// <summary>
    /// 必殺技発動！
    /// </summary>
    public void PlayMagic()
    {
        _goAudio.PlayOneShot(magicuseS);
        _goAudio.PlayOneShot(exprosionS);
        _scPlayer.SetUseTP(1);

        _isSpecial = true;
    }


    /// <summary>
    /// チャイルド全消し
    /// </summary>
    /// <returns></returns>
    private void DeleteAllChild()
    {
        if (_goChild.Count > 0)
        {
            foreach (GameObject child in _goChild)
            {
                Destroy(child);
            }
            _goChild.Clear();
        }

    }


    public MODE_GUN GetGunMode() => _modeGun;

    public bool CheckIsChanging()
    {
        return _isChanging;
    }

    /// <summary>
    /// 特定の武器のエナジー割合を返す
    /// </summary>
    public float CheckEnergy(MODE_GUN mode)
    {
        return 1.0f - GetWeaponState(mode).energy / 100.0f;
    }

    public bool CheckIsAbleChargeShot(MODE_GUN mode)
    {
        return GetWeaponState(mode).energy >= _enegyConsumptionChargeShot;
    }

    /// <summary>
    /// 特定の武器の現在のステータスを返す
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public GunStates GetWeaponState(MODE_GUN mode)
    {
        foreach (GunStates state in _statesWeapon)
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
        for (int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].mode == mode)
            {
                _statesWeapon[i].cooltimeNow = _isSpecial? _statesWeapon[i].cooltimeSPDefault : _statesWeapon[i].cooltimeDefault;
                return;
            }
        }
    }

    /// <summary>
    /// 特定の武器のエネルギーを消費
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private void SetUseEnergy(MODE_GUN mode, bool isChargeShot)
    {
        for (int i = 0; i < _statesWeapon.Length; i++)
        {
            if (_statesWeapon[i].mode == mode)
            {
                if (isChargeShot) _statesWeapon[i].energy -= _enegyConsumptionChargeShot;
                else
                {
                    if (!_isSpecial)
                    {
                        _statesWeapon[i].energy -=  _statesWeapon[i].enegyConsumption;
                    }
                }
 
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
                _statesWeapon[i].energy += 10f;

            }
            _statesWeapon[i].isLoaded = true;

        }
    }

    public bool CheckIsSpecial() => _isSpecial;

    public void SetIsSpecial(bool special) => _isSpecial = special;
}
