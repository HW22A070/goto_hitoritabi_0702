using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDic.Floor;
using EnumDic.Enemy;

public class IcequeenC : MonoBehaviour
{
    private MODE_ATTACK _attackVariation = 0;

    private Vector3[] _treePoses=  new Vector3[20];

    private Vector3 _posOwn, _posPlayer;

    public SpriteRenderer spriteRenderer;
    public Sprite normal, lightning;

    public LoveC LovePrefab;
    public ExpC ExpPrefab,iceP,FlostEP,_eatPfb;
    public BombC FlostP;
    public ECoreC SnowPrefab;

    private int i;

    private GameObject GM;

    /// <summary>
    /// スピーカ
    /// </summary>
    private AudioSource _audioGO;

    public AudioClip loveS,iceS;

    private Quaternion rot;

    public ClearEffectC StaffPrefab;

    /// <summary>
    /// PlayerGameObject
    /// </summary>
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
        GetComponents();

        _eCoreC.IsBoss = true;

    }

    private void GetComponents()
    {
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        playerGO = GameObject.Find("Player");
        _eCoreC = GetComponent<ECoreC>();
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
    }

    // Update is called once per frame
    void Update()
    {
        rot = transform.localRotation;

        _posOwn = transform.position;
        _posPlayer = playerGO.transform.position;

        if (_eCoreC.BossLifeMode == 0)
        {
            _posOwn = transform.position;
            for (i = 0; i < 100; i++)
            {
                float angle = i * 3.6f;
                Quaternion rot = transform.localRotation;
                ExpC shot = Instantiate(ExpPrefab, _posOwn, rot);
                shot.EShot1(angle, 10, 5);
            }

            FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
            _movingCoroutine = StartCoroutine(ActionBranch());
            _eCoreC.BossLifeMode = MODE_LIFE.Fight;
        }

    }

    void FixedUpdate()
    {
        if (_eCoreC.BossLifeMode != MODE_LIFE.Dead) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        //死
        if (_eCoreC.BossLifeMode == MODE_LIFE.Dead)
        {
            GameData.IsInvincible = true;
            FloorManagerC.SetStageGimic(100, 0);
            GameData.IsTimerMoving = false;
            AllCoroutineStop();

            GM.GetComponent<GameManagement>()._bossNowHp = 0;
            if (GameData.Round == GameData.GoalRound)
            {
                Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0));
            }
            else
            {
                playerGO.GetComponent<PlayerC>().StageMoveAction();
            }

            Destroy(gameObject);
        }
    }

    private enum MODE_ATTACK {
        MoveEat,
        VerticalBeam,
        FrostAll,
        FlostFinely,
        IceSin
    }


    /// <summary>
    /// 行動変わるヤツ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionBranch()
    {
        spriteRenderer.sprite = normal;
        yield return new WaitForSeconds(0.15f);
        _attackVariation= (MODE_ATTACK)Random.Range(0, System.Enum.GetNames(typeof(MODE_ATTACK)).Length);

        switch (_attackVariation)
        {
            case MODE_ATTACK.MoveEat:
                _movingCoroutine = StartCoroutine(MoveEat());
                break;

            case MODE_ATTACK.VerticalBeam:
                _movingCoroutine = StartCoroutine(VerticalBeam());
                break;

            case MODE_ATTACK.FrostAll:
                _movingCoroutine = StartCoroutine(AllFrost());
                break;

            case MODE_ATTACK.FlostFinely:
                _movingCoroutine = StartCoroutine(DelayFrost());
                break;

            case MODE_ATTACK.IceSin:
                _movingCoroutine = StartCoroutine(SinIce());
                break;
        }
    }

    /// <summary>
    /// 移動＆バッカルコーン攻撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveEat()
    {
        yield return new WaitForSeconds(0.6f);
        spriteRenderer.sprite = lightning;

        Vector3 goalPos= _posPlayer-(transform.up*16)/*new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5),64), 0)*/;


        //プレイヤーを閉じ込める
        float hole = Random.Range(0, 360);
        _audioGO.PlayOneShot(loveS);
        for (i = 0; i < 360; i += 10)
        {
            if (!(hole - 22 <= i && i <= hole + 22))
            {
                Vector3 direction = goalPos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 200, Mathf.Sin(i * Mathf.Deg2Rad) * 200, 0);
                Quaternion rot = transform.localRotation;
                Instantiate(iceP, direction, rot).EShot1(0, 0, 3.0f);
            }
        }

        FloorManagerC.SetStageGimic(100,MODE_FLOOR.Normal);
        for (int hoge = 0; hoge < 33; hoge++)
        {
            Instantiate(FlostEP, _posOwn+(((goalPos-_posOwn)/33)*hoge), rot).EShot1(0, 0, 2-(0.03f*hoge));
            yield return new WaitForSeconds(0.03f);
        }

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
        _audioGO.PlayOneShot(loveS);
        Instantiate(SnowPrefab, _posOwn, rot);
        transform.localPosition = goalPos;

        //バッカルコーン
        ExpC eatP = Instantiate(_eatPfb, goalPos + (transform.up * 32), rot);
        eatP.transform.parent = gameObject.transform;
        eatP.EShot1(0, 0, 0.2f);

        yield return new WaitForSeconds(0.6f);

        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 縦方向ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator VerticalBeam()
    {
        Vector3 movePos = GameData.GetSneaking(_posOwn, new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0, 5), 64), 0), 40);

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
        spriteRenderer.sprite = lightning;
        yield return new WaitForSeconds(0.6f);
        for (int j = 0; j < 4; j++)
        {
            Vector3 direction = new Vector3(Random.Range(0, 640), 420, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(LovePrefab, direction, rot);
            _audioGO.PlayOneShot(loveS);
            for(int k = 0; k < 10; k++)
            {
                transform.position += movePos;
            }
        }
        yield return new WaitForSeconds(0.6f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 全力氷塊生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator AllFrost()
    {
        FloorManagerC.SetStageGimic(100,MODE_FLOOR.Normal);
        yield return new WaitForSeconds(0.6f);

        Vector3 movePos = GameData.GetSneaking(_posOwn, new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0, 5),64), 0),40);

        spriteRenderer.sprite = lightning;
        for (i = 0; i < 20; i++)
        {
            _treePoses[i] = new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0,5),48), 0);
        }

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.Normal);
        for (int j = 0; j < 40; j++) 
        {
            FloorManagerC.SetStageGimic(2, MODE_FLOOR.IceFloor);
            Quaternion rot = transform.localRotation;
            for (i = 0; i < 20; i++)
            {
                float angle = Random.Range(0, 360);
                ExpC shot = Instantiate(FlostEP, _treePoses[i] + new Vector3(Random.Range(-16, 16), Random.Range(-36, 4), 0), rot);
                shot.EShot1(angle, 1, 0.3f);
            }
            transform.position += movePos;
            yield return new WaitForSeconds(0.03f);
        }
        for (i = 0; i < 20; i++)
        {
            BombC shot = Instantiate(FlostP, _treePoses[i], rot);
            shot.EShot1(0, 0, 0, Random.Range(15, 40), 10, 0.5f);

        }
        _audioGO.PlayOneShot(iceS);

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.Normal);

        yield return new WaitForSeconds(1.0f);
        _movingCoroutine = StartCoroutine(MoveEat());
    }


    /// <summary>
    /// 間隔氷塊生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayFrost()
    {
        FloorManagerC.SetStageGimic(100,0);
        yield return new WaitForSeconds(1.0f);

        Vector3 movePos = GameData.GetSneaking(_posOwn, new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0, 5), 64), 0), 140);

        spriteRenderer.sprite = lightning;
        for(int j = 0; j < 10; j++)
        {
            FloorManagerC.SetStageGimic(30 , MODE_FLOOR.IceFloor);
            for (i = 0; i < 3; i++)
            {
                _treePoses[i] = new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0,5),48), 0);
            }
            for (int k = 0; k < 14; k++)
            {
                for (i = 0; i < 3; i++)
                {
                    Quaternion rot = transform.localRotation;
                    float angle = Random.Range(0, 360);
                    ExpC shot = Instantiate(FlostEP, _treePoses[i] + new Vector3(Random.Range(-16, 16), Random.Range(-36, 4), 0), rot);
                    shot.EShot1(angle, 1, 0.3f);
                }
                transform.position += movePos;
                yield return new WaitForSeconds(0.03f);
            }

            for (i = 0; i < 3; i++)
            {
                Quaternion rot = transform.localRotation;
                BombC shot = Instantiate(FlostP, _treePoses[i], rot);
                shot.EShot1(0, 0, 0, Random.Range(15, 40), 10, 0.5f);
            }
            _audioGO.PlayOneShot(iceS);
        }
        _movingCoroutine = StartCoroutine(MoveEat()); ;
    }

    /// <summary>
    /// Sin結晶
    /// </summary>
    /// <returns></returns>
    private IEnumerator SinIce()
    {
        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);

        yield return new WaitForSeconds(0.6f);
        spriteRenderer.sprite = lightning;
        
        //予告
        for (float fi = 0; fi < 40; fi++)
        {
            Vector3 shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 7) + (fi / 15)) * 220, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(FlostEP, shutu, rot).EShot1(0, 0, 1.2f);
            shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 7) - (fi / 15)) * 220, 0);
            Instantiate(FlostEP, shutu, rot).EShot1(0, 0, 1.2f);
        }

        //結晶
        for (float fi=0;fi<40;fi++)
        {
            Vector3 shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 7) + (fi / 15)) * 220, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(iceP, shutu, rot).EShot1(0, 0, 0.5f);
            shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 7) - (fi / 15)) * 220, 0);
            Instantiate(iceP, shutu, rot).EShot1(0, 0, 0.5f);
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }
    

    //全停止
    private void AllCoroutineStop()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }
}
