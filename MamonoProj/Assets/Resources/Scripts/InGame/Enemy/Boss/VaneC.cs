using EnumDic.Enemy;
using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaneC : BossCoreC
{
    private int _action;

    private float angle;

    private bool _islastMode;

    private int _windowGraValue = 4;

    private Vector3  _moveGoal;

    [SerializeField, Tooltip("LFR")]
    private Sprite[] normal, angry, last, death;

    [SerializeField]
    private ShurikenC F1, F2, F3;

    [SerializeField]
    private EMissile1C SonicPrefab;

    [SerializeField]
    private ExpC _prfbEfWind;

    [SerializeField]
    private AudioClip sonicS, woodS, deathexpS, shoutS;

    /// <summary>
    /// バリア
    /// </summary>
    [SerializeField]
    private GameObject _barrier;

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (_eCoreC.GetModeBossLife()!=MODE_LIFE.Dead) _gameManaC._bossNowHp = _eCoreC.TotalHp;
        _damagePar = _eCoreC.TotalHp * 100 / _firstHP;

        switch (_eCoreC.GetModeBossLife())
        {
            case MODE_LIFE.Arrival:
                break;


            case MODE_LIFE.Fight:
                //Normal
                if (_eCoreC.EvoltionMode == 0)
                {
                    _eCoreC.EvoltionMode = 0;
                    _srOwnBody.sprite = normal[_windowGraValue];
                }
                //Angly
                else if (_eCoreC.EvoltionMode == 1)
                {
                    _barrier.SetActive(false);
                    _eCoreC.EvoltionMode = 1;
                    _srOwnBody.sprite = angry[_windowGraValue];
                }
                //Rage
                else if (_eCoreC.EvoltionMode == 2)
                {
                    _eCoreC.EvoltionMode = 2;
                    _srOwnBody.sprite = last[_windowGraValue];
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

                break;

            case MODE_LIFE.Dead:
                break;
        }

        if (GameData.WindSpeed < -30) _windowGraValue = 0;
        else if (-30 <= GameData.WindSpeed && GameData.WindSpeed < -7) _windowGraValue = 1;
        else if (-7 <= GameData.WindSpeed && GameData.WindSpeed < 7) _windowGraValue = 2;
        else if (7 <= GameData.WindSpeed && GameData.WindSpeed < 30) _windowGraValue = 3;
        else if (30 <= GameData.WindSpeed) _windowGraValue = 4;

    }

    protected override void FxUpArrival()
    {
        GameData.WindSpeed = 10;
        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
    }

    protected override void FxUpDead()
    {
        if (_eCoreC.CheckIsAlive())
        {
            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;

            AllCoroutineStop();
            _eCoreC.SetIsAlive(false);
            StartCoroutine(DeadAction());
        }

        if (_posOwn.y < 96) transform.position += new Vector3(0, Random.Range(1, 20), 0);
        else if (_posOwn.y > 384) transform.position += new Vector3(0, Random.Range(-20, 0), 0);
        else transform.position += new Vector3(0, Random.Range(-10, 11), 0);

        if (_posOwn.x > _posPlayer.x) GameData.WindSpeed = Random.Range(100, 501);
        else GameData.WindSpeed = Random.Range(-500, -99);
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
    protected override IEnumerator ActionBranch()
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

        for (int i = 0; i < 100; i++)
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
        if (_eCoreC.EvoltionMode == 1) StartCoroutine(ActionShotFlying(KIND_FLYINGOBJ.Wood));
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 右向きに風を起こす
    /// </summary>
    /// <returns></returns>
    private IEnumerator SonicLtoR()
    {
        _moveGoal = new Vector3(540, (Random.Range(0, 3) * 90) + 110, 0) - _posOwn;

        for (int i = 0; i < 100; i++)
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
        ChangeTarget();

        float angle = Moving2DSystems.GetAngle(_posOwn, _posPlayer);
        angle += Random.Range(-10, 10);
        Quaternion rot = transform.localRotation;
        EMissile1C shot = Instantiate(SonicPrefab, _posOwn, rot);
        shot.ShotMissile(angle, 0, 0.3f);
    }

    /// <summary>
    /// 暴走
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartHurricane()
    {
        //飛来物発射までの時間稼ぎ
        for (int i = 0; i < Random.Range(10, 20); i++)
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
            StartCoroutine(ActionShotFlying((KIND_FLYINGOBJ)Random.Range(0, 3)));
        }
        _movingCoroutine = StartCoroutine(StartHurricane());
    }

    private IEnumerator ActionShotFlying(KIND_FLYINGOBJ objKind)
    {
        float posY = Random.Range(0, 480);
        int size = 0;
        switch (objKind)
        {
            case KIND_FLYINGOBJ.Wood:
                size = 196 / 2;
                break;
            case KIND_FLYINGOBJ.Cristal:
                size = 128 / 2;
                break;
            case KIND_FLYINGOBJ.DeadMonstar:
                size = 96 / 2;
                break;
        }
        for (int i=0;i<33;i++)
        {
            Instantiate(_prfbEfWind, new Vector3(0, posY+ Random.Range(-size, size), 0), _rotOwn).ShotEXP(0,Random.Range(48,180),1.0f);
            yield return new WaitForSeconds(0.03f);
        }
        ShotFlyingObj(objKind, posY);
    }

    /// <summary>
    /// 飛来物を飛ばす
    /// </summary>
    /// <param name="objKind">
    /// 0=Wood
    /// 1=Cristal
    /// 2=Dead
    /// </param>
    private void ShotFlyingObj(KIND_FLYINGOBJ objKind,float posY)
    {
        Quaternion rot = transform.localRotation;
        rot.z = Random.Range(0, 360);
        _audioGO.PlayOneShot(woodS);

        switch (objKind)
        {
            case KIND_FLYINGOBJ.Wood:
                Instantiate(F1, new Vector3(-48, posY, 0), rot)
                    .ShotShuriken(0, Random.Range(10, 30), 0.1f, Random.Range(5, 20));
                break;

            case KIND_FLYINGOBJ.Cristal:
                Instantiate(F2, new Vector3(-48, posY, 0), rot)
                    .ShotShuriken(0, Random.Range(10, 30), 0.1f, Random.Range(5, 15));
                break;

            case KIND_FLYINGOBJ.DeadMonstar:
                Instantiate(F3, new Vector3(-48, posY, 0), rot)
                    .ShotShuriken(0, Random.Range(10, 30), 0.1f, Random.Range(5, 10));
                break;
        }
    }

    protected override IEnumerator ArrivalAction()
    {
        for(int i = 0; i < 32; i++)
        {
            GameData.WindSpeed = i;
            yield return new WaitForSeconds(0.03f);
        }
        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    protected override IEnumerator DeadAction()
    {
        for(int j = 0; j < 40; j++)
        {
            _gameManaC._bossNowHp = 0;

            yield return new WaitForFixedUpdate();
        }

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 30; i++)
            {
                float angle = Moving2DSystems.GetAngle(_posOwn, _posPlayer);
                angle += Random.Range(10, 350);
                Quaternion rot = transform.localRotation;
                EMissile1C shot = Instantiate(SonicPrefab, _posOwn, rot);
                shot.ShotMissile(angle, 10 + (10 * j), 0);
            }
        }
        GameData.WindSpeed = 0;
        _audioGO.PlayOneShot(sonicS);
        _eCoreC.SummonItems();

        DoCollapuse();

    }

    protected override IEnumerator LeaveAction()
    {
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForFixedUpdate();
            GameData.WindSpeed *=0.8f;
        }
        GameData.WindSpeed =0;
        for (int i = 0; i < 1028; i += 2)
        {
            transform.position += transform.up * i;
            yield return new WaitForFixedUpdate();
        }
    }
}
