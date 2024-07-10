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

    [SerializeField]
    private  SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public EMissile1C EMissile1Prefab;
    public ExpC LightningP;
    
    private GameManagement _gameManaC;
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

    }

    // Update is called once per frame
    void Update()
    {
        damagePar = _eCoreC.hp[0] * 100 / firstHP;
        pos = transform.position;
        ppos = playerGO.transform.position;
        if (_eCoreC.BossLifeMode != 2) _gameManaC._bossNowHp = _eCoreC.hp[0];
    }

    public void Summon(int judge)
    {
        _eCoreC = GetComponent<ECoreC>();
        _gameManaC = GameObject.Find("GameManager").GetComponent<GameManagement>();
        firstHP = _eCoreC.hp[0];
        playerGO = GameObject.Find("Player");
        _gameManaC._bossNowHp = _eCoreC.hp[0];
        _gameManaC._bossMaxHp = _eCoreC.hp[0];
        _gameManaC._bossName = "Insect";
        _movingCoroutine = StartCoroutine(ActionBranch());
        _eCoreC.IsBoss = true;
        pos = transform.position;
        ppos = GameObject.Find("Player").transform.position;
        Quaternion rot = transform.localRotation;
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(expS);
        for (int i = 0; i < 50; i++)
        {
            ExpC shot4 = Instantiate(LightningP, GameData.RandomWindowPosition(), rot);
            shot4.EShot1(Random.Range(0, 360), 0, 0.1f);
        }
        _eCoreC.BossLifeMode = 1;

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
                playerGO.GetComponent<PlayerC>().StageMoveAction();
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
        Debug.Log(damagePar);
        //charge
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(chargeS);
        for (int j = 0; j < 50; j++)
        {
            Quaternion rot = transform.localRotation;
            ExpC shot4 = Instantiate(LightningP, new Vector3(pos.x + Random.Range(0, 640), pos.y, 0), rot);
            shot4.EShot1(Random.Range(0, 360), 0, 0.1f);
            if (Random.Range(0, 50 - j) <= 2)
            {
                GameObject.FindObjectOfType<AudioSource>().PlayOneShot(effectS);
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
        Vector3 direction = new Vector3(0, 0, 0);
        float angle = GetAngle(direction);
        Quaternion rot = transform.localRotation;
        EMissile1C shot = Instantiate(EMissile1Prefab, pos, rot);
        GameObject.FindObjectOfType<AudioSource>().PlayOneShot(shotS);
        shot.EShot1(angle, 0, 3);
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

    public float GetAngle(Vector2 direction)
    {
        float rad = Mathf.Atan2(direction.y, direction.x);
        return rad * Mathf.Rad2Deg;
    }

}
