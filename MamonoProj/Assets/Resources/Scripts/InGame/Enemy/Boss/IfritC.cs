using System.Collections;
using UnityEngine;
using EnumDic.Stage;
using EnumDic.Enemy;
using Move2DSystem;

public class IfritC : BossCoreC
{
    private MODE_ATTACK _attackVariation = 0;
    private MODE_ATTACK _attackBefore;
    private int _lookAngle = 0;

    private float _angle;

    private float _moveX = 0, _modeDelta = 0,_moveY = 0;

    private Vector3  target, face, fireworklockon;
   

    /// <summary>
    /// 軸固定
    /// </summary>
    private bool _isLockLookAngle = false;

    [SerializeField]
    private BombC BombPrefab;

    [SerializeField]
    private ExpC ExpPrefab, PowderPrefab;

    [SerializeField]
    private AudioClip fireS, volS, wingS;

    /// <summary>
    /// アニメーター
    /// </summary>
    private Animator _animOwn;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        GetComponents();
    }

    private void GetComponents()
    {
        _animOwn = _tfOwnBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();


    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        if (_modeDelta != 0) transform.localPosition += Moving2DSystems.GetSneaking(_posOwn, _posPlayer, _modeDelta);
    }

    protected override void FxUpArrival()
    {
        if (!_isLockLookAngle) _srOwnBody.flipX = _posOwn.x <= _posPlayer.x;
    }

    protected override void FxUpFight()
    {
        if (!_isLockLookAngle) _srOwnBody.flipX = _posOwn.x <= _posPlayer.x;
    }

    protected override void FxUpDead()
    {
        if (_eCoreC.CheckIsAlive())
        {
            FloorManagerC.SetStageGimic(100, 0);

            GameData.IsInvincible = true;
            GameData.IsTimerMoving = false;

            AllCoroutineStop();
            _eCoreC.SetIsAlive(false);
            StartCoroutine(DeadAction());
        }
        _gameManaC._bossNowHp = 0;
    }

    private enum MODE_ATTACK
    {
        Firework,
        Volcano,
        FireAttack
    }

    /// <summary>
    /// 行動分岐
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ActionBranch()
    {
        yield return new WaitForSeconds(0.15f);

        ChangeTarget();

        do
        {
            _attackVariation = (MODE_ATTACK)Random.Range(0, 3);
        } while (_attackVariation == _attackBefore);
        _attackBefore = _attackVariation;

        switch (_attackVariation)
        {
            case MODE_ATTACK.Firework:
                _movingCoroutine = StartCoroutine(FireWork());
                break;

            case MODE_ATTACK.Volcano:
                Volcano();
                break;

            case MODE_ATTACK.FireAttack:
                _movingCoroutine = StartCoroutine(FireAttack());
                break;
        }
    }

    /// <summary>
    /// 火球放射
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWork()
    {
        _modeDelta = 100;
        for (int j = 0; j < 25; j++)
        {
            yield return StartCoroutine(ChargeFire());
        }

        _animOwn.SetBool("IsSpeedUp", true);

        float targetAngle = Moving2DSystems.GetAngle(_posOwn, _posPlayer);
        for (int j = 0; j < 25; j++)
        {
            yield return StartCoroutine(ChargeFire());
        }

        yield return StartCoroutine(FireWorks(targetAngle));
        _audioGO.PlayOneShot(fireS);
        _animOwn.SetBool("IsSpeedUp", false);

        yield return new WaitForSeconds(0.6f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }
    
    /// <summary>
    /// 爆発火球放射
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireWorks(float targetAngle)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Instantiate(BombPrefab, _posOwn, _rotOwn).ShotBomb(targetAngle + Random.Range(-30, 30), 12 + (i * 1.2f), -0.6f, 20 + (i * 2), 128);
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    /// <summary>
    /// 炎吸収エフェクト
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChargeFire()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 tar = _posOwn + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0);
            Vector3 direction2 = _posOwn - tar + new Vector3(0, 30, 0);
            Instantiate(ExpPrefab, tar, _rotOwn).ShotEXP(Moving2DSystems.GetAngle(tar + new Vector3(0, 30, 0), _posOwn), 10, 0.1f);
        }
        yield return new WaitForSeconds(0.03f);
    }


    /// <summary>
    /// 火炎放射
    /// </summary>
    /// <returns></returns>
    private void Volcano()
    {
        _modeDelta = 0;
        if (100 >= _damagePar && _damagePar > 60)
        {
            _movingCoroutine = StartCoroutine(VolcanoBreath());
        }

        else if (60 >= _damagePar && _damagePar > 30)
        {
            _movingCoroutine = StartCoroutine(VolcanoHorming());
        }

        else
        {
            _movingCoroutine = StartCoroutine(VolcanoSlash());
        }

    }

    /// <summary>
    /// 火の粉突進
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireAttack()
    {
        _animOwn.SetBool("IsSpeedUp", true);
        _modeDelta = 0;
        for (int j = 0; j < 20; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                _angle = Random.Range(0, 360);
                Quaternion rot3 = transform.localRotation;
                ExpC shot3 = Instantiate(ExpPrefab, _posOwn, rot3);
                shot3.ShotEXP(_angle, 1 + ((100 - _damagePar) / 30), 0.8f);
                yield return new WaitForSeconds(0.03f);
            }
        }
        _modeDelta = 50;
        _audioGO.PlayOneShot(volS);
        for (int j = 0; j < 30; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                _angle = Random.Range(0, 360);
                ExpC shot3 = Instantiate(ExpPrefab, _posOwn, _rotOwn);
                shot3.ShotEXP(_angle, 4 + ((100 - _damagePar) / 30), 0.8f);
                yield return new WaitForSeconds(0.03f);
            }
        }

        _animOwn.SetBool("IsSpeedUp", false);

        yield return new WaitForSeconds(1.2f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }


    /// <summary>
    /// 広範囲火炎放射0
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoBreath()
    {
        _animOwn.SetBool("IsSpeedUp", true);
        for (int j = 0; j < 40; j++)
        {
            if (j < 20)
            {
                if (_posOwn.x > _posPlayer.x)
                {
                    _lookAngle = 0;//L
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    _lookAngle = 1;//R
                    face = _posOwn + new Vector3(12, 9, 0);
                }
            }
            else _isLockLookAngle = true;

            for (int i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = Random.Range(180 - (60 - _damagePar / 2), 180 + (60 - _damagePar / 2));
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = Random.Range(-60 + _damagePar / 2, 61 - _damagePar / 2);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(PowderPrefab, face, _rotOwn);
                shot2.ShotEXP(_angle, Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {

            for (int i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = Random.Range(180 - (60 - _damagePar / 2), 180 + (60 - _damagePar / 2));
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = Random.Range(-60 + _damagePar / 2, 61 - _damagePar / 2);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(ExpPrefab, face, _rotOwn);
                shot2.ShotEXP(_angle, Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _isLockLookAngle = false;
        _animOwn.SetBool("IsSpeedUp",false);
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// 追跡火炎放射1
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoHorming()
    {
        _animOwn.SetBool("IsSpeedUp", true);
        for (int j = 0; j < 40; j++)
        {
            if (j < 20)
            {
                target = _posPlayer;
                if (_posOwn.x > _posPlayer.x)
                {
                    _lookAngle = 0;//L
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    _lookAngle = 1;//R
                    face = _posOwn + new Vector3(12, 9, 0);
                }
            }
            else _isLockLookAngle = true;

            for (int i = 0; i < 10; i++)
            {

                Vector3 direction = target - _posOwn;
                float angle = Moving2DSystems.GetAngle(_posOwn, target) + Random.Range(-20, 20);
                ExpC shot2 = Instantiate(PowderPrefab, face, _rotOwn);
                shot2.ShotEXP(angle, Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {
            for (int i = 0; i < 10; i++)
            {

                Vector3 direction = target - _posOwn;
                float angle = Moving2DSystems.GetAngle(_posOwn, target) + Random.Range(-20, 20);
                ExpC shot2 = Instantiate(ExpPrefab, face, _rotOwn);
                shot2.ShotEXP(angle, Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _isLockLookAngle = false;
        _animOwn.SetBool("IsSpeedUp", false);
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    /// <summary>
    /// なぎはらい火炎放射2
    /// </summary>
    /// <returns></returns>
    private IEnumerator VolcanoSlash()
    {
        target = _posPlayer;
        _isLockLookAngle = true;
        _animOwn.SetBool("IsSpeedUp", true);

        for (int j = 0; j < 30; j++)
        {

            for (int i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = 270 + ((-30 + j) * (-30 + j) * 0.03f);
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = 270 - ((-30 + j) * (-30 + j) * 0.03f);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(PowderPrefab, face, _rotOwn);
                shot2.ShotEXP(_angle + Random.Range(-10, 10), Random.Range(15, 135), 0.4f);
            }
            yield return new WaitForSeconds(0.03f);

        }
        _audioGO.PlayOneShot(volS);

        for (int j = 0; j < 70; j++)
        {

            for (int i = 0; i < 10; i++)
            {
                if (_lookAngle == 0)
                {
                    transform.localPosition += new Vector3(0.05f, 0, 0);
                    _angle = 270 - (j * j * 0.03f);
                    face = _posOwn + new Vector3(-12, 9, 0);
                }
                else
                {
                    transform.localPosition += new Vector3(-0.05f, 0, 0);
                    _angle = 270 + (j * j * 0.03f);
                    face = _posOwn + new Vector3(12, 9, 0);
                }
                ExpC shot2 = Instantiate(ExpPrefab, face, _rotOwn);
                shot2.ShotEXP(_angle + Random.Range(-10, 10), Random.Range(15, 135), 0.4f);
            }

            yield return new WaitForSeconds(0.03f);

        }

        _isLockLookAngle = false;
        _animOwn.SetBool("IsSpeedUp",false);
        yield return new WaitForSeconds(0.3f);
        _movingCoroutine = StartCoroutine(ActionBranch());
    }

    protected override void AllCoroutineStop()
    {
        StopAllCoroutines();
        _movingCoroutine = null;
    }

    protected override IEnumerator ArrivalAction()
    {
        Vector3 summonPos = new Vector3(transform.position.x, Random.Range(50, 330), 0);

        for (int i = 0; i< 30; i++)
        {
            transform.position+= Moving2DSystems.GetSneaking(_posOwn, summonPos, 10);
            yield return new WaitForSeconds(0.03f);
        }

        _audioGO.PlayOneShot(fireS);
        foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (floor.GetComponent<FloorC>()._isBedRock)
            {
                float distance = Moving2DSystems.GetDistance(_posOwn, floor.transform.position);
                Instantiate(BombPrefab, _posOwn, _rotOwn)
                    .ShotBomb(Moving2DSystems.GetAngle(_posOwn, floor.transform.position), distance / 20, -distance/800, 30, 64);
            }
        }
        yield return new WaitForSeconds(0.99f);

        FloorManagerC.SetGimicBedRock(MODE_FLOOR.PreBurning);

        _eCoreC.SetModeBossLife(MODE_LIFE.Fight);
        _movingCoroutine = StartCoroutine(FireWork());
    }

    /// <summary>
    /// 死アクション
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator DeadAction()
    {
        _modeDelta = 0;
        _eCoreC.SummonItems();
        _tfOwnBody.eulerAngles = new Vector3(180, 0, 0);
        for (int k = 0; k < 3; k++)
        {
            for (int j = 0; j < 5; j++)
            {
                for (int l = 0; l < 10; l++)
                {

                    Instantiate(PowderPrefab, _posOwn, _rotOwn).ShotEXP(Random.Range(0, 360), 10 + (j * 10), 0.4f);
                }
            }

            for (int j = 0; j < 30; j++)
            {
                transform.position += new Vector3(Random.Range(-10, 10), 0, 0);
                yield return new WaitForSeconds(0.03f);
            }
        }
        for (int j = 0; j < 50; j++)
        {
            Instantiate(PowderPrefab, _posOwn, _rotOwn).ShotEXP(Random.Range(0, 360), 20, 0.4f);
        }
        DoCollapuse();
    }

    protected override IEnumerator LeaveAction()
    {
        for(int j = 0; j < 3; j++)
        {
            for (int angle = 0; angle < 360; angle += 30)
            {
                Instantiate(BombPrefab, _posOwn, _rotOwn).ShotBomb(angle+Random.Range(-15,15), 30, -1, 40, 128);
            }
            FloorManagerC.SetStageGimic(40, MODE_FLOOR.PreBurning);
            yield return new WaitForSeconds(0.3f);
        }

        FloorManagerC.SetStageGimic(100, MODE_FLOOR.PreBurning);

        yield return base.LeaveAction();
    }
}
