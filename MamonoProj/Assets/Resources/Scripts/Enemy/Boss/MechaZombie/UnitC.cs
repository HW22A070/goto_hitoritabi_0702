using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitC : MonoBehaviour
{
    /// <summary>
    /// 自信の座標
    /// </summary>
    private Vector3 _pos;

    /// <summary>
    /// プレイヤーの座標
    /// </summary>
    private Vector3 _ppos;

    /// <summary>
    /// めかぞんび座標
    /// </summary>
    private Vector3 _mpos;

    /// <summary>
    /// 銃口座標
    /// </summary>
    private Vector3 _shotPos;

    /// <summary>
    /// 収納座標
    /// </summary>
    private Vector3 _posDefault;


    private GameObject playerGO;

    private float _angle, _ownAngle,_ofset=0;

    /// <summary>
    /// 0=Normal
    /// 1=Gun
    /// 2=Rocket
    /// </summary>
    private int _modeAttack=0;

    private bool _isLeader;

    private GameObject _goMechaZombie;

    [SerializeField]
    private Sprite _spNormal, _spMachineGun,_spRocket, _spBeam,_spShotGun,_spRailGun, _spFirePlace,_spRocketCannon2,_spFireSummoner;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    [SerializeField]
    private AudioClip missileS, gunS, fireS,_moveS,_moveS2,_raserS;


    [SerializeField]
    private EMissile1C Ballet1P,_prfbRaserFat;
    [SerializeField]
    private HomingC RocketPrefab;
    [SerializeField]
    private GuardC Guard;
    [SerializeField]
    private ExpC _prfbLightningEffect, _prhbTargetEffect,_prfbFireEffect,_prfbEXPEffect;
    [SerializeField]
    private BombC _prfbRocket2;

    [SerializeField]
    private SpriteRenderer _srOwn;

    [SerializeField]
    private GameObject _goHPBar;

    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.Find("Player");
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        //_srOwn = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _pos = transform.position;
        _ppos = playerGO.transform.position;
        _mpos = _goMechaZombie.transform.position;
        switch (_modeAttack)
        {
            case 0:
                _shotPos = _pos;
                _goHPBar.SetActive(true);
                break;
            case 1:
                _goHPBar.SetActive(false);
                if (_srOwn.flipX) _shotPos = _pos - transform.right * 32;
                else _shotPos = _pos + transform.right * 32;

                break;
            case 2:
                _goHPBar.SetActive(false);
                _shotPos = _pos+transform.up*32;
                break;
            case 3:
                _goHPBar.SetActive(false);
                _shotPos = _pos + transform.right * 32;
                break;
            case 4:
                _goHPBar.SetActive(false);
                _shotPos = _pos;
                break;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //_angle = GameData.GetAngle(_shotPos, _ppos);

        _posDefault = _mpos + new Vector3(Mathf.Sin(_ofset * Mathf.Deg2Rad) * 96, Mathf.Cos(_ofset * Mathf.Deg2Rad) * 96, 0);

        if (_modeAttack==0)
        {
            _srOwn.sprite = _spNormal;
            transform.position = _posDefault;
            GetComponent<ECoreC>().SetMuteki(false);
        }
        else {
            GetComponent<ECoreC>().SetMuteki(true);
        }

        if (90 < transform.eulerAngles.z && transform.eulerAngles.z < 270)
        {
            _srOwn.flipY = true;
        }
        else
        {
            _srOwn.flipY = false;
        }
    }

    public void SetParent(GameObject parent)
    {
        _goMechaZombie = parent;
    }

    public void SetOfset(float ofset)
    {
        _ofset = ofset;
    }

    public void SetLeader(bool isLeader)
    {
        _isLeader = isLeader;
    }




    /// <summary>
    /// レーザー
    /// </summary>
    public void AttackRaser(Vector3 movePos, float delay, float delay2)
    {
        if (_isLeader) _audioGO.PlayOneShot(_moveS2);
        _srOwn.sprite = _spBeam;
        _modeAttack = 3;
        StartCoroutine(DoAttackRaser(movePos, delay, delay2));
    }
    private IEnumerator DoAttackRaser(Vector3 movePos, float delay, float delay2)
    {
        Instantiate(_prhbTargetEffect, _ppos, Quaternion.Euler(0, 0, 0)).EShot1(0, 0, 1.3f);
        Vector3 ppos = _ppos;
        float angle = 0;
        yield return new WaitForSeconds(delay);

        _audioGO.PlayOneShot(_moveS);
        for (int i = 0; i < 10; i++)
        {
            angle = GameData.GetAngle(_shotPos, ppos);
            transform.eulerAngles = transform.forward * angle;
            transform.position += GameData.GetSneaking(_pos, movePos, 2);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(delay2);

        if (_isLeader) _audioGO.PlayOneShot(_raserS);
        EMissile1C shot = Instantiate(_prfbRaserFat, _shotPos, transform.rotation);
        shot.EShot1(angle, 0, 1000);
        shot.transform.position += shot.transform.up * 320;

        yield return new WaitForSeconds(0.3f);

        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        _modeAttack = 0;
    }

    /// <summary>
    /// アイスレーザー
    /// </summary>
    public void AttackIceBeam(float deray)
    {

        _srOwn.sprite = _spBeam;
        _modeAttack = 3;
        StartCoroutine(DoIceBeam(deray));
    }
    private IEnumerator DoIceBeam(float deray)
    {
        for (int j = 0; j < 3; j++)
        {
            if (_isLeader)
            {
                _audioGO.PlayOneShot(_moveS2);
                _audioGO.PlayOneShot(_moveS);
            }

            Vector3 movePos = new Vector3(Random.Range(20, GameData.WindowSize.x - 20), 440, 0);

            Instantiate(_prhbTargetEffect, new Vector3(movePos.x, _ppos.y, 0), Quaternion.Euler(0, 0, 0)).EShot1(0, 0, deray + 0.3f);
            for (int i = 0; i < 10; i++)
            {
                transform.eulerAngles = transform.forward * 270;
                transform.position += GameData.GetSneaking(_pos, movePos, 2);
                yield return new WaitForSeconds(0.03f);
            }
            yield return new WaitForSeconds(deray);

            if(_isLeader)_audioGO.PlayOneShot(_raserS);
            EMissile1C shot = Instantiate(_prfbRaserFat, _shotPos, transform.rotation);
            shot.EShot1(270, 0, 1000);
            shot.transform.position += shot.transform.up * 320;
        }

        yield return new WaitForSeconds(0.6f);

        if (_isLeader) _audioGO.PlayOneShot(_moveS);
        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        _modeAttack = 0;
    }

    /// <summary>
    /// 連射攻撃
    /// </summary>
    public void AttackMachineGun(Vector3 movePos, float delay)
    {
        if (_isLeader) _audioGO.PlayOneShot(_moveS2);
        _srOwn.sprite = _spMachineGun;
        _modeAttack = 1;
        StartCoroutine(DoAttackMachineGun(movePos, delay));
    }
    private IEnumerator DoAttackMachineGun(Vector3 movePos, float delay)
    {
        yield return new WaitForSeconds(delay);

        _audioGO.PlayOneShot(_moveS);
        Instantiate(_prhbTargetEffect, _ppos, Quaternion.Euler(0, 0, 0)).EShot1(0, 0, 1.3f);
        Vector3 ppos = _ppos;
        float angle = 0;
        for (int i = 0; i < 30; i++)
        {
            angle = GameData.GetAngle(_shotPos, ppos);
            transform.eulerAngles = transform.forward * angle;
            transform.position += GameData.GetSneaking(_pos, movePos, 5);
            yield return new WaitForSeconds(0.03f);
        }

        for (int j = 0; j < 3; j++)
        {
            _audioGO.PlayOneShot(gunS);
            Instantiate(Ballet1P, _shotPos, transform.rotation).EShot1(angle, 30, 0);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        _modeAttack = 0;
    }

    /// <summary>
    /// 散弾
    /// </summary>
    public void AttackShotGun(float delay, bool isRight)
    {
        if (_isLeader)
        {
            _audioGO.PlayOneShot(_moveS2);
            _audioGO.PlayOneShot(_moveS);
        }
        _srOwn.sprite = _spShotGun;
        _modeAttack = 1;
        StartCoroutine(DoAttackShotGun(delay,isRight));
    }
    private IEnumerator DoAttackShotGun(float delay,bool isRight)
    {

        float angle = 0;
        GetComponent<SpriteRenderer>().flipX = !isRight;

        yield return new WaitForSeconds(delay);



        if (isRight) angle = 0;
        else angle = 180;

        for (int k = 0; k < 10; k++)
        {
            if (_isLeader) _audioGO.PlayOneShot(gunS);
            for (int j = 0; j < 5; j++)
            {
                Instantiate(Ballet1P, _shotPos, transform.rotation).EShot1(angle + (-5 + (2.5f * j)), 10, 0);
            }
            yield return new WaitForSeconds(0.15f);
        }
        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        GetComponent<SpriteRenderer>().flipX = false;
        _modeAttack = 0;
    }

    /// <summary>
    /// レールガン
    /// </summary>
    public void AttackRailGun(Vector3 movePos, float delay, bool isRight)
    {
        if (_isLeader) _audioGO.PlayOneShot(_moveS2);
        _srOwn.sprite = _spRailGun;
        _modeAttack = 4;
        StartCoroutine(DoAttackRailGun(movePos, delay,isRight));
    }
    private IEnumerator DoAttackRailGun(Vector3 movePos, float delay, bool isRight)
    {
        float angle = GameData.GetAngle(movePos, _ppos);
        _audioGO.PlayOneShot(_moveS);
        for (int i = 0; i < 10; i++)
        {
            transform.eulerAngles = transform.forward * angle;
            transform.position += GameData.GetSneaking(_pos, movePos, 2);
            yield return new WaitForSeconds(0.03f);
        }
        Quaternion rot = transform.rotation;
        for(float k=0;k< delay; k += 0.03f)
        {
            if (Random.Range(0, 10) == 0)
            {
                if(_isLeader)_audioGO.PlayOneShot(missileS);
                Instantiate(_prfbLightningEffect, _pos + new Vector3(Random.Range(-32, 32), Random.Range(-32, 32), 0), rot)
                    .EShot1(Random.Range(0, 360), 0, 0.09f);
            }
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(delay);

        yield return new WaitForSeconds(0.3f);

        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        _modeAttack = 0;
    }

    /// <summary>
    /// フロア炎上
    /// </summary>
    public void AttackDropFire(Vector3 movePos, float delay)
    {
        if (_isLeader) _audioGO.PlayOneShot(_moveS2);
        _srOwn.sprite = _spFirePlace;
        _modeAttack = 2;
        StartCoroutine(DoAttackDropFire(movePos, delay));
    }
    private IEnumerator DoAttackDropFire(Vector3 movePos, float delay)
    {
        yield return new WaitForSeconds(delay);

        _audioGO.PlayOneShot(_moveS);
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, movePos, 5);
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.6f);

        _audioGO.PlayOneShot(fireS);
        Instantiate(_prfbFireEffect, _shotPos, transform.rotation).EShot1(270, 0, 10.0f);
        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        _modeAttack = 0;
    }

    /// <summary>
    /// ローリング
    /// </summary>
    public void AttackRolling(float delay)
    {
        if (_isLeader)
        {
            _audioGO.PlayOneShot(_moveS2);
            _audioGO.PlayOneShot(_moveS);
        }
        _srOwn.sprite = _spFireSummoner;
        _modeAttack = 2;
        StartCoroutine(DoAttackRolling(delay));
    }
    private IEnumerator DoAttackRolling(float delay)
    {
        float rollValue =0;
        for (int i=0;i<60;i++)
        {
            Vector3 shotPos = _shotPos + new Vector3(Random.Range(-20,20), Random.Range(-20, 20),0);
            Instantiate(_prfbEXPEffect, shotPos, transform.rotation).EShot1(Random.Range(0, 360), 0.3f, 0.5f);
            transform.position = _mpos + new Vector3(Mathf.Sin((rollValue + _ofset )* Mathf.Deg2Rad) * 128, Mathf.Cos((rollValue + _ofset )* Mathf.Deg2Rad) *128, 0);
            yield return new WaitForSeconds(0.03f);
            rollValue += (float)i/3;
        }

        for (int i = 0; i < 150; i++)
        {
            Vector3 shotPos = _shotPos + new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), 0);
            Instantiate(_prfbEXPEffect, shotPos, transform.rotation).EShot1(Random.Range(0, 360), 0.3f, 0.5f);
            transform.position = _mpos + new Vector3(Mathf.Sin((rollValue + _ofset) * Mathf.Deg2Rad) * 128, Mathf.Cos((rollValue + _ofset) * Mathf.Deg2Rad) * 128, 0);
            yield return new WaitForSeconds(0.03f);
            rollValue += 20;
        }

        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        GetComponent<SpriteRenderer>().flipX = false;
        _modeAttack = 0;
    }

    /// <summary>
    /// ロケット
    /// </summary>
    public void AttackRocket(float ofset, float delay)
    {
        if (_isLeader) _audioGO.PlayOneShot(_moveS2);
        _srOwn.sprite = _spRocket;
        _modeAttack = 2;
        Vector3 posOfset = new Vector3(40 + ofset, _mpos.y, 0);
        StartCoroutine(DoAttackRocket(posOfset, delay));
    }
    private IEnumerator DoAttackRocket(Vector3 movePos, float delay)
    {
        float kaber = GameData.GetAngle(_shotPos, new Vector3(50, 460, 0));
        float kabel = GameData.GetAngle(_shotPos, new Vector3(590, 460, 0));

        yield return new WaitForSeconds(delay);
        _audioGO.PlayOneShot(_moveS);
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, movePos, 5);
            yield return new WaitForSeconds(0.03f);
        }

        for (int j = 0; j < 10; j++)
        {
            float angle = Random.Range(kabel, kaber);
            Quaternion rot = transform.localRotation;
            _audioGO.PlayOneShot(fireS);
            Instantiate(RocketPrefab, _shotPos, rot).EShot1(angle, 15, 300, 5, 0.5f, Random.Range(20, 30));
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.3f);
        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        _modeAttack = 0;
    }

    /// <summary>
    /// ロケット狙撃
    /// </summary>
    public void AttackRockerSniper(float delay, bool isRight, float angle)
    {
        if (_isLeader)
        {
            _audioGO.PlayOneShot(_moveS2);
            _audioGO.PlayOneShot(_moveS);
        }
        _srOwn.sprite = _spRocketCannon2;
        _modeAttack = 1;
        StartCoroutine(DoAttackRockerSniper(delay, isRight,angle));
    }
    private IEnumerator DoAttackRockerSniper(float delay, bool isRight,float angle)
    {

        GetComponent<SpriteRenderer>().flipX = !isRight;

        yield return new WaitForSeconds(delay);

        for (int k = 0; k < 50; k++)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (_isLeader) _audioGO.PlayOneShot(fireS);
                Instantiate(_prfbRocket2, _shotPos, transform.rotation).EShot1(angle + Random.Range(-5, 6), Random.Range(15,30), 0, 100, 5, 0.2f);
            }
            yield return new WaitForSeconds(0.03f);
        }
        transform.eulerAngles = Vector3.zero;
        for (int i = 0; i < 30; i++)
        {
            transform.position += GameData.GetSneaking(_pos, _posDefault, 5);
            yield return new WaitForSeconds(0.03f);
        }
        GetComponent<SpriteRenderer>().flipX = false;
        _modeAttack = 0;
    }


    public void Apoptosis()
    {
        Destroy(gameObject);
    }
}
