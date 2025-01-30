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
    private Vector3 pos, ppos, movepo;
    private Quaternion rot;
    public SpriteRenderer spriteRenderer;

    [SerializeField, Tooltip("LFR")]
    private Sprite[] normal, angry, last, death;

    public StaffRollC StaffPrefab;

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
        _eCoreC.BossLifeMode = 1;
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        _gameManaC._bossNowHp = firstHP;
        _gameManaC._bossMaxHp = firstHP;
        playerGO = GameObject.Find("Player");

        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    // Update is called once per frame
    void Update()
    {

        if (_eCoreC.BossLifeMode != 2) _gameManaC._bossNowHp = _eCoreC.TotalHp;
        pos = transform.position;
        ppos = playerGO.transform.position;
        damagePar = _eCoreC.TotalHp * 100 / firstHP;
        if (_eCoreC.BossLifeMode == 1)
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
                    movepo = new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0) - pos;
                    AllCoroutineStop();
                    _movingCoroutine = StartCoroutine("Hurricane");
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
            _eCoreC.BossLifeMode = 1;
        }


        //DeathAction
        if (_eCoreC.BossLifeMode == 2)
        {
            GameData.Star = true;
            GameData.TimerMoving = false;
            AllCoroutineStop();
            _gameManaC._bossNowHp = 0;
            k++;
            if (pos.y < 96) transform.position += new Vector3(0, Random.Range(1, 20), 0);
            else if (pos.y > 384) transform.position += new Vector3(0, Random.Range(-20, 0), 0);
            else transform.position += new Vector3(0, Random.Range(-10, 11), 0);
            if (pos.x > ppos.x) GameData.WindSpeed = Random.Range(100, 501);
            else GameData.WindSpeed = Random.Range(-500, -99);

            if (k == 20)
            {
                for (j = 0; j < 3; j++)
                {
                    for (i = 0; i < 30; i++)
                    {
                        float angle = GameData.GetAngle(pos, ppos);
                        angle += Random.Range(10, 350);
                        Quaternion rot = transform.localRotation;
                        EMissile1C shot = Instantiate(SonicPrefab, pos, rot);
                        shot.EShot1(angle, 10 + (10 * j), 0);
                    }
                }
                GameData.WindSpeed = 0;
                if (GameData.Round == GameData.GoalRound)
                {
                    Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0)).Summon(0);
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

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(1.0f);
        if (_eCoreC.EvoltionMode == 0 || _eCoreC.EvoltionMode == 1)
        {
            _action = Random.Range(0, 2);
            if (_action == 0) StartCoroutine("SonicRtoL");
            else if (_action == 1) StartCoroutine("SonicLtoR");
        }
        else if (_eCoreC.EvoltionMode == 2)
        {
            StartCoroutine("Hurricane");
        }

    }

    private IEnumerator SonicRtoL()
    {

        movepo = new Vector3(100, (Random.Range(0, 3) * 90) + 110, 0) - pos;
        for (i = 0; i < 100; i++)
        {
            transform.position += movepo / 100;
            if (GameData.WindSpeed < 32) GameData.WindSpeed += 2;
            if (i % 20 == 0)
            {
                SonicShot();
                if (_eCoreC.EvoltionMode == 1) SonicShot();
                _audioGO.PlayOneShot(sonicS);
            }
            yield return new WaitForSeconds(0.03f);
        }
        if (_eCoreC.EvoltionMode == 1) FlyingObj(0);
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    private IEnumerator SonicLtoR()
    {
        movepo = new Vector3(540, (Random.Range(0, 3) * 90) + 110, 0) - pos;
        for (i = 0; i < 100; i++)
        {
            transform.position += movepo / 100;
            if (GameData.WindSpeed > -32) GameData.WindSpeed -= 2;
            if (_eCoreC.EvoltionMode != 2)
            {
                if (i % ((2 - _eCoreC.EvoltionMode) * 10) == 0)
                {
                    SonicShot();
                    _audioGO.PlayOneShot(sonicS);
                }
            }
            yield return new WaitForSeconds(0.03f);
        }
        //if (_eCoreC.EvoltionMode == 1) FlyingObj(0);
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    private void SonicShot()
    {
        float angle = GameData.GetAngle(pos, ppos);
        angle += Random.Range(-10, 10);
        Quaternion rot = transform.localRotation;
        EMissile1C shot = Instantiate(SonicPrefab, pos, rot);
        shot.EShot1(angle, 0, 0.3f);
    }

    private IEnumerator Hurricane()
    {
        for (i = 0; i < Random.Range(8, 20); i++)
        {
            GameData.WindSpeed += 4;
            if (GameData.WindSpeed <= 100) transform.position += movepo / 25;

            if (pos.y < 96) transform.position += new Vector3(0, Random.Range(1, 20), 0);
            else if (pos.y > 384) transform.position += new Vector3(0, Random.Range(-20, 0), 0);
            else transform.position += new Vector3(0, Random.Range(-10, 11), 0);

            yield return new WaitForSeconds(0.03f);
        }

        if (GameData.WindSpeed > 100)
        {
            FlyingObj(Random.Range(0, 3));
        }
        _movingCoroutine = StartCoroutine("Hurricane");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objKind">
    /// 0=Wood
    /// 1=Cristal
    /// 2=Dead
    /// </param>
    private void FlyingObj(int objKind)
    {
        if (objKind == 0)
        {
            angle = Random.Range(-10, 0);
            Quaternion rot = transform.localRotation;
            rot.z = Random.Range(0, 360);
            ShurikenC shot = Instantiate(F1, new Vector3(-48, Random.Range(0, 480), 0), rot);
            shot.EShot1(angle, Random.Range(20, 50), 0.1f, Random.Range(5, 20));
            _audioGO.PlayOneShot(woodS);
        }
        else if (objKind == 1)
        {
            angle = Random.Range(-10, 0);
            Quaternion rot = transform.localRotation;
            rot.z = Random.Range(0, 360);
            ShurikenC shot = Instantiate(F2, new Vector3(-48, Random.Range(0, 480), 0), rot);
            shot.EShot1(angle, Random.Range(20, 50), 0.1f, Random.Range(5, 15));
            _audioGO.PlayOneShot(woodS);
        }
        else if (objKind == 2)
        {
            angle = Random.Range(-10, 0);
            Quaternion rot = transform.localRotation;
            rot.z = Random.Range(0, 360);
            ShurikenC shot = Instantiate(F3, new Vector3(-48, Random.Range(0, 480), 0), rot);
            shot.EShot1(angle, Random.Range(20, 50), 0.1f, Random.Range(5, 10));
            _audioGO.PlayOneShot(woodS);
        }
    }

    private void DeathAction()
    {

    }

    private void AllCoroutineStop()
    {
        StopAllCoroutines();
        _movingCoroutine = null;
    }
}
