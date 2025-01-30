using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBossC : MonoBehaviour
{
    private int firstHP;
    private float damagePar;

    private float futurey;

    private bool _isLightning;

    private Vector3 pos, ppos;

    private float spritenumber = 0;

    public StaffRollC StaffPrefab;

    [SerializeField]
    private  SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public EMissile1C EMissile1Prefab;
    public ExpC LightningP;
    
    private GameManagement _gameManaC;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip shotS,chargeS,effectS,expS;

    [SerializeField,Tooltip("PlayerGameObject")]
    private GameObject playerGO;

    /// <summary>
    /// 動作中のコルーチン
    /// </summary>
    private Coroutine _movingCoroutine;

    /// <summary>
    /// ECoreCのコンポーネント
    /// </summary>
    private ECoreC _eCoreC;

    //Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        ppos = GameObject.Find("Player").transform.position;
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        playerGO = GameObject.Find("Player");
        _eCoreC = GetComponent<ECoreC>();
        _movingCoroutine = StartCoroutine(ActionBranch());
        pos = transform.position;
        Quaternion rot = transform.localRotation;
        _audioGO.PlayOneShot(expS);
        for (int i = 0; i < 50; i++)
        {
            ExpC shot4 = Instantiate(LightningP, GameData.RandomWindowPosition(), rot);
            shot4.EShot1(Random.Range(0, 360), 0, 0.1f);
        }

        for (int j = 0; j < _eCoreC.hp.Length; j++)
        {
            firstHP += _eCoreC.hp[j];
        }
        _eCoreC.IsBoss = true;
        _gameManaC._bossNowHp = firstHP;
        _gameManaC._bossMaxHp = firstHP;
        _gameManaC._bossName = "Insect";
        _eCoreC.BossLifeMode = 1;
    }

    // Update is called once per frame
    void Update()
    {
        damagePar = _eCoreC.hp[0] * 100 / firstHP;
        pos = transform.position;
        ppos = playerGO.transform.position;
        if (_eCoreC.BossLifeMode != 2) _gameManaC._bossNowHp = _eCoreC.hp[0];
    }

    void FixedUpdate()
    {
        if (_eCoreC.BossLifeMode == 1)
        {
            if (!_isLightning)spriteRenderer.sprite = sprites[(int)spritenumber];
            else spriteRenderer.sprite = sprites[(int)spritenumber + 3];
            spritenumber+=0.3f;
            if (spritenumber>=3 )
            {
                spritenumber = 0;
            }
        }

        //死
        if (_eCoreC.BossLifeMode == 2)
        {
            GameData.Star = true;
            GameData.TimerMoving = false;
            spriteRenderer.sprite = sprites[6];
            AllCoroutineStop();


            Quaternion rot = transform.localRotation;
            ExpC shot4 = Instantiate(LightningP, pos, rot);
            shot4.EShot1(Random.Range(0, 360), 0, 0.1f);

            _gameManaC._bossNowHp = 0;
            transform.localPosition += new Vector3(Random.Range(-10, 10), -5, 0);
            transform.localEulerAngles += new Vector3(0, 0, 10);
            if (pos.y < -64)
            {
                if (GameData.Round == GameData.GoalRound)
                {
                    Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0,0,0)).Summon(0);
                }
                else
                {
                    playerGO.GetComponent<PlayerC>().StageMoveAction();
                }
                Destroy(gameObject);
            }
        }
    }

    //行動変わるヤツ
    private IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.15f);
        _movingCoroutine = StartCoroutine("Charge");
    }

    //発射準備
    private IEnumerator Charge()
    {
        //charge
        _audioGO.PlayOneShot(chargeS);
        for (int j = 0; j < 50; j++)
        {
            Quaternion rot = transform.localRotation;
            ExpC shot4 = Instantiate(LightningP, new Vector3(pos.x + Random.Range(0, 640), pos.y, 0), rot);
            shot4.EShot1(Random.Range(0, 360), 0, 0.1f);
            if (Random.Range(0, 50 - j) <= 2)
            {
                _audioGO.PlayOneShot(effectS);
                _isLightning = true;
                shot4 = Instantiate(LightningP, pos, rot);
                shot4.EShot1(Random.Range(0, 360), 0, 0.1f);

            }
            else _isLightning = false;

            yield return new WaitForSeconds(0.03f);
        }
        _isLightning = true;
        if (damagePar>50) StartCoroutine("FireAction1");
        else if (damagePar>10) StartCoroutine("FireAction2");
        else StartCoroutine("FireAction3");
    }

    //発射一連1
    private IEnumerator FireAction1()
    {
        Fire();
        yield return new WaitForSeconds(0.03f * 20);
        OnAnimatorMove();
    }

    //発射一連2
    private IEnumerator FireAction2()
    {
        for(int j = 0; j < 3; j++)
        {
            Fire();
            yield return new WaitForSeconds(0.03f * 5);
        }
        OnAnimatorMove();
    }

    //発射一連3
    private IEnumerator FireAction3()
    {
        for (int j = 0; j < 6; j++)
        {
            Fire();
            yield return new WaitForSeconds(0.06f);
        }
        OnAnimatorMove();
    }

    //発射
    private void Fire()
    {
        Quaternion rot = transform.localRotation;
        EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
        _audioGO.PlayOneShot(shotS);
        shot.EShot1(0, 0, 3);
    }

    //移動ランダム
    private void OnAnimatorMove()
    {
        _isLightning = false;
        if (Random.Range(0, 2) == 0) _movingCoroutine = StartCoroutine("Moving");
        else _movingCoroutine = StartCoroutine("HomingMoving");
    }

    //移動
    private IEnumerator Moving()
    {
        futurey = Random.Range(2, 5) * 90;
        futurey = (futurey - pos.y) / 20;
        for(int j = 0; j < 20; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    //捕捉
    private IEnumerator HomingMoving()
    {
        futurey = ((int)ppos.y / 90 * 90) + 32;
        futurey = (futurey - pos.y) / 20;
        for (int j = 0; j < 20; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine("ActionBranch");
    }

    //全停止
    private void AllCoroutineStop() {
        StopCoroutine("ActionBranch");
        StopCoroutine("Charge");
        StopCoroutine("FireAction1");
        StopCoroutine("FireAction2");
        StopCoroutine("FireAction3");
        StopCoroutine("Moving");
        _movingCoroutine = null;
    }



}
