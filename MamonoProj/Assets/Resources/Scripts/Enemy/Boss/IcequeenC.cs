using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcequeenC : MonoBehaviour
{
    private int _attackVariation = 0;

    private Vector3[] ftree;

    private Vector3 pos, ppos;

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

    public StaffRollC StaffPrefab;

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
        _audioGO = GameObject.Find("AudioManager").GetComponent<AudioSource>();

        playerGO = GameObject.Find("Player");
        _eCoreC = GetComponent<ECoreC>();
        _eCoreC.IsBoss = true;

        ftree = new Vector3[20];
        GM = GameObject.Find("GameManager");
        GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];
        GM.GetComponent<GameManagement>()._bossMaxHp = _eCoreC.hp[0];
    }

    // Update is called once per frame
    void Update()
    {
        rot = transform.localRotation;

        pos = transform.position;
        ppos = playerGO.transform.position;

        if (_eCoreC.BossLifeMode == 0)
        {
            pos = transform.position;
            for (i = 0; i < 100; i++)
            {
                float angle = i * 3.6f;
                Quaternion rot = transform.localRotation;
                ExpC shot = Instantiate(ExpPrefab, pos, rot);
                shot.EShot1(angle, 10, 5);
            }

            FloorManagerC.StageGimic(100, 1);
            _movingCoroutine = StartCoroutine(ActionBranch());
            _eCoreC.BossLifeMode = 1;
        }

    }

    void FixedUpdate()
    {
        if (_eCoreC.BossLifeMode != 2) GM.GetComponent<GameManagement>()._bossNowHp = _eCoreC.hp[0];

        //死
        if (_eCoreC.BossLifeMode == 2)
        {
            GameData.Star = true;
            FloorManagerC.StageGimic(100, 0);
            GameData.TimerMoving = false;
            AllCoroutineStop();

            GM.GetComponent<GameManagement>()._bossNowHp = 0;
            if (GameData.Round == GameData.GoalRound)
            {
                Instantiate(StaffPrefab, new Vector3(320, -100, 0), Quaternion.Euler(0, 0, 0)).Summon(0);
            }
            else
            {
                playerGO.GetComponent<PlayerC>().StageMoveAction();
            }

            Destroy(gameObject);
        }
    }


    /// <summary>
    /// 行動変わるヤツ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionBranch()
    {
        spriteRenderer.sprite = normal;
        yield return new WaitForSeconds(0.15f);
        _attackVariation= Random.Range(0, 5);
        /*if (ppos.x > pos.x - 196 && pos.x + 196 > ppos.x&& ppos.y > pos.y - 196 && pos.y + 196 > ppos.y)
        {
            _movingCoroutine = StartCoroutine(Move());
        }*/
        if (_attackVariation == 0) _movingCoroutine = StartCoroutine(Move());
        else if (_attackVariation == 1) _movingCoroutine = StartCoroutine(YBeam());
        else if (_attackVariation == 2) _movingCoroutine = StartCoroutine(AllFrost());
        else if (_attackVariation == 3) _movingCoroutine = StartCoroutine(DelayFrost()) ;
        else if (_attackVariation == 4) _movingCoroutine = StartCoroutine(SinIce()) ;
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.6f);
        spriteRenderer.sprite = lightning;

        Vector3 goalPos= ppos-(transform.up*16)/*new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5),64), 0)*/;

        
        for (i = 0; i < 360; i += 10)
        {
            Vector3 direction = goalPos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 200, Mathf.Sin(i * Mathf.Deg2Rad) * 200, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(FlostEP, direction, rot).EShot1(0, 0, 1.0f);
        }

        FloorManagerC.StageGimic(100,0);
        for (int hoge = 0; hoge < 33; hoge++)
        {
            Instantiate(FlostEP, pos+(((goalPos-pos)/33)*hoge), rot).EShot1(0, 0, 1-(0.03f*hoge));
            yield return new WaitForSeconds(0.03f);
        }

        FloorManagerC.StageGimic(100,1);
        for (i = 0; i < 360; i += 10)
        {
            Vector3 direction = goalPos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 200, Mathf.Sin(i * Mathf.Deg2Rad) * 200, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(iceP, direction, rot).EShot1(0, 0, 1.0f);
        }
        _audioGO.PlayOneShot(loveS);
        Instantiate(SnowPrefab, pos, rot);
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
    private IEnumerator YBeam()
    {
        Vector3 movePos = GameData.GetSneaking(pos, new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0, 5), 64), 0), 40);

        FloorManagerC.StageGimic(100, 1);
        spriteRenderer.sprite = lightning;
        yield return new WaitForSeconds(0.6f);
        for (int j = 0; j < 4; j++)
        {
            Vector3 direction = new Vector3(Random.Range(0, 640), 420, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(LovePrefab, direction, rot).EShot1();
            _audioGO.PlayOneShot(loveS);
            for(int k = 0; k < 10; k++)
            {
                transform.position += movePos;
            }
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 全力氷塊生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator AllFrost()
    {
        FloorManagerC.StageGimic(100,0);
        yield return new WaitForSeconds(0.6f);

        Vector3 movePos = GameData.GetSneaking(pos, new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0, 5),64), 0),40);

        spriteRenderer.sprite = lightning;
        for (i = 0; i < 20; i++)
        {
            ftree[i] = new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5),48), 0);
        }

        FloorManagerC.StageGimic(100, 0);
        for (int j = 0; j < 40; j++) 
        {
            FloorManagerC.StageGimic(2, 1);
            Quaternion rot = transform.localRotation;
            for (i = 0; i < 20; i++)
            {
                float angle = Random.Range(0, 360);
                ExpC shot = Instantiate(FlostEP, ftree[i] + new Vector3(Random.Range(-16, 16), Random.Range(-36, 4), 0), rot);
                shot.EShot1(angle, 1, 0.3f);
            }
            transform.position += movePos;
            yield return new WaitForSeconds(0.03f);
        }
        for (i = 0; i < 20; i++)
        {
            BombC shot = Instantiate(FlostP, ftree[i], rot);
            shot.EShot1(0, 0, 0, Random.Range(15, 40), 10, 0.5f);

        }
        _audioGO.PlayOneShot(iceS);

        FloorManagerC.StageGimic(100,0);

        yield return new WaitForSeconds(1.0f);
        _movingCoroutine = StartCoroutine(Move());
    }


    /// <summary>
    /// 間隔氷塊生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayFrost()
    {
        FloorManagerC.StageGimic(100,0);
        yield return new WaitForSeconds(1.0f);

        Vector3 movePos = GameData.GetSneaking(pos, new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0, 5), 64), 0), 140);

        spriteRenderer.sprite = lightning;
        for(int j = 0; j < 10; j++)
        {
            FloorManagerC.StageGimic(30 ,1);
            for (i = 0; i < 3; i++)
            {
                ftree[i] = new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5),48), 0);
            }
            for (int k = 0; k < 14; k++)
            {
                for (i = 0; i < 3; i++)
                {
                    Quaternion rot = transform.localRotation;
                    float angle = Random.Range(0, 360);
                    ExpC shot = Instantiate(FlostEP, ftree[i] + new Vector3(Random.Range(-16, 16), Random.Range(-36, 4), 0), rot);
                    shot.EShot1(angle, 1, 0.3f);
                }
                transform.position += movePos;
                yield return new WaitForSeconds(0.03f);
            }

            for (i = 0; i < 3; i++)
            {
                Quaternion rot = transform.localRotation;
                BombC shot = Instantiate(FlostP, ftree[i], rot);
                shot.EShot1(0, 0, 0, Random.Range(15, 40), 10, 0.5f);
            }
            _audioGO.PlayOneShot(iceS);
        }
        _movingCoroutine = StartCoroutine(Move()); ;
    }

    /// <summary>
    /// Sin氷塊
    /// </summary>
    /// <returns></returns>
    private IEnumerator SinIce()
    {
        FloorManagerC.StageGimic(100, 1);

        yield return new WaitForSeconds(0.6f);
        spriteRenderer.sprite = lightning;
        
        for (float fi = 0; fi < 40; fi++)
        {
            Vector3 shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 7) + (fi / 15)) * 220, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(FlostEP, shutu, rot).EShot1(0, 0, 1.2f);
            shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 7) - (fi / 15)) * 220, 0);
            Instantiate(FlostEP, shutu, rot).EShot1(0, 0, 1.2f);
        }

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
        StopCoroutine(ActionBranch());
        StopCoroutine(YBeam());
        StopCoroutine(AllFrost());
        StopCoroutine(DelayFrost());
        StopCoroutine(SinIce());
        StopCoroutine(Move());
        _movingCoroutine = null;
    }
}
