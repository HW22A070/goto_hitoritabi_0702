using EnumDic.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaneC : MonoBehaviour
{
    private int firstHP = 0;
    private float damagePar = 100;
    private int i = 0;
    private int j = 0;
    private int k = 0;
    private int _action;

    private float angle;

    private bool _islastMode;

    private int _windowGraValue = 4;

    private GameManagement _gameManaC;
    private Vector3 _posOwn, _posPlayer, _moveGoal;
    private Quaternion rot;
    public SpriteRenderer spriteRenderer;

    [SerializeField, Tooltip("LFR")]
    private Sprite[] normal, angry, last, death;

    public ClearEffectC StaffPrefab;

    public ShurikenC F1, F2, F3;
    public EMissile1C SonicPrefab;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip sonicS, woodS, deathexpS, shoutS;

    private GameObject playerGO;

    /// <summary>
    /// バリア
    /// </summary>
    [SerializeField]
    private GameObject _barrier;

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
        GameData.WindSpeed = 32;
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();

        _eCoreC = GetComponent<ECoreC>();
        for (int j = 0; j < _eCoreC.hp.Length; j++)
        {
            firstHP += _eCoreC.hp[j];
        }
        _eCoreC.IsBoss = true;
        _eCoreC.BossLifeMode = MODE_LIFE.Fight;
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        _gameManaC._bossNowHp = firstHP;
        _gameManaC._bossMaxHp = firstHP;
        playerGO = GameObject.Find("Player");

        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    // Update is called once per frame
    void Update()
    {

        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead) _gameManaC._bossNowHp = _eCoreC.TotalHp;
        _posOwn = transform.position;
        _posPlayer = playerGO.transform.position;
        damagePar = _eCoreC.TotalHp * 100 / firstHP;
        if (_eCoreC.BossLifeMode == MODE_LIFE.Fight)
        {
            //Normal
            if (_eCoreC.EvoltionMode == 0)
            {
                _eCoreC.EvoltionMode = 0;
                spriteRenderer.sprite = normal[_windowGraValue];
            }
            //Angly
            else if (_eCoreC.EvoltionMode == 1)
            {
                _barrier.SetActive(false);
                _eCoreC.EvoltionMode = 1;
                spriteRenderer.sprite = angry[_windowGraValue];
            }
            //Rage
            else if (_eCoreC.EvoltionMode == 2)
            {
                _eCoreC.EvoltionMode = 2;
                spriteRenderer.sprite = last[_windowGraValue];
                if (!_islastMode)
                {
                    _islastMode = true;
                    GameData.WindSpeed = 0;
                    _audioGO.PlayOneShot(shoutS);
                    _moveGoal = new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0) - _posOwn;
                    AllCoroutineStop();
                    _movingCoroutine = StartCoroutine(StartHurricane());
                }
            }
        }

        if (GameData.WindSpeed < -30) _windowGraValue = 0;
        else if (-30 <= GameData.WindSpeed && GameData.WindSpeed < -7) _windowGraValue = 1;
        else if (-7 <= GameData.WindSpeed && GameData.WindSpeed < 7) _windowGraValue = 2;
        else if (7 <= GameData.WindSpeed && GameData.WindSpeed < 30) _windowGraValue = 3;
        else if (30 <= GameData.WindSpeed) _windowGraValue = 4;

    }

    void FixedUpdate()
    {
        //SummonAction
        if (_eCoreC.BossLifeMode == 0)
        {
            GameData.WindSpeed = 10;
            _eCoreC.BossLifeMode = MODE_LIFE.Fight;
        }


        //DeathAction
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;
            AllCoroutineStop();
            _gameManaC._bossNowHp = 0;
            k++;
            if (_posOwn.y < 96) transform.position += new Vector3(0, Random.Range(1, 20), 0);
            else if (_posOwn.y > 384) transform.position += new Vector3(0, Random.Range(-20, 0), 0);
            else transform.position += new Vector3(0, Random.Range(-10, 11), 0);
            if (_posOwn.x > _posPlayer.x) GameData.WindSpeed = Random.Range(100, 501);
            else GameData.WindSpeed = Random.Range(-500, -99);

            if (k == 20)
            {
                for (j = 0; j < 3; j++)
                {
                    for (i = 0; i < 30; i++)
                    {
                        float angle = GameData.GetAngle(_posOwn, _posPlayer);
                        angle += Random.Range(10, 350);
                        Quaternion rot = transform.localRotation;
                        EMissile1C shot = Instantiate(SonicPrefab, _posOwn, rot);
                        shot.EShot1(angle, 10 + (10 * j), 0);
                    }
                }
                GameData.WindSpeed = 0;
                if (GameData.Round == GameData.GoalRound)
                {
                    Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    playerGO.GetComponent<PlayerC>().StageMoveAction();
                }
                _audioGO.PlayOneShot(sonicS);
                _eCoreC.SummonItems();
                Destroy(gameObject);
            }
        }
    }

    private enum MODE_ATTTACK
    {
        SonicLtoR,
        SonicRtoL,
        Hurricane,
    }

    /// <summary>
    /// 飛来物の種類
    /// </summary>
    private enum KIND_FLYINGOBJ
    {
        Wood,
        Cristal,
        DeadMonstar
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(1.0f);

        switch (_eCoreC.EvoltionMode)
        {
            case 0:
            case 1:
                switch ((MODE_ATTTACK)Random.Range(0, 2))
                {
                    case MODE_ATTTACK.SonicLtoR:
                        _movingCoroutine = StartCoroutine(SonicLtoR());
                        break;

                    case MODE_ATTTACK.SonicRtoL:
                        _movingCoroutine = StartCoroutine(SonicRtoL());
                        break;
                }
                break;

            case 2:
                _movingCoroutine = StartCoroutine(StartHurricane());
                break;
        }

    }

    /// <summary>
    /// 左向きに風を起こす
    /// </summary>
    /// <returns></returns>
    private IEnumerator SonicRtoL()
    {

        _moveGoal = new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0) - _posOwn;

        for (i = 0; i < 100; i++)
        {
            transform.position += _moveGoal / 100;

            //左向きに風を起こす
            if (GameData.WindSpeed < 32) GameData.WindSpeed += 2;
            if (i % 20 == 0)
            {
                ShotSonic();
                //第二形態であれば二つ発射
                if (_eCoreC.EvoltionMode == 1) ShotSonic();
                _audioGO.PlayOneShot(sonicS);
            }
            yield return new WaitForSeconds(0.03f);
        }

        //第二形態であれば飛来物を召喚
        if (_eCoreC.EvoltionMode == 1) ShotFlyingObj(KIND_FLYINGOBJ.Wood);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 右向きに風を起こす
    /// </summary>
    /// <returns></returns>
    private IEnumerator SonicLtoR()
    {
        _moveGoal = new Vector3(540, (Random.Range(0, 3) * 90) + 110, 0) - _posOwn;

        for (i = 0; i < 100; i++)
        {
            transform.position += _moveGoal / 100;
            if (GameData.WindSpeed > -32) GameData.WindSpeed -= 2;
            if (_eCoreC.EvoltionMode != 2)
            {
                if (i % ((2 - _eCoreC.EvoltionMode) * 10) == 0)
                {
                    ShotSonic();
                    _audioGO.PlayOneShot(sonicS);
                }
            }
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 衝撃波発射
    /// </summary>
    private void ShotSonic()
    {
        float angle = GameData.GetAngle(_posOwn, _posPlayer);
        angle += Random.Range(-10, 10);
        Quaternion rot = transform.localRotation;
        EMissile1C shot = Instantiate(SonicPrefab, _posOwn, rot);
        shot.EShot1(angle, 0, 0.3f);
    }

    /// <summary>
    /// 暴走
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartHurricane()
    {
        //飛来物発射までの時間稼ぎ
        for (i = 0; i < Random.Range(10, 20); i++)
        {
            //風加速
            GameData.WindSpeed += 4;
            if (GameData.WindSpeed <= 100) transform.position += _moveGoal / 25;

            //痙攣
            if (_posOwn.y < 96) transform.position += new Vector3(0, Random.Range(1, 20), 0);
            else if (_posOwn.y > 384) transform.position += new Vector3(0, Random.Range(-20, 0), 0);
            else transform.position += new Vector3(0, Random.Range(-10, 11), 0);

            yield return new WaitForSeconds(0.03f);
        }

        //風速が速ければ飛来物を飛ばす
        if (GameData.WindSpeed > 100)
        {
            ShotFlyingObj((KIND_FLYINGOBJ)Random.Range(0, 3));
        }
        _movingCoroutine = StartCoroutine(StartHurricane());
    }

    /// <summary>
    /// 飛来物を飛ばす
    /// </summary>
    /// <param name="objKind">
    /// 0=Wood
    /// 1=Cristal
    /// 2=Dead
    /// </param>
    private void ShotFlyingObj(KIND_FLYINGOBJ objKind)
    {

        angle = Random.Range(-10, 0);
        Quaternion rot = transform.localRotation;
        rot.z = Random.Range(0, 360);
        ShurikenC shot = Instantiate(F1, new Vector3(-48, Random.Range(0, 480), 0), rot);
        shot.EShot1(angle, Random.Range(10, 30), 0.1f, Random.Range(5, 20));
        _audioGO.PlayOneShot(woodS);

        switch (objKind)
        {
            case KIND_FLYINGOBJ.Wood:
                Instantiate(F1, new Vector3(-48, Random.Range(0, 480), 0), rot)
                    .EShot1(angle, Random.Range(10, 30), 0.1f, Random.Range(5, 20));
                break;

            case KIND_FLYINGOBJ.Cristal:
                Instantiate(F2, new Vector3(-48, Random.Range(0, 480), 0), rot)
                    .EShot1(angle, Random.Range(20, 50), 0.1f, Random.Range(5, 15));
                break;

            case KIND_FLYINGOBJ.DeadMonstar:
                Instantiate(F3, new Vector3(-48, Random.Range(0, 480), 0), rot)
                    .EShot1(angle, Random.Range(20, 50), 0.1f, Random.Range(5, 10));
                break;
        }
    }

    private void AllCoroutineStop()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }
}
