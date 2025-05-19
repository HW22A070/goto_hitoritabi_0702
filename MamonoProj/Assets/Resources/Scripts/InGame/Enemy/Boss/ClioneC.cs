using System.Collections;
using UnityEngine;
using EnumDic.Stage;
using EnumDic.Enemy;
using Move2DSystem;

public class ClioneC : BossCoreC
{
    private MODE_ATTACK _attackVariation = 0;

    private Vector3[] _treePoses=  new Vector3[20];
    public Sprite normal, lightning;

    public LoveC LovePrefab;
    public ExpC ExpPrefab,iceP,FlostEP,_eatPfb;
    public HowitzerC FlostP;
    public ECoreC SnowPrefab;

    private int i;

    public AudioClip loveS,iceS,_acEat;

    protected override void FxUpDead()
    {
        if (_eCoreC.CheckIsAlive())
        {
            GameData.IsInvincible = true;
            FloorManagerC.SetStageGimic(100, 0);
            GameData.IsTimerMoving = false;
            AllCoroutineStop();
            _eCoreC.SetIsAlive(false);
            StartCoroutine(DeadAction());
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
    protected override IEnumerator ActionBranch()
    {
        _srOwnBody.sprite = normal;
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

        ChangeTarget();

        _srOwnBody.sprite = lightning;

        Vector3 goalPos= _posPlayer-(transform.up*16)/*new Vector3(Random.Range(16, 624), GameData.GroundPutY(Random.Range(0,5),64), 0)*/;


        //プレイヤーを閉じ込める
        float hole = Random.Range(0, 360);

        for (i = 0; i < 360; i += 10)
        {
            if (!(hole - 22 <= i && i <= hole + 22))
            {
                Vector3 direction = goalPos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 200, Mathf.Sin(i * Mathf.Deg2Rad) * 200, 0);
                Quaternion rot = transform.localRotation;
                Instantiate(FlostEP, direction, rot).ShotEXP(0, 0, 0.6f);
            }
        }

        yield return new WaitForSeconds(0.6f);

        _audioGO.PlayOneShot(loveS);
        for (i = 0; i < 360; i += 10)
        {
            if (!(hole - 22 <= i && i <= hole + 22))
            {
                Vector3 direction = goalPos + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * 200, Mathf.Sin(i * Mathf.Deg2Rad) * 200, 0);
                Quaternion rot = transform.localRotation;
                Instantiate(iceP, direction, rot).ShotEXP(0, 0, 3.0f);
            }

        }

        FloorManagerC.SetStageGimic(100,MODE_FLOOR.Normal);
        for (int hoge = 0; hoge < 33; hoge++)
        {
            Instantiate(FlostEP, _posOwn+(((goalPos-_posOwn)/33)*hoge), _rotOwn).ShotEXP(0, 0, 2-(0.03f*hoge));
            yield return new WaitForSeconds(0.03f);
        }

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
        _audioGO.PlayOneShot(_acEat);
        Instantiate(SnowPrefab, _posOwn, _rotOwn);
        transform.localPosition = goalPos;

        //バッカルコーン
        ExpC eatP = Instantiate(_eatPfb, goalPos + (transform.up * 32), _rotOwn);
        eatP.transform.parent = gameObject.transform;
        eatP.ShotEXP(0, 0, 0.2f);

        yield return new WaitForSeconds(2.0f);

        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 縦方向ビーム
    /// </summary>
    /// <returns></returns>
    private IEnumerator VerticalBeam()
    {
        Vector3 movePos = Moving2DSystems.GetSneaking(_posOwn, new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0, 5), 64), 0), 40);

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
        _srOwnBody.sprite = lightning;
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

        Vector3 movePos = Moving2DSystems.GetSneaking(_posOwn, new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0, 5),64), 0),60);

        _srOwnBody.sprite = lightning;
        for (i = 0; i < 20; i++)
        {
            _treePoses[i] = new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0,5),48), 0);
        }

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.Normal);
        for (int j = 0; j < 60; j++) 
        {
            FloorManagerC.SetStageGimic(2, MODE_FLOOR.IceFloor);
            Quaternion rot = transform.localRotation;
            for (i = 0; i < 20; i++)
            {
                float angle = Random.Range(0, 360);
                Instantiate(FlostEP, _treePoses[i] + new Vector3(Random.Range(-16, 16), Random.Range(-36, 4), 0), rot)
                    .ShotEXP(angle, 1, 0.3f);
            }
            transform.position += movePos;
            yield return new WaitForSeconds(0.03f);
        }
        for (i = 0; i < 20; i++)
        {
            HowitzerC shot = Instantiate(FlostP, _treePoses[i], _rotOwn);
            shot.ShotHowitzer(0, 0, 0, Random.Range(15, 40), 10, 0.5f);

        }
        _audioGO.PlayOneShot(iceS);

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.Normal);

        yield return new WaitForSeconds(1.5f);
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

        Vector3 movePos = Moving2DSystems.GetSneaking(_posOwn, new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0, 5), 64), 0), 140);

        _srOwnBody.sprite = lightning;
        for(int j = 0; j < 7; j++)
        {
            FloorManagerC.SetStageGimic(30 , MODE_FLOOR.IceFloor);
            for (i = 0; i < 3; i++)
            {
                _treePoses[i] = new Vector3(Random.Range(16, 624), GameData.GetGroundPutY(Random.Range(0,5),48), 0);
            }
            for (int k = 0; k < 20; k++)
            {
                for (i = 0; i < 3; i++)
                {
                    Quaternion rot = transform.localRotation;
                    float angle = Random.Range(0, 360);
                    Instantiate(FlostEP, _treePoses[i] + new Vector3(Random.Range(-16, 16), Random.Range(-36, 4), 0), rot)
                        .ShotEXP(angle, 1, 0.3f);
                }
                transform.position += movePos;
                yield return new WaitForSeconds(0.03f);
            }

            for (i = 0; i < 3; i++)
            {
                Quaternion rot = transform.localRotation;
                HowitzerC shot = Instantiate(FlostP, _treePoses[i], rot);
                shot.ShotHowitzer(0, 0, 0, Random.Range(15, 40), 10, 0.5f);
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
        _srOwnBody.sprite = lightning;
        
        //予告
        for (float fi = 0; fi < 40; fi++)
        {
            Vector3 shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 7) + (fi / 15)) * 220, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(FlostEP, shutu, rot).ShotEXP(0, 0, 2.2f);
            shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 7) - (fi / 15)) * 220, 0);
            Instantiate(FlostEP, shutu, rot).ShotEXP(0, 0, 1.2f);
        }

        yield return new WaitForSeconds(1.0f);

        //結晶
        for (float fi=0;fi<40;fi++)
        {
            Vector3 shutu = new Vector3(fi * 16, 240 + Mathf.Sin((fi / 7) + (fi / 15)) * 220, 0);
            Quaternion rot = transform.localRotation;
            Instantiate(iceP, shutu, rot).ShotEXP(0, 0, 0.5f);
            shutu = new Vector3(fi * 16, 240 + Mathf.Sin(-(fi / 7) - (fi / 15)) * 220, 0);
            Instantiate(iceP, shutu, rot).ShotEXP(0, 0, 0.5f);
            yield return new WaitForSeconds(0.03f);
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }
    

    //全停止
    protected override void AllCoroutineStop()
    {
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
            _movingCoroutine = null;
        }
    }

    protected override IEnumerator ArrivalAction()
    {

        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 360; i+=20)
            {
                Instantiate(FlostEP, _posOwn + new Vector3(Mathf.Sin(i * Mathf.Deg2Rad) * 30*j, Mathf.Cos(i * Mathf.Deg2Rad) * 30*j, 0), _rotOwn).ShotEXP(i * 10, 0, Random.Range(1.0f,3.0f));
            }
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(3.0f);

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.IceFloor);
        _movingCoroutine = StartCoroutine(ActionBranch());
        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
    }

    protected override IEnumerator DeadAction()
    {
        while (_posOwn.y > -64)
        {

            _gameManaC._bossNowHp = 0;
            transform.localPosition += new Vector3(Random.Range(-10, 10), -5, 0);
            _tfOwnBody.eulerAngles += new Vector3(0, 0, 10);

            yield return new WaitForFixedUpdate();
        }
        DoCollapuse();
    }

    protected override IEnumerator LeaveAction()
    {
        yield return new WaitForSeconds(1.0f);

        _audioGO.PlayOneShot(loveS);
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 360; i += 20)
            {
                Instantiate(FlostEP, _posOwn + new Vector3(Mathf.Sin(i * Mathf.Deg2Rad) * 30 * j, Mathf.Cos(i * Mathf.Deg2Rad) * 30 * j, 0), _rotOwn).ShotEXP(i * 10, 0, Random.Range(1.0f, 3.0f));
            }
        }

        Destroy(gameObject);
    }
}
