using EnumDic.Enemy;
using EnumDic.System;
using Move2DSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBossC : BossCoreC
{
    private float futurey;

    private bool _isLightning;

    [SerializeField]
    private GameObject _raserLight;

    [SerializeField]
    private Transform _tfEyeLight;

    private float spritenumber = 0;


    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private EMissile1C EMissile1Prefab;

    [SerializeField]
    private StayMissileC _prfbStayMissile;

    [SerializeField]
    private RaserC _prfbRaser;

    [SerializeField]
    private ExpC LightningP, _prfRaserDust;

    [SerializeField]
    private AudioClip shotS, chargeS, effectS, expS, _acShortExp;

    protected override void FxUpFight() {
        if (!_isLightning) _srOwnBody.sprite = sprites[(int)spritenumber];
        else _srOwnBody.sprite = sprites[(int)spritenumber + 3];
        spritenumber += 0.3f;
        if (spritenumber >= 3)
        {
            spritenumber = 0;
        }
    }

    protected override void FxUpLeave()
    {
        if (!_isLightning) _srOwnBody.sprite = sprites[(int)spritenumber];
        else _srOwnBody.sprite = sprites[(int)spritenumber + 3];
        spritenumber += 0.3f;
        if (spritenumber >= 3)
        {
            spritenumber = 0;
        }
        base.FxUpLeave();
    }

    protected override void FxUpDead() {
        if (_eCoreC.CheckIsAlive())
        {
            _srOwnBody.sprite = sprites[6];

            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;

            AllCoroutineStop();
            _eCoreC.SetIsAlive(false);
            StartCoroutine(DeadAction());
        }
    }

    private enum MODE_ATTACK
    {
        EnergyBall,
        RaserSweep,
        RaserSnip,
        EnergyBallMulti
    }

    //行動変わるヤツ
    protected override IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.15f);

        ChangeTarget();

        if (_damagePar > 50)
        {
            _movingCoroutine = StartCoroutine(FireAction());
        }
        else
        {
            if(GameData.MultiPlayerCount == 1)
            {
                switch ((MODE_ATTACK)Random.Range(0, System.Enum.GetNames(typeof(MODE_ATTACK)).Length - 1))
                {
                    case MODE_ATTACK.EnergyBall:
                        if (_damagePar > 25) _movingCoroutine = StartCoroutine(FireAction3());
                        else _movingCoroutine = StartCoroutine(FireAction6());
                        break;

                    case MODE_ATTACK.RaserSweep:
                        _movingCoroutine = StartCoroutine(RaserSweep());
                        break;

                    case MODE_ATTACK.RaserSnip:
                        _movingCoroutine = StartCoroutine(RaserSnip());
                        break;
                }
            }
            else
            {
                switch ((MODE_ATTACK)Random.Range(0, System.Enum.GetNames(typeof(MODE_ATTACK)).Length))
                {
                    case MODE_ATTACK.EnergyBall:
                        if (_damagePar > 25) _movingCoroutine = StartCoroutine(FireAction3());
                        else _movingCoroutine = StartCoroutine(Random.Range(0, 2) == 0 && GameData.MultiPlayerCount > 1 ? FireActionMulti() : FireAction6());
                        break;

                    case MODE_ATTACK.EnergyBallMulti:
                        _movingCoroutine = StartCoroutine(FireActionMulti());
                        break;

                    case MODE_ATTACK.RaserSweep:
                        _movingCoroutine = StartCoroutine(RaserSweep());
                        break;

                    case MODE_ATTACK.RaserSnip:
                        _movingCoroutine = StartCoroutine(RaserSnipMulti());
                        break;
                }
            }
            
        }


    }

    //発射準備
    private IEnumerator ChargeEnergyBall()
    {
        //charge
        _audioGO.PlayOneShot(chargeS);
        for (int j = 0; j < 50; j++)
        {
            Quaternion rot = transform.localRotation;
            ExpC shot4 = Instantiate(LightningP, new Vector3(_posOwn.x + Random.Range(0, 640), _posOwn.y, 0), rot);
            shot4.ShotEXP(Random.Range(0, 360), 0, 0.1f);
            if (Random.Range(0, 50 - j) <= 2)
            {
                _audioGO.PlayOneShot(effectS);
                _isLightning = true;
                shot4 = Instantiate(LightningP, _posOwn, rot);
                shot4.ShotEXP(Random.Range(0, 360), 0, 0.1f);

            }
            else _isLightning = false;

            yield return new WaitForFixedUpdate();
        }
        _isLightning = true;
    }

    //発射
    private IEnumerator FireAction()
    {
        yield return StartCoroutine(ChargeEnergyBall());
        Fire(3);
        yield return new WaitForSeconds(0.03f * 20);
        OnMove();
    }

    //発射一連2
    private IEnumerator FireAction3()
    {
        yield return StartCoroutine(ChargeEnergyBall());
        for (int j = 0; j < 3; j++)
        {
            Fire(3);
            yield return new WaitForSeconds(0.03f * 5);
        }
        OnMove();
    }

    //発射一連3
    private IEnumerator FireAction6()
    {
        yield return StartCoroutine(ChargeEnergyBall());
        for (int j = 0; j < 6; j++)
        {
            Fire(3);
            yield return new WaitForSeconds(0.06f);
        }
        OnMove();
    }

    //プレイヤー見つけ次第発射
    private IEnumerator FireActionMulti()
    {
        futurey = 0 * 90;
        futurey = (futurey - _posOwn.y) / 20;
        for (int j = 0; j < 20; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);
            yield return new WaitForFixedUpdate();
        }

        //charge
        _audioGO.PlayOneShot(chargeS);
        for (int j = 0; j < 50; j++)
        {
            Quaternion rot = transform.localRotation;
            ExpC shot4 = Instantiate(LightningP, new Vector3(_posOwn.x, _posOwn.y + Random.Range(0, 480), 0), rot);
            shot4.ShotEXP(Random.Range(0, 360), 0, 0.1f);
            if (Random.Range(0, 50 - j) <= 2)
            {
                _audioGO.PlayOneShot(effectS);
                _isLightning = true;
                shot4 = Instantiate(LightningP, _posOwn, rot);
                shot4.ShotEXP(Random.Range(0, 360), 0, 0.1f);

            }
            else _isLightning = false;

            yield return new WaitForFixedUpdate();
        }
        _isLightning = true;

        futurey = 5 * 90;
        futurey = (futurey - _posOwn.y) / 40;
        List<GameObject> players = new List<GameObject> { };
        List<StayMissileC> stayMissiles = new List<StayMissileC> { };
        for (int j = 0; j < 40; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);

            foreach(GameObject player in _scPlsM.GetAlivePlayers())
            {
                if(transform.position.y >= player.transform.position.y)
                {
                    if (!players.Contains(player))
                    {
                        players.Add(player);
                        StayMissileC missile = Instantiate(_prfbStayMissile, _posOwn, transform.rotation);
                        missile.SetStayMissile(0, 0, 2.0f, 100f);
                        stayMissiles.Add(missile);
                        _audioGO.PlayOneShot(shotS);
                    }
                }
            }
            yield return new WaitForFixedUpdate();
        }

        futurey = 0 * 90;
        futurey = (futurey - _posOwn.y) / 5;
        for (int j = 0; j < 5; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);
            yield return new WaitForFixedUpdate();
        }
        _goCamera.GetComponent<CameraShakeC>().StartShakeVertical(10, 10);
        _audioGO.PlayOneShot(expS);

        yield return new WaitForFixedUpdate();

        _audioGO.PlayOneShot(shotS);
        foreach (StayMissileC missile in stayMissiles)
        {
            missile.SetDeparture();
        }

        yield return new WaitForSeconds(1.2f);

        OnMove();
    }

    /// <summary>
    /// 発射
    /// </summary>
    private void Fire(float speedDelta)
    {
        Instantiate(EMissile1Prefab, _posOwn, transform.rotation).ShotMissile(0, 0, speedDelta);
        _audioGO.PlayOneShot(shotS);
    }


    //発射準備
    private IEnumerator ChargeRaser(List<float> angles, Transform nozzle)
    {
        //charge
        _audioGO.PlayOneShot(chargeS);

        List<Vector3> vecAngles = new List<Vector3> { };
        foreach(float angle in angles)
        {
            vecAngles.Add(Moving2DSystems.GetDirection(angle).normalized);
        }

        for (int j = 0; j < 40; j++)
        {
            for (int i = 0; i < angles.Count; i++)
            {
                Vector3 posEnergySummon = nozzle.position;
                for (int k = 0; k < 512; k++)
                {
                    posEnergySummon += vecAngles[i] * 64;

                    Instantiate(_prfRaserDust, posEnergySummon, transform.rotation)
                        .ShotEXP(angles[i] + Random.Range(0, 2) * 180, 8, 0.1f);

                    if (posEnergySummon.y > GameData.WindowSize.y || posEnergySummon.x > GameData.WindowSize.x) break;
                }
            }

            _isLightning = Random.Range(0, 50 - j) <= 2;

            yield return new WaitForFixedUpdate();

        }
        _isLightning = true;
        _audioGO.PlayOneShot(shotS);
    }

    //レーザーなぎはらい
    private IEnumerator RaserSweep()
    {
        yield return StartCoroutine(ChargeRaser(new List<float>{ 0f}, _tfEyeLight));

        _raserLight.GetComponent<AudioSource>().Play();
        for (int i = 0; i < 20; i++)
        {
            if (Random.Range(0, 4) == 0)
            {

                Instantiate(_prfbRaser, _tfEyeLight.position, transform.rotation).ShotRaser(0,480);
            }
            yield return new WaitForFixedUpdate();
        }

        for (int j = 1; j < 4; j++)
        {
            for (float i = 0f; i < 360f; i += 10f)
            {
                Instantiate(_prfbRaser, _tfEyeLight.position, transform.rotation).ShotRaser(Mathf.Sin(i * Mathf.Deg2Rad) * 9 * j,480);
                yield return new WaitForFixedUpdate();
            }
        }


        for (int i = 0; i < 30; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(_prfbRaser, _tfEyeLight.position, transform.rotation).ShotRaser(0,480);
            }

            yield return new WaitForFixedUpdate();
        }
        _audioGO.PlayOneShot(effectS);
        _raserLight.GetComponent<AudioSource>().Stop();
        OnMove();
    }

    //レーザー狙い撃ち
    private IEnumerator RaserSnip()
    {
        float angle = Moving2DSystems.GetAngle(_raserLight.transform.position, _posPlayer);
        yield return StartCoroutine(ChargeRaser(new List<float> { 0f }, _raserLight.transform));

        _raserLight.GetComponent<AudioSource>().Play();
        for (int i = 0; i < 30; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            }
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 30; i++)
        {
            Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 30; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            }

            yield return new WaitForFixedUpdate();
        }
        _audioGO.PlayOneShot(effectS);
        _raserLight.GetComponent<AudioSource>().Stop();
        OnMove();
    }

    //レーザー狙い撃ち
    private IEnumerator RaserSnipMulti()
    {
        List<float> angles = new List<float> { };

        foreach(GameObject player in _scPlsM.GetAlivePlayers())
        {
            angles.Add(Moving2DSystems.GetAngle(_raserLight.transform.position, player.transform.position));
        }
        yield return StartCoroutine(ChargeRaser(angles, _raserLight.transform));

        _raserLight.GetComponent<AudioSource>().Play();
        for (int i = 0; i < 30; i++)
        {
            foreach(float angle in angles)
            {
                if (Random.Range(0, 4) == 0)
                {
                    Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
                }
            }
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 30; i++)
        {
            foreach (float angle in angles)
            {
                Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            }
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 30; i++)
        {
            foreach (float angle in angles)
            {
                if (Random.Range(0, 4) == 0)
                {
                    Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
                }
            }
            yield return new WaitForFixedUpdate();
        }
        _audioGO.PlayOneShot(effectS);
        _raserLight.GetComponent<AudioSource>().Stop();
        OnMove();
    }

    //移動ランダム
    private void OnMove()
    {
        _isLightning = false;
        if (Random.Range(0, 2) == 0) _movingCoroutine = StartCoroutine(Moving());
        else _movingCoroutine = StartCoroutine(HomingMoving());
    }

    //移動
    private IEnumerator Moving()
    {
        futurey = Random.Range(2, 5) * 90;
        futurey = (futurey - _posOwn.y) / 20;
        for (int j = 0; j < 20; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);
            yield return new WaitForFixedUpdate();
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    //捕捉
    private IEnumerator HomingMoving()
    {
        futurey = ((int)_posPlayer.y / 90 * 90) + 32;
        futurey = (futurey - _posOwn.y) / 20;
        for (int j = 0; j < 20; j++)
        {
            transform.localPosition += new Vector3(0, futurey, 0);
            yield return new WaitForFixedUpdate();
        }
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    protected override IEnumerator ArrivalAction()
    {
        Vector3 summonPos = new Vector3(64, Random.Range(2, 5) * 90, 0);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                ExpC shot4 = Instantiate(LightningP, summonPos + new Vector3(Random.Range(-64, 64), Random.Range(-64, 64), 0), transform.rotation);
                shot4.ShotEXP(Random.Range(0, 360), 0, 0.1f);
            }
            _audioGO.PlayOneShot(_acShortExp);
            yield return new WaitForSeconds(0.3f);
        }

        for (int j = 0; j < 100; j++)
        {
            ExpC shot4 = Instantiate(LightningP, GameData.GetRandomWindowPosition(), transform.rotation);
            shot4.ShotEXP(Random.Range(0, 360), 0, 0.12f);
        }
        _audioGO.PlayOneShot(expS);
        yield return new WaitForSeconds(0.3f);
        transform.position = summonPos;

        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    protected override IEnumerator DeadAction()
    {
        while (_posOwn.y > -64)
        {
            Instantiate(LightningP, _posOwn, transform.localRotation).ShotEXP(Random.Range(0, 360), 0, 0.1f);

            _gameManaC._bossNowHp = 0;
            transform.localPosition += new Vector3(Random.Range(-10, 10), -5, 0);
            _tfOwnBody.eulerAngles += new Vector3(0, 0, 10);

            yield return new WaitForFixedUpdate();
        }
        DoCollapuse();
    }

    protected override IEnumerator LeaveAction()
    {
        float angle = Moving2DSystems.GetAngle(_raserLight.transform.position, _posPlayer);

        _raserLight.GetComponent<AudioSource>().Play();
        for (int i = 0; i < 10; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            }
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 10; i++)
        {
            Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 10; i++)
        {
            if (Random.Range(0, 4) == 0)
            {
                Instantiate(_prfbRaser, _raserLight.transform.position, transform.rotation).ShotRaser(angle,480);
            }

            yield return new WaitForFixedUpdate();
        }
        _audioGO.PlayOneShot(effectS);
        _raserLight.GetComponent<AudioSource>().Stop();

        for (int i = 0; i < 1028; i+=2)
        {
            transform.position += transform.up*i;
            yield return new WaitForFixedUpdate();
        }
    }
}
