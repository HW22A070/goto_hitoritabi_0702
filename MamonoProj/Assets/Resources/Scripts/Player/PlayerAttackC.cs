using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackC : MonoBehaviour
{
    private Vector3 pos;
    private Quaternion rot;

    /// <summary>
    /// プレイヤーのグラフィックかちょっとでかいぶん銃の場所を下げる値
    /// </summary>
    private Vector3 _shotPos;

    /// <summary>
    /// クールタイムセット
    /// </summary>
    private float[] _defaultCoolTime = { 1.8f, 0.1f, 2.0f, 0.08f, 1.5f, 0.6f, 4.0f, 0.03f };

    /// <summary>
    /// 現在のクールタイム
    /// </summary>
    private float[] shotdown = { 0, 0, 0, 0, 0, 0, 0, 0 };

    /// <summary>
    /// 0=ビーム
    /// 1=バレット
    /// 2=ボム
    /// 3=バーン
    /// </summary>
    private int _gunMode = 1;
    private int plnum = 0;

    /// <summary>
    /// 遠距離武器がロードされているか
    /// </summary>
    private bool[] _isLoaded = { true, true, true, true };

    /// <summary>
    /// 変形中
    /// </summary>
    private bool _isChanging;

    [SerializeField, Tooltip("弾アタッチ")]
    private PMissile PBulletP;

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
    /// ロードされたやつ管理用
    /// </summary>
    private List<GameObject> _goChild = new List<GameObject> { };

    [SerializeField, Tooltip("サウンド")]
    private AudioClip shotS,magicuseS, exprosionS, bulletS, putS, fireS, ChangeS, SlashS;

    [SerializeField, Tooltip("ロードサウンド")]
    private AudioClip[] _loadS;


    private PlayerC _scPlayer;
    private GameObject _goCamera;
    private AudioSource _audioGO;

    // Start is called before the first frame update
    void Start()
    {
        _scPlayer = GetComponent<PlayerC>();
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        _goCamera = GameObject.Find("Main Camera");

        pos = transform.position;
        Summon_Child();

        if (GameData.Difficulty >= 3)
        {
            for (int i = 0; i < _defaultCoolTime.Length; i++) _defaultCoolTime[i] *= 0.3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
        rot = transform.localRotation;

        _shotPos = pos - (transform.up * 4);

        for (int i = 0; i < 8; i++)
        {
            if (shotdown[i] != 0)
            {
                if (_gunMode != i / 2) shotdown[i] -= Time.deltaTime * 2;
                else shotdown[i] -= Time.deltaTime;
            }
        }

        //ロードされたらSE
        for (int i = 0; i < 4; i++)
        {
            if (shotdown[i * 2] <= 0 && !_isLoaded[i])
            {
                _audioGO.PlayOneShot(_loadS[i]);
                _isLoaded[i] = true;
            }
        }
    }

    private void FixedUpdate()
    {
        //銃モード中は照準出す
        if (_gunMode == 1)
        {
            GameObject flontObj = _scPlayer.GetFlontEnemy();
            if (flontObj != null)
            {
                Vector3 flontObjPos = GameData.FixPosition(flontObj.transform.position, 32, 32);
                Instantiate(_prfbTarget, flontObjPos, rot).EShot1(0, 0, 0.03f);
            }
        }
    }

    private void Summon_Child()
    {
        switch (_gunMode)
        {
            case 0:
                Change_Beam();
                break;
            case 1:
                Change_Bullet();
                break;
            case 2:
                Change_Bomb();
                break;
            case 3:
                Change_Burn();
                break;
        }
    }


    /// <summary>
    /// 変形
    /// </summary>
    /// <param name="modevalue"></param>
    public void GunModeChange(int modevalue)
    {

        if (modevalue != _gunMode)
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
    private IEnumerator ChangingAnim(int modevalue)
    {
        _audioGO.PlayOneShot(ChangeS);
        yield return new WaitForSeconds(0.2f);
        _gunMode = modevalue;
        if (_isLoaded[modevalue]) _audioGO.PlayOneShot(_loadS[_gunMode]);
        
        DeleteChild();
        Summon_Child();
        _scPlayer.PlayerAnimReset();

        _isChanging = false;
    }



    /// <summary>
    /// ビームチャイルド呼び出し
    /// </summary>
    private void Change_Beam()
    {
        for (int hoge = 0; hoge < 3; hoge++)_goChild.Add(Instantiate(_prhbBeamChild, pos, rot));
        for(int hoge=0;hoge< _goChild.Count;hoge++)_goChild[hoge].GetComponent<BeamChildC>().SetOfset(hoge * (360/_goChild.Count));
    }

    /// <summary>
    /// バレットチャイルド呼び出し
    /// </summary>
    private void Change_Bullet()
    {
        for (int hoge = 0; hoge < 2; hoge++)_goChild.Add(Instantiate(_prhbBulletChild, pos, rot));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BulletChildC>().SetOfset(new Vector3(0, hoge * 32, 0));

    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Bomb()
    {
        for (int hoge = 0; hoge < 1; hoge++) _goChild.Add(Instantiate(_prhbBombChild, pos, rot));
        for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<BombChildC>().SetOfset(new Vector3(0, (hoge+1)*48, 0));
    }

    /// <summary>
    /// ボムチャイルド呼び出し
    /// </summary>
    private void Change_Burn()
    {
        for (int hoge = 0; hoge < 1; hoge++)_goChild.Add(Instantiate(_prhbBurnChild, pos, rot));
        //for (int hoge = 0; hoge < _goChild.Count; hoge++) _goChild[hoge].GetComponent<FireChildC>();
    }

    /// <summary>
    /// 遠距離攻撃
    /// </summary>
    public void Fire()
    {
        if (!_isChanging)
        {
            if (_gunMode < 1 && shotdown[0] <= 0) Shot_Raser();
            else if (_gunMode >= 1 && _gunMode < 2 && shotdown[2] <= 0) Shot_Bullet();
            else if (_gunMode >= 2 && _gunMode < 3 && shotdown[4] <= 0) Shot_Drop();
            else if (_gunMode >= 3 && _gunMode < 4 && shotdown[6] <= 0) Shot_Rocket();
        }
    }

    /// <summary>
    /// 近距離攻撃
    /// </summary>
    public void Fire2()
    {
        if (!_isChanging)
        {
            if (_gunMode < 1 && shotdown[1] <= 0) Shot_Slash();
            else if (_gunMode >= 1 && _gunMode < 2 && shotdown[3] <= 0) Shot_ShotGun();
            else if (_gunMode >= 2 && _gunMode < 3 && shotdown[5] <= 0) Shot_MineSield();
            else if (_gunMode >= 3 && _gunMode < 4 && shotdown[7] <= 0) Shot_Fire();
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
            
        _audioGO.PlayOneShot(shotS);
        _isLoaded[0] = false;
        shotdown[0] = _defaultCoolTime[0];
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

        PExpC prefab = Instantiate(PSlashP, _shotPos, rot);
        prefab.EShot1(180 + (PlayerC.muki * 180), 0, 0.08f);
        prefab.transform.position += prefab.transform.up * 64;
        _audioGO.PlayOneShot(SlashS);
        shotdown[1] = _defaultCoolTime[1];
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

        _goCamera.GetComponent<CameraC>().StartShakeVertical(4, 5);
        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _audioGO.PlayOneShot(bulletS);
        shotdown[2] = _defaultCoolTime[2];
    }

    /// <summary>
    /// 3Bullet_MachineGun
    /// </summary>
    private void Shot_ShotGun()
    {
        PMissile prefab = Instantiate(PBulletP, _shotPos, rot);
        prefab.Shot(180 + (PlayerC.muki * 180) + Random.Range(-4, 4), 70, 0);
        prefab.transform.position += prefab.transform.up * 16;
        _goCamera.GetComponent<CameraC>().StartShakeVertical(2, 4);
        BulletEffect();

        //Instantiate(PBulletExpP, _shotPos, rot).EShot1(180 + (muki * 180) + Random.Range(-4, 4), 70, 0.1f);
        _audioGO.PlayOneShot(bulletS);
        shotdown[3] = _defaultCoolTime[3];
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

        _audioGO.PlayOneShot(putS);
        _isLoaded[2] = false;
        shotdown[4] = _defaultCoolTime[4];
    }

    /// <summary>
    /// 5Bomb_Sield
    /// </summary>
    private void Shot_MineSield()
    {
        PBombC shot = Instantiate(PMinePutP, new Vector3(_shotPos.x - 48 + (96 * PlayerC.muki), _shotPos.y, 0), rot);
        shot.EShot1(270, 0, 0, 100, 3, 0.5f);
        _audioGO.PlayOneShot(putS);
        shotdown[5] = _defaultCoolTime[5];
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

        _audioGO.PlayOneShot(bulletS);
        _isLoaded[3] = false;
        shotdown[6] = _defaultCoolTime[6];
    }

    /// <summary>
    /// 7Fire
    /// </summary>
    private void Shot_Fire()
    {
        PExpC shot4 = Instantiate(PFireP, _shotPos, rot);
        shot4.EShot1(180 + (PlayerC.muki * 180) + Random.Range(-20, 20), Random.Range(2, 20), 0.2f);
        _audioGO.PlayOneShot(fireS);

        foreach (GameObject child in _goChild)
        {
            child.GetComponent<FireChildC>().DoAttackBress();
        }


        shotdown[7] = _defaultCoolTime[7];
    }

    /// <summary>
    /// 銃弾発射光エフェクト
    /// </summary>
    private void BulletEffect()
    {
        //発射エフェクト
        ExpC bulletEf = Instantiate(_prhbBulletShot, _shotPos, rot);
        bulletEf.transform.parent = transform;
        bulletEf.EShot1(180 + (PlayerC.muki * 180), 0, 0.06f);
        bulletEf.transform.position += bulletEf.transform.up * 24;
    }

    /// <summary>
    /// 必殺発動
    /// </summary>
    public void Magic()
    {
        _audioGO.PlayOneShot(magicuseS);
        _audioGO.PlayOneShot(exprosionS);
        GameData.TP -= 1;
        if (_gunMode < 1 && shotdown[1] <= 0) Magic_SuperRaser();
        else if (_gunMode >= 1 && _gunMode < 2 && shotdown[3] <= 0) Magic_BackMachinegun();
        else if (_gunMode >= 2 && _gunMode < 3 && shotdown[5] <= 0) Magic_MineCristal();
        else if (_gunMode >= 3 && _gunMode < 4 && shotdown[7] <= 0) Magic_Meteor();
    }


    /// <summary>
    /// 必殺ビーム
    /// </summary>
    private void Magic_SuperRaser()
    {

        for (short i = 0; i < 2; i++)
        {
            /*BeamMC shot = */
            Instantiate(BeamMP, pos, rot).SetPos(i);
            //我が子にする
            //shot.transform.parent = transform;
            //shot.EShot1(i*120);
        }
    }

    /// <summary>
    /// 必殺マシンガン
    /// </summary>
    private void Magic_BackMachinegun()
    {
        PMachineGunC machine = Instantiate(PMachinegunP, _shotPos, rot);
        //我が子にする
        machine.transform.parent = transform;
    }

    /// <summary>
    /// 必殺ボム
    /// </summary>
    private void Magic_MineCristal()
    {
        Quaternion rot = transform.localRotation;

        for (int i = 10; i < 640; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(i, _shotPos.y, 0), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 300 - (i / 10), 10, 1.0f);
        }
        for (int i = 10; i < 480; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, new Vector3(_shotPos.x, i, 0), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 300 - (i / 10), 10, 1.0f);
        }
        for (int i = 10; i < 360; i += 40)
        {
            PBombC shot = Instantiate(PMinecristalP, _shotPos + (new Vector3(Mathf.Sin(i * Mathf.Deg2Rad), Mathf.Cos(i * Mathf.Deg2Rad), 0) * 100), rot);
            shot.EShot1(Random.Range(0, 360), 0, 0.001f, 350, 10, 1.0f);
        }
    }

    /// <summary>
    /// 必殺ファイヤ
    /// </summary>
    private void Magic_Meteor()
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
    private void DeleteChild()
    {
        foreach (GameObject child in _goChild)
        {
            Destroy(child);
        }

        _goChild.Clear();
    }


    public int GetGunMode()
    {
        return _gunMode;
    }

    public float GetShotdown(int value)
    {
        return shotdown[value];
    }

    public bool GetIsChanging()
    {
        return _isChanging;
    }

    /// <summary>
    /// 特定の武器のクールタイム割合を返す
    /// </summary>
    public float CheckCoolTime(int weaponValue)
    {
        return shotdown[weaponValue * 2] / _defaultCoolTime[weaponValue * 2];
    }

    public void SetGunMode(int value)
    {
        _gunMode = value;
        _audioGO.PlayOneShot(ChangeS);
    }
}
